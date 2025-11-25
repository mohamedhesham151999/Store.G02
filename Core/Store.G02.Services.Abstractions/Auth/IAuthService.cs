using Store.G02.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Abstractions.Auth
{
    public interface IAuthService
    {
        Task<UserResponse?> LoginAsync(LoginRequest request);
        Task<UserResponse?> RegisterAsync(RegisterRequest request);

        // check Existance of an Email
        Task <bool> CheckEmailExistAsync(string email);
        // Get Current User
        Task <UserResponse?> GetCurrentUserAsync(string email);
        // Get current user Address
        Task <AddressDto?> GetUserAddressAsync(string email);
        // Update current user Address
        Task<UserResponse> UpdateCurrentUserAddressAsync(AddressDto request,string email);
    }
}
