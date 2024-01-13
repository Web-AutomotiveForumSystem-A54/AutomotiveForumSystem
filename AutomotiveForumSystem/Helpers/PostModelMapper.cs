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
                    Likes = p.Likes
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
                Likes = post.Likes
            };
            return postResponseDTO;
        }
		public PostPreViewModel MapPostToPreViewModel(Post post)
		{
            var postPreViewModel = new PostPreViewModel()
            {
                Id = post.Id,
                CategoryName = post.Category.Name,
                Title = post.Title,
                Content = post.Content,
                CreateDate = post.CreateDate.ToString(),
                Likes = post.Likes,
                CreatedBy = post.User.UserName
			};
			return postPreViewModel;
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
					Likes = p.Likes,
                    CreatedBy = p.User.UserName
				})
				.ToList();
		}

		public PostDataViewModel MapPostToDataViewModel(Post post)
		{
			var postDataViewModel = new PostDataViewModel()
			{
                Id = post.Id,
				CategoryName = post.Category.Name,
				Title = post.Title,
				Content = post.Content,
				CreateDate = post.CreateDate.ToString(),
				Likes = post.Likes,
				CreatedBy = post.User.UserName,
                Comments = post.Comments,
			};
			return postDataViewModel;
		}
	}
}
