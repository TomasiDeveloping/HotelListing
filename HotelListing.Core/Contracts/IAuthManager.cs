using HotelListing.Core.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Core.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> RegisterAsync(ApiUserDto userDto);

        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<string> CreateRefreshTokenAsync();
        Task<AuthResponseDto> VerifyRefreshTokenAsync(AuthResponseDto responseDto);
    }
}
