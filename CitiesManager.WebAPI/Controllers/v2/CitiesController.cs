using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using CitiesManager.Infrastructure.DatabaseContext;

namespace CitiesManager.WebAPI.Controllers.v2
{
  [ApiVersion("2.0")]
  public class CitiesController : CustomControllerBase
  {
    private readonly ApplicationDbContext _context;
    public CitiesController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: api/Cities
    /// <summary>
    /// To get list of cities from cities table from version 2.0
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    //[Produces("application/xml")]
    public async Task<ActionResult<IEnumerable<string>>> GetCities()
    {
      var cities =await _context.Cities
        .OrderBy(temp => temp.CityName)
        .Select(temp=> temp.CityName)
        .ToListAsync();
      return cities;
    }
  }
}