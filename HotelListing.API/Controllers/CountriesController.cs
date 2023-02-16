using HotelListing.Core.Contracts;
using HotelListing.Core.Models;
using HotelListing.Core.Models.Country;
using HotelListing.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class CountriesController : ControllerBase
{
    private readonly ICountriesRepository _countriesRepository;

    public CountriesController(ICountriesRepository countriesRepository)
    {
        _countriesRepository = countriesRepository;
    }

    // GET: api/Countries/GetAll
    [HttpGet("GetAll")]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var countries = await _countriesRepository.GetAllAsync<GetCountryDto>();
        return Ok(countries);
    }

    // GET: api/Countries/?StartIndex=0&pageSize=25&pageNumber=1
    [HttpGet]
    public async Task<ActionResult<PagedResult<GetCountryDto>>> GetPagedCountries(
        [FromQuery] QueryParameters queryParameters)
    {
        var pagedCountryResult = await _countriesRepository.GetAllAsync<GetCountryDto>(queryParameters);
        return Ok(pagedCountryResult);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        var country = await _countriesRepository.GetDetailsAsync(id);
        return Ok(country);
    }

    // PUT: api/Countries/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
    {
        if (id != updateCountryDto.Id) return BadRequest("Invalid Record Id");

        try
        {
            await _countriesRepository.UpdaterAsync(id, updateCountryDto);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CountryExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
    {
        var country = await _countriesRepository.AddAsync<CreateCountryDto, GetCountryDto>(createCountryDto);

        return CreatedAtAction("GetCountry", new {id = country.Id}, country);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        await _countriesRepository.DeleteAsync(id);

        return NoContent();
    }

    private async Task<bool> CountryExists(int id)
    {
        return await _countriesRepository.Exists(id);
    }
}