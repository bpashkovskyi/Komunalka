using System.Text;

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

    [HttpGet("report")]
    [ProducesResponseType(typeof(AccidentFilterViewModel), StatusCodes.Status200OK)]

    public async Task<IActionResult> Report(int dist = 50, int year = 2024, int count = 3)
    {
        var accidents = await _komunalkaContext.Accidents
            .Where(a => a.Timestamp.Year >= year)
            .ToListAsync();

        var clusters = Clusterization.Clusterize(accidents, dist, count);

        return View(clusters);
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

