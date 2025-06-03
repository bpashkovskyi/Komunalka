using Advertising.Model;
using Advertising.Web.Models;
using Komunalka.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Advertising.Web.Controllers;

[ApiController]
[Route("")]
public class HomeController : Controller
{
    private readonly KomunalkaContext _komunalkaContext;

    public HomeController(KomunalkaContext komunalkaContext)
    {
        _komunalkaContext = komunalkaContext;
    }

    [HttpPost("accidents")]
    [ProducesResponseType(typeof(List<BoardViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccidents(BoardSearchViewModel boardSearchViewModel)
    {
        var boards = await _komunalkaContext.Boards.ToListAsync().ConfigureAwait(false);
        boards = boardSearchViewModel.Filter(boards);

        var boardViewModels = boards.Select(accident => new BoardViewModel(accident));

        return Ok(boardViewModels);
    }

    [HttpGet("filters")]
    [ProducesResponseType(typeof(BoardFilterViewModel), StatusCodes.Status200OK)]

    public async Task<IActionResult> Index()
    {
        var boards = await _komunalkaContext.Boards.ToListAsync().ConfigureAwait(false);

        return Ok(new BoardFilterViewModel(boards));
    }


    [HttpPost("setLocation")]
    [ProducesResponseType(typeof(BoardFilterViewModel), StatusCodes.Status200OK)]

    public async Task<IActionResult> SetMarker(SetBoardLocationViewModel setBoardLocationViewModel)
    {
        var accident = await this._komunalkaContext.Boards.FindAsync(setBoardLocationViewModel.Id);

        if (accident.Coordinates == null)
        {
            accident.Coordinates = new Coordinates();
        }

        accident.Coordinates.ManuallyCorrectedLatitude = setBoardLocationViewModel.Lat;
        accident.Coordinates.ManuallyCorrectedLongitude = setBoardLocationViewModel.Lng;

        await _komunalkaContext.SaveChangesAsync();

        return Ok();
    }
}

