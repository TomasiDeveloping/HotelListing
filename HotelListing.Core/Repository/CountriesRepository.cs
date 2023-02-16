using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.Core.Contracts;
using HotelListing.Core.Exceptions;
using HotelListing.Core.Models.Country;
using HotelListing.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Core.Repository;

public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;

    public CountriesRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CountryDto> GetDetailsAsync(int id)
    {
        var country = await _context.Countries
            .Include(c => c.Hotels)
            .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (country is null) throw new NotFoundException(nameof(GetDetailsAsync), id);

        return country;
    }
}