using System.Globalization;

using Komunalka.Persistence;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Protocols.Web.Models;

namespace Protocols.Web.Controllers;

public class HomeController : Controller
{
    private readonly KomunalkaContext _komunalkaContext;

    public HomeController(KomunalkaContext komunalkaContext)
    {
        _komunalkaContext = komunalkaContext;
    }

    [Route("")]
    public async Task<IActionResult> Index()
    {
        var protocols = await this._komunalkaContext.Protocols
            .Include(protocol => protocol.Items)
            .OrderByDescending(protocol => protocol.Date)
            .ThenBy(protocol => protocol.Number)
            .ToListAsync();

        var protocolsViewModel = protocols.Select(protocol => new ProtocolListViewModel
        {
            Id = protocol.Id,
            Date = protocol.Date.ToString("dd.MM.yyyy", CultureInfo.CreateSpecificCulture("uk-UA")),
            Number = protocol.Number,
            ItemsCount = protocol.Items.Count,
        });

        return View(protocolsViewModel);
    }

    [Route("protocol")]

    public async Task<IActionResult> Details(int id)
    {
        var protocol = await this._komunalkaContext.Protocols
            .Include(protocol => protocol.Items)
            .FirstOrDefaultAsync(protocol => protocol.Id == id);

        if (protocol == null)
        {
            return NotFound();
        }

        var protocolDetailsViewModel = new ProtocolDetailsViewModel
        {
            Id = protocol.Id,
            Date = protocol.Date.ToString("dd.MM.yyyy", CultureInfo.CreateSpecificCulture("uk-UA")),
            Number = protocol.Number,
            Items = protocol.Items.OrderBy(item => item.OrderNumber).Select(item => new ItemViewModel
            {
                Number = item.Number,
                Heard = item.Heard,
                Decided = item.Decided,
            }).ToList()
        };

        return View(protocolDetailsViewModel);
    }

    [Route("download")]
    public async Task<IActionResult> Download(int id)
    {
        var protocol = await this._komunalkaContext.Protocols
            .Include(protocol => protocol.Items)
            .FirstOrDefaultAsync(protocol => protocol.Id == id);

        if (protocol == null)
        {
            return NotFound();
        }

        var fileName = $"Протокол №{protocol.Number} від {protocol.Date.ToString("dd.MM.yyyy", CultureInfo.CreateSpecificCulture("uk-UA"))} року.pdf";
        var protocolFile = await System.IO.File.ReadAllBytesAsync($"wwwroot/data/{fileName}");

        return new FileContentResult(protocolFile, "application/pdf");
    }

    [Route("search")]
    public async Task<IActionResult> Search(string searchString)
    {
        var items = await this._komunalkaContext.Items.Include(item => item.Protocol)
            .Where(item => item.Heard.Contains(searchString) || item.Decided.Contains(searchString))
        .ToListAsync();

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
}