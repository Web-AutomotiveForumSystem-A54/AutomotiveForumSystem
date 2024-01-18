using AutomotiveForumSystem.Data;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;
using AutomotiveForumSystem.Models.ViewModels;

namespace AutomotiveForumSystem.Helpers
{
    public class PostModelMapper : IPostModelMapper
    {
        public Post Map(PostCreateDTO model)
        {
            var post = new Post
            {
                Title = model.Title,
                Content = model.Content,
                CategoryID = model.CategoryID,
            };
            return post;
        }

        public List<PostResponseDto> MapPostsToResponseDtos(IList<Post> postsToReturn)
        {
            return postsToReturn
                .Select(p => new PostResponseDto()
                {
                    CategoryName = p.Category.Name,
                    Title = p.Title,
                    Content = p.Content,
                    CreateDate = p.CreateDate.ToString(),
                    Comments = p.Comments,
                })
                .ToList();
        }

        public PostResponseDto MapPostToResponseDto(Post post)
        {
            var postResponseDTO = new PostResponseDto()
            {
                CategoryName = post.Category.Name,
                Title = post.Title,
                Content = post.Content,
                CreateDate = post.CreateDate.ToString(),
                Comments = post.Comments,
            };
            return postResponseDTO;
        }

		public List<PostPreViewModel> MapPostsToPreViewModel(IList<Post> postsToReturn)
		{
			return postsToReturn
				.Select(p => new PostPreViewModel()
				{
                    Id = p.Id,
					CategoryName = p.Category.Name,
					Title = p.Title,
					Content = p.Content,
					CreateDate = p.CreateDate.ToString(),
                    CreatedBy = p.User.UserName,
                    Likes = p.TotalLikesCount,
                    Tags = p.Tags
				})
				.ToList();
		}

		public PostDataViewModel MapPostToDataViewModel(Post post)
		{
			var postDataViewModel = new PostDataViewModel()
			{
                Id = post.Id,
				CategoryId = post.Category.Id,
				Title = post.Title,
				Content = post.Content,
				CreateDate = post.CreateDate.ToString(),
				CreatedBy = post.User.UserName,
                Comments = post.Comments.ToList(),
                Likes = post.Likes.ToList()
			};
			return postDataViewModel;
		}
	}
}
