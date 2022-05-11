using ContactDatabase.DTO;
using ContactDatabase.Model;
using DatabaseAndModel.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBusinessLogic.Interface
{
    public interface IUserService
    {
        Task<UserResponseDTO> Register(RegisterRequestDTO registerationRequest);
        Task<bool> Delete(string Id);
        Task<UserResponseDTO> GetUserAsync(string Id);
        Task<UserResponseDTO> GetUserByEmailAsync(string email);
        Task<bool> Update(string Id, UpdateRequest updateUser);
        List<User> GetListOfUsers();
    }
}
