using Microsoft.AspNetCore.Mvc;
using WeatherApplicationServer.Models;
using WeatherApplicationServer.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowBlazorApp")]
public class WeatherForecastController : ControllerBase
{
    private readonly SupabaseService _supabaseService;

    public WeatherForecastController(SupabaseService supabaseService)
    {
        _supabaseService = supabaseService;
        _supabaseService.InitializeAsync().Wait();
    }

    // Get all weather data
    [HttpGet]
    public async Task<ActionResult<List<WeatherModel>>> Get()
    {
        var weatherData = await _supabaseService.GetWeatherDataAsync();
        return Ok(weatherData);
    }

    // Create new weather data entry
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] WeatherModel weatherModel)
    {
        await _supabaseService.CreateAsync(weatherModel);
        return CreatedAtAction(nameof(Get), new { id = weatherModel.Id }, weatherModel);
    }

    // Update weather data by ID
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] WeatherModel weatherModel)
    {
        if (id != weatherModel.Id)
        {
            return BadRequest("ID mismatch");
        }

        await _supabaseService.UpdateAsync(weatherModel);
        return NoContent();
    }

    // Delete weather data by ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _supabaseService.DeleteAsync(id);
        return NoContent();
    }
}
