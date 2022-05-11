using ContactDatabase.DTO;
using ContactDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAndModel.DTO.Mappings
{
    public class UserMappings
    {
        public static UserResponseDTO GetUserResponse(User user)
        {
            return new UserResponseDTO()
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
            };
        }

        public static User GetUser(RegisterRequestDTO request)
        {
            return new User()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = string.IsNullOrWhiteSpace(request.UserName) ? null : request.UserName,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber
            };
        }
    }
}
