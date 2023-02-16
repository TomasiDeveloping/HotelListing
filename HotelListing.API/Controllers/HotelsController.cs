using HotelListing.Core.Contracts;
using HotelListing.Core.Models;
using HotelListing.Core.Models.Hotel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
//[Authorize]
public class HotelsController : ControllerBase
{
    private readonly IHotelsRepository _hotelsRepository;

    public HotelsController(IHotelsRepository hotelsRepository)
    {
        _hotelsRepository = hotelsRepository;
    }

    // GET: api/Hotels/GetAll
    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
    {
        var hotels = await _hotelsRepository.GetAllAsync<HotelDto>();
        return Ok(hotels);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<HotelDto>>> GetPagedHotels([FromQuery] QueryParameters queryParameters)
    {
        var pagedHotelsResult = await _hotelsRepository.GetAllAsync<HotelDto>(queryParameters);
        return Ok(pagedHotelsResult);
    }

    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        var hotel = await _hotelsRepository.GetAsync<HotelDto>(id);

        if (hotel == null) return NotFound();

        return Ok(hotel);
    }

    // PUT: api/Hotels/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
    {
        if (id != hotelDto.Id) return BadRequest();

        try
        {
            await _hotelsRepository.UpdaterAsync(id, hotelDto);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await HotelExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Hotels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<HotelDto>> PostHotel(CreateHotelDto createHotelDto)
    {
        var hotel = await _hotelsRepository.AddAsync<CreateHotelDto, HotelDto>(createHotelDto);

        return CreatedAtAction("GetHotel", new {id = hotel.Id}, hotel);
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        await _hotelsRepository.DeleteAsync(id);

        return NoContent();
    }

    private async Task<bool> HotelExists(int id)
    {
        return await _hotelsRepository.Exists(id);
    }
}