using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.WebAPI.Model;
using CitiesManager.WebAPI.DatabaseContext;

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
  [Produces("application/json")]
  public async Task<ActionResult<IEnumerable<City>>> GetCities()
  {
    return await _context.Cities.ToListAsync();
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

    var city = await _context.Cities.FirstOrDefaultAsync(temp => temp.CityId == cityid);

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
  public async Task<IActionResult> PutCity(System.Guid? cityID, [Bind(nameof(City.CityId), nameof(City.CityName))] City city)
  {
    if (cityID != city.CityId)
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
  public async Task<ActionResult<City>> PostCity([Bind(nameof(City.CityId), nameof(City.CityName))] City city)
  {
    _context.Cities.Add(city);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetCities", new { cityid = city.CityId }, city);  //201
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
    return _context.Cities.Any(e => e.CityId == cityid);
  }
}
