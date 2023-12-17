﻿using AutomotiveForumSystem.Data;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;

namespace AutomotiveForumSystem.Helpers
{
    public class PostModelMapper : IPostModelMapper
    {
        private readonly ApplicationContext applicationContext;

        public PostModelMapper(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public Post Map(PostModelCreate model)
        {
            var post = new Post
            {
                Title = model.Title,
                Content = model.Content,
                Category = applicationContext.Categories.FirstOrDefault(c => c.Name == model.CategoryName)
            };
            return post;
        }

        public List<PostResponseDto> MapPostsToResponseDtos(IList<Post> postsToReturn)
        {
            return postsToReturn
                .Select(p => new PostResponseDto()
                {
                    Category = p.Category.Name,
                    Title = p.Title,
                    Content = p.Content,
                    CreateDate = p.CreateDate,
                    Comments = p.Comments,
                    Likes = p.Likes
                })
                .ToList();
        }

        public PostResponseDto MapPostToResponseDto(Post post)
        {
            return new PostResponseDto()
            {
                Category = post.Category.Name,
                Title = post.Title,
                Content = post.Content,
                CreateDate = post.CreateDate,
                Comments = post.Comments,
                Likes = post.Likes
            };
        }
    }
}
