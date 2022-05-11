using ContactDatabase.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBusinessLogic.Interface
{
    public interface IAuthentication
    {
        Task<UserResponseDTO> Login(LoginRequestDTO userRequest);
    }
}
