﻿using AutomotiveForumSystem.Data;
using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Repositories.Contracts;
using AutomotiveForumSystem.Services.Contracts;
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
            var comments = this.applicationContext.Comments
                .Where(c => c.IsDeleted == false)
                .Include(c => c.Post).Where(c => c.IsDeleted == false)
                .Include(c => c.Replies).Where(r => r.IsDeleted == false)
                .Include(c => c.User).AsQueryable();

            if (!string.IsNullOrEmpty(commentQueryParameters.Content))
            {
                comments = comments.Where(c => c.Content.Contains(commentQueryParameters.Content));
            }

            // NOTE : we probably shouldn't check for deleted user as his comments will be already deleteed
            // once the user got deleted
            if (!string.IsNullOrEmpty(commentQueryParameters.User))
            {
                comments = comments.Where(c => c.User.UserName == commentQueryParameters.User &&
                !c.User.IsDeleted);
            }

            IOrderedQueryable<Comment> orderedComments = (IOrderedQueryable<Comment>)comments;

            // NOTE : do we even have to sort by data as the entries are always created sequentially
            if (!string.IsNullOrEmpty(commentQueryParameters.SortBy))
            {
                if (commentQueryParameters.SortBy == "user")
                {
                    orderedComments = orderedComments.OrderBy(c => c.User.UserName);
                }
                else if (commentQueryParameters.SortBy == "date")
                {
                    orderedComments = orderedComments.ThenBy(c => c.CreateDate);
                }
            }

            if (!string.IsNullOrEmpty(commentQueryParameters.SortOrder))
            {
                if (commentQueryParameters.SortOrder == "desc")
                {
                    orderedComments = orderedComments.OrderByDescending(c => c);
                }
            }

            return orderedComments.ToList();
        }

        // NOTE : do we even have to have this ?
        // and do we have to have sorting
        public IList<Comment> GetAllRepliesByCommentId(int id)
        {
            var targetComment = this.applicationContext.Comments.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
                ?? throw new EntityNotFoundException($"Comment with id {id} not found.");

            return targetComment.Replies.Where(r =>!r.IsDeleted).ToList();
        }
        
        // NOTE : do we have to consider checking IsDeleted for user here ?
        public Comment GetCommentById(int id)
        {
            var targetComment = this.applicationContext.Comments
                .Include(c => c.User)
                .Include(c => c.Post)
                .Include(c => c.Replies)
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted)
                ?? throw new EntityNotFoundException($"Comment with id {id} not found.");

            return targetComment;
        }

        public Comment UpdateComment(int id, Comment comment)
        {
            var commentToUpdate = this.applicationContext.Comments.FirstOrDefault(p => p.Id == id && !p.IsDeleted)
                ?? throw new EntityNotFoundException($"Comment with id {id} not found.");

            commentToUpdate.Content = comment.Content;
            this.applicationContext.SaveChanges();

            return commentToUpdate;
        }

        public bool DeleteComment(Comment comment, bool b_SaveChanges = true)
        {
            comment.IsDeleted = true;

            foreach (var item in comment.Replies)
            {
                item.IsDeleted = true;
            }

            if (b_SaveChanges)
            {
                this.applicationContext.SaveChanges();
            }

            return true;
        }

        // TODO : ask trainers if we can call this method directly from Post Repository
        public bool DeleteComments(List<Comment> comments)
        {
            foreach (var comment in comments)
            {
                this.DeleteComment(comment, false);
            }
            return true;
        }
    }
}
