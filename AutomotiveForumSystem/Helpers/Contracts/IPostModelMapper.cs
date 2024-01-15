using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;
using AutomotiveForumSystem.Models.ViewModels;

namespace AutomotiveForumSystem.Helpers.Contracts
{
    public interface IPostModelMapper
    {
        Post Map(PostCreateDTO model);
        List<PostResponseDto> MapPostsToResponseDtos(IList<Post> postsToReturn);
        PostResponseDto MapPostToResponseDto(Post post);
        List<PostPreViewModel> MapPostsToPreViewModel(IList<Post> postsToReturn);
        PostDataViewModel MapPostToDataViewModel(Post post);
	}
}
