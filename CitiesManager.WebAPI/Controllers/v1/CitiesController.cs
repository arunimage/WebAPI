using Asp.Versioning;
using CitiesManager.Core.Entities;
using CitiesManager.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.Controllers.v1
{
  [ApiVersion("1.0")]
  //[EnableCors("4100Client")]
  public class CitiesController : CustomControllerBase
  {
    private readonly ApplicationDbContext _context;
    public CitiesController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: api/Cities
    /// <summary>
    /// To get list of cities from cities table
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    //[Produces("application/xml")]
    public async Task<ActionResult<IEnumerable<City>>> GetCities()
    {
      var cities = await _context.Cities
        .OrderBy(temp => temp.CityName)
        .ToListAsync();
      return cities;
    }

    // GET: api/City/5
    /// <summary>
    /// To get city by provided city id
    /// </summary>
    /// <param name="cityid"></param>
    /// <returns></returns>
    [HttpGet("{cityid}")]
    public async Task<ActionResult<City>> GetCity(System.Guid cityid)
    {
      //var city = await _context.Cities.FindAsync(cityid);

      var city = await _context.Cities.FirstOrDefaultAsync(temp => temp.CityID == cityid);

      if (city == null)
      {
        return Problem(detail: "Invalid CityID", statusCode: 400, title: "City Search");
        //return NotFound();
      }

      return city;
    }

    // PUT: api/City/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{cityID}")]
    public async Task<IActionResult> PutCity(System.Guid? cityID, [Bind(nameof(City.CityID), nameof(City.CityName))] City city)
    {
      if (cityID != city.CityID)
      {
        return BadRequest();  //400
      }
      var existingCity = await _context.Cities.FindAsync(cityID);

      if (existingCity == null)
      {
        return NotFound(); //404
      }

      existingCity.CityName = city.CityName;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!CityExists(cityID))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }
      return NoContent();  //204
    }

    // POST: api/City
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<City>> PostCity([Bind(nameof(City.CityID), nameof(City.CityName))] City city)
    {
      _context.Cities.Add(city);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetCities", new { cityid = city.CityID }, city);  //201
    }

    // DELETE: api/City/5
    [HttpDelete("{cityid}")]
    public async Task<IActionResult> DeleteCity(System.Guid? cityid)
    {
      var city = await _context.Cities.FindAsync(cityid);
      if (city == null)
      {
        return NotFound();  //404
      }

      _context.Cities.Remove(city);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool CityExists(System.Guid? cityid)
    {
      return _context.Cities.Any(e => e.CityID == cityid);
    }
  }
}