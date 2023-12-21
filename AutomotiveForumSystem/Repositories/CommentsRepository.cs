﻿using AutomotiveForumSystem.Data;
using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AutomotiveForumSystem.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        ApplicationContext applicationContext;

        public CommentsRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public Comment CreateComment(Comment comment)
        {
            this.applicationContext.Comments.Add(comment);
            this.applicationContext.SaveChanges();
            return comment;
        }

        public IList<Comment> GetAllComments(CommentQueryParameters commentQueryParameters)
        {
            // TODO : check how to filter deleted users, comments etc.
            return this.applicationContext.Comments
                .Where(c => c.IsDeleted == false)
                .Include(c => c.Post).Where(c => c.IsDeleted == false )
                .Include(c => c.Replies).Where(r => r.IsDeleted == false)
                .Include(c => c.User)
                .ToList();
        }

        public IList<Comment> GetAllRepliesByCommentId(int id)
        {
            var targetComment = this.applicationContext.Comments.FirstOrDefault(c => c.Id == id)
                ?? throw new EntityNotFoundException($"Comment with id {id} not found.");

            return targetComment.Replies;
        }

        public Comment GetCommentById(int id)
        {
            var targetComment = this.applicationContext.Comments.FirstOrDefault(c => c.Id == id)
                ?? throw new EntityNotFoundException($"Comment with id {id} not found.");

            if (targetComment.IsDeleted == true)
            {
                throw new EntityNotFoundException($"There's no comment with id {id}");
            }

            targetComment.User = this.applicationContext.Users.FirstOrDefault(user => user.Id == targetComment.UserID)
                ?? throw new EntityNotFoundException($"User with id {targetComment.UserID} not found.");

            targetComment.Post = this.applicationContext.Posts.FirstOrDefault(post => post.Id == targetComment.Id)
                ?? throw new EntityNotFoundException($"Post with id {targetComment.PostID} not found.");

            targetComment.Replies = this.applicationContext.Comments
                .Where(p => p.PostID == id).ToList();

            return targetComment;
        }

        public Comment UpdateComment(int id, Comment comment)
        {
            var commentToUpdate = this.applicationContext.Comments.FirstOrDefault(p => p.Id == id)
                ?? throw new EntityNotFoundException($"Comment with id {id} not found.");

            commentToUpdate.Content = comment.Content;
            //this.applicationContext.Update(commentToUpdate);
            this.applicationContext.SaveChanges();

            return commentToUpdate;
        }

        public bool DeleteComment(Comment comment)
        {
            comment.IsDeleted = true;
            //this.applicationContext.Update(comment);
            this.applicationContext.SaveChanges();
            return true;
        }
    }
}
