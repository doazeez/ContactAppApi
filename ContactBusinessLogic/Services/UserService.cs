using ContactBusinessLogic.Interface;
using ContactDatabase.DTO;
using ContactDatabase.Model;
using DatabaseAndModel;
using DatabaseAndModel.DTO;
using DatabaseAndModel.DTO.Mappings;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBusinessLogic.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<User> _userManager;
        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserResponseDTO> Register(RegisterRequestDTO registerationRequest)
        {
            User user = UserMappings.GetUser(registerationRequest);
            IdentityResult result = await _userManager.CreateAsync(user, registerationRequest.Password);
            if (result.Succeeded)
            {
                return UserMappings.GetUserResponse(user);
            }

            string errors = string.Empty;
            foreach (var error in result.Errors)
            {
                errors += error.Description + Environment.NewLine;
            }
            throw new MissingFieldException(errors);
        }

        public List<User> GetListOfUsers()
        {
            return  _userManager.Users.ToList();
        }
        public async Task<bool> Update(string Id, UpdateRequest updateUser)
        {
            User user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                user.FirstName = string.IsNullOrWhiteSpace(updateUser.FirstName) ? user.FirstName : updateUser.FirstName;
                user.LastName = string.IsNullOrWhiteSpace(updateUser.LastName) ? user.LastName : updateUser.LastName;
                user.PhoneNumber = string.IsNullOrWhiteSpace(updateUser.PhoneNumber) ? user.PhoneNumber : updateUser.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description + Environment.NewLine;
                }
                throw new MissingMemberException(errors);
            }
            throw new ArgumentException("Resource Not Found");
        }
        
        public async Task<bool> Delete(string Id)
        {
            User user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description + Environment.NewLine;
                }
                throw new MissingMemberException(errors);
            }
            throw new ArgumentException("Resource Not Found");
        }

        public async Task<UserResponseDTO> GetUserAsync(string Id)
        {
            User user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                return UserMappings.GetUserResponse(user);

            }
            throw new ArgumentException("Resource Not Found");
        }

        public async Task<UserResponseDTO> GetUserByEmailAsync(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return UserMappings.GetUserResponse(user);

            }
            throw new ArgumentException("Resource Not Found");
        }
    }
}
