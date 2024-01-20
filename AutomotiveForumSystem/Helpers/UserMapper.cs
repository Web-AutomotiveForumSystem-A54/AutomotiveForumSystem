using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models.ViewModels;

namespace AutomotiveForumSystem.Helpers
{
    public class UserMapper : IUserMapper
    {
        public User Map(UserCreateDTO user)
        {
            return new User()
            {
                Username = user.Username,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public UserResponseDTO Map(User user)
        {
            return new UserResponseDTO()
            {
                UserName = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public List<UserResponseDTO> Map(List<User> users)
        {
            var responseList = new List<UserResponseDTO>();

            foreach (var user in users)
            {
                responseList.Add(this.Map(user));
            }

            //var list = users.Select(u => this.Map(u)).ToList();

            return responseList;
        }

		public User Map(UserRegisterViewModel user)
		{
			return new User()
			{
				Username = user.Username,
				Password = user.Password,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber
			};
		}

        public User Map(UserUpdateProfileInformationDTO user)
        {
            return new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public User Map(UserUpdateProfileInformationViewModel user)
        {
            return new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}