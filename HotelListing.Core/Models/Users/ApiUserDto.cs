using System.ComponentModel.DataAnnotations;

namespace HotelListing.Core.Models.Users;

public class ApiUserDto : LoginDto
{
    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }


}