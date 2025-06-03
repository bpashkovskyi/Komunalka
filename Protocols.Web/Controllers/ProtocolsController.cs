using Komunalka.Persistence;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Protocols.Model;
using Protocols.Web.Models;
#pragma warning disable CA2007

namespace Protocols.Web.Controllers;

public class ProtocolsController(KomunalkaContext komunalkaContext, IProtocolParser protocolParser)
    : Controller
{
    private const string ProtocolsFolder = "wwwroot/data";

    [Route("")]
    public async Task<IActionResult> Index()
    {
        var protocols = await komunalkaContext.Protocols
            .Include(protocol => protocol.Items)
            .OrderByDescending(protocol => protocol.Date)
            .ThenBy(protocol => protocol.Number).ToListAsync();

        var protocolsViewModel = protocols.Select(protocol => new ProtocolListViewModel(protocol));
        

        return View(protocolsViewModel);
    }

    [Route("protocol")]

    public async Task<IActionResult> Details(int id)
    {
        var protocol = await komunalkaContext.Protocols
            .Include(protocol => protocol.Items)
            .FirstOrDefaultAsync(protocol => protocol.Id == id);

        if (protocol == null)
        {
            return NotFound();
        }

        var protocolDetailsViewModel = new ProtocolDetailsViewModel(protocol);

        return View(protocolDetailsViewModel);
    }

    [Route("download")]
    public async Task<IActionResult> Download(int id)
    {
        var protocol = await komunalkaContext.Protocols
            .Include(protocol => protocol.Items)
            .FirstOrDefaultAsync(protocol => protocol.Id == id);

        if (protocol == null)
        {
            return NotFound();
        }

        var filePath = Path.Combine(ProtocolsFolder, protocol.FileName);
        var protocolFile = await System.IO.File.ReadAllBytesAsync(filePath);

        return new FileContentResult(protocolFile, "application/pdf");
    }

    [Route("search")]
    public async Task<IActionResult> Search(string searchString)
    {
        var items = await komunalkaContext.Items.Include(item => item.Protocol)
            .Where(item => item.Heard.Contains(searchString) || item.Decided.Contains(searchString))
            .OrderByDescending(item => item.Protocol.Date)
            .ToListAsync();

        var itemSearchViewModels = items.Select(item => new ItemSearchViewModel(item));

        return View(itemSearchViewModels);
    }

    [Authorize]
    [HttpGet]
    [Route("import")]

    public IActionResult Import()
    {
        return View(new ProtocolImportViewModel());
    }

    [Authorize]
    [HttpPost]
    [Route("import")]
    public async Task<IActionResult> Import(ProtocolImportViewModel protocolImportViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(protocolImportViewModel);
            }

            if (protocolImportViewModel.File != null)
            {
                var protocol = protocolParser.Parse(protocolImportViewModel.File.OpenReadStream(),
                    out List<string> validationErrors);

                protocol.Date = DateTime.ParseExact(protocolImportViewModel.Date, "dd.MM.yyyy", null);
                protocol.Number = protocolImportViewModel.Number;

                if (validationErrors.Count > 0)
                {
                    foreach (var validationError in validationErrors)
                    {
                        ModelState[nameof(ProtocolImportViewModel.File)]?.Errors.Add(validationError);
                    }

                    return View(protocolImportViewModel);
                }

                await SaveProtocolToFileSystem(protocolImportViewModel.File, protocol);
                await SaveProtocolToDatabase(protocol);
            }

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return Ok(e.Message);
        }
        
    }

    [Authorize]
    [Route("edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var protocol = await komunalkaContext.Protocols
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (protocol == null)
            return NotFound();

        var model = new ProtocolEditViewModel
        {
            Id = protocol.Id,
            Number = protocol.Number,
            Date = protocol.Date.ToString("dd.MM.yyyy"),
            Items = protocol.Items
                .OrderBy(item => item.OrderNumber)
                .Select(item => new ItemEditViewModel
                {
                    Id = item.Id,
                    OrderNumber = item.OrderNumber,
                    Number = item.Number,
                    Heard = item.Heard,
                    Decided = item.Decided
                }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [Route("edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProtocolEditViewModel protocolEditViewModel)
    {
        var protocol = await komunalkaContext.Protocols
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == protocolEditViewModel.Id);

        if (protocol == null)
        {
            return NotFound();
        }

        protocol.Number = protocolEditViewModel.Number;
        protocol.Date = DateTime.ParseExact(protocolEditViewModel.Date, "dd.MM.yyyy", null); ;

        // Очистимо всі старі пункти
        komunalkaContext.Items.RemoveRange(protocol.Items);

        // Додаємо нові
        protocol.Items = protocolEditViewModel.Items.Select((item, index) => new Item
        {
            Id = item.Id ?? 0,
            OrderNumber = index + 1,
            Number = item.Number,
            Heard = item.Heard,
            Decided = item.Decided ?? "foo",
            ProtocolId = protocol.Id
        }).ToList();

        await komunalkaContext.SaveChangesAsync();

        return RedirectToAction("Details", new { id = protocolEditViewModel.Id });
    }

    private async Task SaveProtocolToDatabase(Protocol protocol)
    {
        await komunalkaContext.AddAsync(protocol);
        await komunalkaContext.SaveChangesAsync();
    }

    private static async Task SaveProtocolToFileSystem(IFormFile formFile, Protocol protocol)
    {
        var filePath = Path.Combine(ProtocolsFolder, protocol.FileName);

        await using var stream = new FileStream(filePath, FileMode.Create);

        await formFile.CopyToAsync(stream);
    }
}