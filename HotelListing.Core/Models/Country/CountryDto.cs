using HotelListing.Core.Models.Hotel;

namespace HotelListing.Core.Models.Country;

public class CountryDto : BaseCountryDto
{
    public int Id { get; set; }
    public List<HotelDto> Hotels { get; set; }
}