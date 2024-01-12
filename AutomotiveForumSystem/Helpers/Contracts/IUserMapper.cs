using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Models.ViewModels;

namespace AutomotiveForumSystem.Helpers.Contracts
{
    public interface IUserMapper
    {
        User Map(UserCreateDTO user);

        UserResponseDTO Map(User user);

		User Map(UserRegisterViewModel user);

		List<UserResponseDTO> Map(List<User> users);
    }
}
