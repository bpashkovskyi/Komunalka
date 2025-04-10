using Komunalka.Persistence;

using Light.Web.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Light.Web.Controllers;

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
    [ProducesResponseType(typeof(List<PointViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccidents(PointsSearchViewModel pointsSearchViewModel)
    {
        var points = await _komunalkaContext.Points.Where(p => p.Coordinates.OriginalLatitude != null).ToListAsync().ConfigureAwait(false);
        points = pointsSearchViewModel.Filter(points);

        var pointViewModels = points.Select(accident => new PointViewModel(accident));

        return Ok(pointViewModels);
    }

    [HttpGet("filters")]
    [ProducesResponseType(typeof(PointFilterViewModel), StatusCodes.Status200OK)]

    public async Task<IActionResult> Index()
    {
        var points = await _komunalkaContext.Points.Where(p => p.Coordinates.OriginalLatitude != null).ToListAsync().ConfigureAwait(false);

        return Ok(new PointFilterViewModel(points));
    }
}