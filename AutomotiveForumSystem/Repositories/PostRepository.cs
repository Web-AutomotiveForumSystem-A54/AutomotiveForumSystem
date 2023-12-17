﻿using AutomotiveForumSystem.Data;
using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;
using AutomotiveForumSystem.Repositories.Contracts;

namespace AutomotiveForumSystem.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationContext applicationContext;

        public PostRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public Post Create(Post post, User currentUser)
        {
            currentUser.Posts.Add(post);
            this.applicationContext.Posts.Add(post);
            return post;
        }

        public void Delete(Post post, User currentUser)
        {
            currentUser.Posts.Remove(post);
            this.applicationContext.Posts.Remove(post);
        }

        public IList<Post> GetAllPosts()
        {
            IQueryable<Post> postsToReturn = applicationContext.Posts;
            return postsToReturn.ToList();
        }

        public IList<Post> GetPostByCategory(string categoryName)
        {
            var postsToReturn = this.applicationContext.Posts.AsQueryable()
                .Where(p => p.Category.Name == categoryName);

            return postsToReturn.ToList();
        }

        public Post GetPostById(int id)
        {
            return applicationContext.Posts.FirstOrDefault(p => p.Id == id)
                ?? throw new EntityNotFoundException($"Post with ID: {id} not found");
        }

        public IList<Post> GetPostsByUser(int userId, PostQueryParameters postQueryParameters)
        {
            var user = this.applicationContext.Users.FirstOrDefault(u => u.Id == userId)
                ?? throw new EntityNotFoundException($"User with ID: {userId} doesn't exist");

            var postsToReturn = user.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(postQueryParameters.Category))
            {
                postsToReturn = postsToReturn.Where(p => p.Category.Name == postQueryParameters.Category);
            }
            if (!string.IsNullOrEmpty(postQueryParameters.Title))
            {
                postsToReturn = postsToReturn.Where(p => p.Title == postQueryParameters.Title);
            }
            return postsToReturn.ToList();
        }

        public bool PostExist(string title)
        {
            return applicationContext.Posts.Any(p => p.Title == title);
        }

        public Post Update(int id, Post post)
        {
            var postToUpdate = applicationContext.Posts.FirstOrDefault(p => p.Id == id)
                ?? throw new EntityNotFoundException($"Post with ID: {id} not found");

            postToUpdate.Title = post.Title;
            postToUpdate.Content = post.Content;
            postToUpdate.Category = post.Category;
            applicationContext.SaveChanges();

            return postToUpdate;
        }
    }
}