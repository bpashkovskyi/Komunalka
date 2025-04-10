using System.Globalization;

using Komunalka.Persistence;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Protocols.Model;
using Protocols.Web.Models;

namespace Protocols.Web.Controllers;

public class ProtocolsController : Controller
{
    private readonly KomunalkaContext _komunalkaContext;
    private readonly IProtocolParser _protocolParser;

    private const string ProtocolsFolder = "wwwroot/data";

    public ProtocolsController(KomunalkaContext komunalkaContext, IProtocolParser protocolParser)
    {
        _komunalkaContext = komunalkaContext;
        _protocolParser = protocolParser;
    }

    [Route("")]
    public async Task<IActionResult> Index()
    {
        var protocols = await this._komunalkaContext.Protocols
            .Include(protocol => protocol.Items)
            .OrderByDescending(protocol => protocol.Date)
            .ThenBy(protocol => protocol.Number).ToListAsync().ConfigureAwait(false);

        var protocolsViewModel = protocols.Select(ProtocolListViewModel.FromProtocol);
        

        return View(protocolsViewModel);
    }

    [Route("protocol")]

    public async Task<IActionResult> Details(int id)
    {
        var protocol = await this._komunalkaContext.Protocols
            .Include(protocol => protocol.Items)
            .FirstOrDefaultAsync(protocol => protocol.Id == id).ConfigureAwait(false);

        if (protocol == null)
        {
            return NotFound();
        }

        var protocolDetailsViewModel = ProtocolDetailsViewModel.FromProtocol(protocol);

        return View(protocolDetailsViewModel);
    }

    [Route("download")]
    public async Task<IActionResult> Download(int id)
    {
        var protocol = await this._komunalkaContext.Protocols
            .Include(protocol => protocol.Items)
            .FirstOrDefaultAsync(protocol => protocol.Id == id).ConfigureAwait(false);

        if (protocol == null)
        {
            return NotFound();
        }

        var filePath = Path.Combine(ProtocolsFolder, protocol.FileName);
        var protocolFile = await System.IO.File.ReadAllBytesAsync(filePath).ConfigureAwait(false);

        return new FileContentResult(protocolFile, "application/pdf");
    }

    [Route("search")]
    public async Task<IActionResult> Search(string searchString)
    {
        var items = await this._komunalkaContext.Items.Include(item => item.Protocol)
            .Where(item => item.Heard.Contains(searchString) || item.Decided.Contains(searchString))
            .OrderByDescending(item => item.Protocol.Date)
            .ToListAsync().ConfigureAwait(false);

        var itemSearchViewModels = items.Select(item => new ItemSearchViewModel
        {
            ProtocolId = item.Protocol.Id,
            ProtocolNumber = item.Protocol.Number,
            ProtocolDate = item.Protocol.Date.ToString("dd.MM.yyyy", CultureInfo.CreateSpecificCulture("uk-UA")),
            Number = item.Number,
            Heard = item.Heard,
            Decided = item.Decided
        });

        return View(itemSearchViewModels);
    }

    [HttpGet]
    [Route("import")]

    public IActionResult Import()
    {
        return View(new ProtocolImportViewModel());
    }

    [HttpPost]
    [Route("import")]
    public async Task<IActionResult> Import(ProtocolImportViewModel protocolImportViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(protocolImportViewModel);
        }

        if (protocolImportViewModel.File != null)
        {
            var protocol = this._protocolParser.Parse(protocolImportViewModel.File.OpenReadStream(), out List<string> validationErrors);
            protocol.Date = DateTime.ParseExact(protocolImportViewModel.Date, "dd.MM.yyyy", null);
            protocol.Number = protocolImportViewModel.Number;

            if (validationErrors.Count > 0)
            {
                foreach (var validationError in validationErrors)
                {
                    ModelState[nameof(ProtocolImportViewModel.File)].Errors.Add(validationError);
                    return View(protocolImportViewModel);
                }
            }
            else
            {
                var filePath = Path.Combine(ProtocolsFolder, protocol.FileName);

                var stream = new FileStream(filePath, FileMode.Create);

                await protocolImportViewModel.File.CopyToAsync(stream).ConfigureAwait(false);
                await _komunalkaContext.AddAsync(protocol).ConfigureAwait(false);
                await _komunalkaContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        return RedirectToAction("Index");
    }
}