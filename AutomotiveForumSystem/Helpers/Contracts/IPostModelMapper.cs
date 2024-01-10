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
        public List<PostDataViewModel> MapPostsToDataViewModel(IList<Post> postsToReturn);
        public PostDataViewModel MapPostToDataViewModel(Post post);

	}
}
