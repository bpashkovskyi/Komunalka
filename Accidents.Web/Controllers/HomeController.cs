using Accidents.Model;
using Accidents.Web.Models;

using Komunalka.Persistence;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Accidents.Web.Controllers;

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
    [ProducesResponseType(typeof(List<AccidentViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccidents(AccidentSearchViewModel accidentSearchViewModel)
    {
        var accidents = await _komunalkaContext.Accidents.ToListAsync();
        accidents = accidentSearchViewModel.Filter(accidents);

        var accidentViewModels = accidents.Select(accident => new AccidentViewModel(accident));

        return Ok(accidentViewModels);
    }

    [HttpGet("filters")]
    [ProducesResponseType(typeof(AccidentFilterViewModel), StatusCodes.Status200OK)]

    public async Task<IActionResult> Index()
    {
        var accidents = await _komunalkaContext.Accidents.ToListAsync();

        return Ok(new AccidentFilterViewModel(accidents));
    }

    ////[HttpPost("setLocation")]
    ////[ProducesResponseType(typeof(AccidentFilterViewModel), StatusCodes.Status200OK)]

    ////public async Task<IActionResult> SetMarker(SetAccidentLocationViewModel setAccidentLocationViewModel)
    ////{
    ////    var accident = await this._komunalkaContext.Accidents.FindAsync(setAccidentLocationViewModel.Id);

    ////    if (accident.Coordinates == null)
    ////    {
    ////        accident.Coordinates = new Coordinates();
    ////    }

    ////    accident.Coordinates.ManuallyCorrectedLatitude = setAccidentLocationViewModel.Lat;
    ////    accident.Coordinates.ManuallyCorrectedLongitude = setAccidentLocationViewModel.Lng;

    ////    await _komunalkaContext.SaveChangesAsync();

    ////    return Ok();
    ////}
}