using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Repositories.Contracts;
using AutomotiveForumSystem.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomotiveForumSystem.Exceptions;

namespace AutomotiveForumSystem.Tests.CommentsServiceTests
{
    [TestClass]
    public class CommentServiceTests
    {
        private Mock<ICommentsRepository> commentsRepositoryMock;
        private Mock<IPostRepository> postRepositoryMock;
        private CommentsService commentsService;

        public CommentServiceTests()
        {
            commentsRepositoryMock = new Mock<ICommentsRepository>();
            postRepositoryMock = new Mock<IPostRepository>();
            commentsService = new CommentsService(commentsRepositoryMock.Object, postRepositoryMock.Object);
        }

        [TestMethod]
        public void GetAllComments_ReturnsCommentsFromRepository()
        {
            // Arrange
            var commentQueryParameters = new CommentQueryParameters();
            var expectedComments = new List<Comment>
        {
            new Comment { Id = 1, Content = "Comment 1" },
            new Comment { Id = 2, Content = "Comment 2" },
            // Add more comments as needed
        };

            // Mock repository behavior
            commentsRepositoryMock.Setup(repo => repo.GetAllComments(commentQueryParameters)).Returns(expectedComments);

            // Act
            var result = commentsService.GetAllComments(commentQueryParameters).ToList();

            // Assert
            // Verify that the repository method was called with the correct parameters
            commentsRepositoryMock.Verify(repo => repo.GetAllComments(commentQueryParameters), Times.Once);

            // Ensure that the returned comments match the expected comments
            CollectionAssert.AreEqual(expectedComments, result);
        }

        [TestMethod]
        public void GetCommentById_ReturnsCommentFromRepository()
        {
            // Arrange
            var commentId = 1;
            var expectedComment = new Comment { Id = commentId, Content = "Sample Comment" };

            // Mock repository behavior
            commentsRepositoryMock.Setup(repo => repo.GetCommentById(commentId)).Returns(expectedComment);

            // Act
            var result = commentsService.GetCommentById(commentId);

            // Assert
            // Verify that the repository method was called with the correct parameters
            commentsRepositoryMock.Verify(repo => repo.GetCommentById(commentId), Times.Once);

            // Ensure that the returned comment matches the expected comment
            Assert.AreEqual(expectedComment, result);
        }

        [TestMethod]
        public void GetAllRepliesByCommentId_ReturnsRepliesFromRepository()
        {
            // Arrange
            var commentId = 1;
            var expectedReplies = new List<Comment>
        {
            new Comment { Id = 2, Content = "Reply 1"},
            new Comment { Id = 3, Content = "Reply 2"},
            // Add more replies as needed
        };

            // Mock repository behavior
            commentsRepositoryMock.Setup(repo => repo.GetAllRepliesByCommentId(commentId)).Returns(expectedReplies);

            // Act
            var result = commentsService.GetAllRepliesByCommentId(commentId).ToList();

            // Assert
            // Verify that the repository method was called with the correct parameters
            commentsRepositoryMock.Verify(repo => repo.GetAllRepliesByCommentId(commentId), Times.Once);

            // Ensure that the returned replies match the expected replies
            CollectionAssert.AreEqual(expectedReplies, result);
        }

        [TestMethod]
        public void CreateComment_CreatesCommentInRepository()
        {
            // Arrange
            var user = new User { Id = 1, Username = "SampleUser" };
            var post = new Post { Id = 1, Title = "SamplePost" };
            var comment = new Comment { Content = "New Comment" };
            var expectedComment = new Comment
            {
                Id = 1,
                Content = "New Comment",
                UserID = user.Id,
                User = user,
                PostID = post.Id,
                Post = post,
                CreateDate = It.IsAny<DateTime>()
            };

            // Mock repository behavior
            commentsRepositoryMock.Setup(repo => repo.CreateComment(It.IsAny<Comment>())).Returns(expectedComment);

            // Act
            var result = commentsService.CreateComment(user, post, comment, null);

            // Assert
            // Verify that the repository method was called with the correct parameters
            commentsRepositoryMock.Verify(repo => repo.CreateComment(It.IsAny<Comment>()), Times.Once);

            // Ensure that the returned comment matches the expected comment
            Assert.AreEqual(expectedComment, result);
        }

        [TestMethod]
        public void UpdateComment_WhenUserIsCommentOwner_UpdatesComment()
        {
            //// Arrange
            //var user = new User { Id = 1, UserName = "SampleUser" };
            //var commentId = 1;
            //var comment = new Comment { Id = commentId, Content = "Updated Content", UserID = user.Id };
            //var expectedComment = new Comment { Id = commentId, Content = "Updated Content", UserID = user.Id };

            //// Mock repository behavior
            //commentsRepositoryMock.Setup(repo => repo.UpdateComment(commentId, comment)).Returns(expectedComment);

            //// Act
            //var result = commentsService.UpdateComment(user, commentId, comment);

            //// Assert
            //// Verify that the repository method was called with the correct parameters
            //commentsRepositoryMock.Verify(repo => repo.UpdateComment(commentId, comment), Times.Once);

            //// Ensure that the returned comment matches the expected comment
            //Assert.AreEqual(expectedComment, result);
        }

        [TestMethod]
        public void UpdateComment_WhenUserIsNotCommentOwner_ThrowsAuthorizationException()
        {
            // Arrange
            //var user = new User { Id = 1, UserName = "SampleUser" };
            //var commentId = 1;
            //var comment = new Comment { Id = commentId, Content = "Updated Content", UserID = 2 }; // Different user

            //// Act
            //// Assert
            //Assert.ThrowsException<AuthorizationException>(() => commentsService.UpdateComment(user, commentId, comment));

            //// Verify that the repository method was not called
            //commentsRepositoryMock.Verify(repo => repo.UpdateComment(It.IsAny<int>(), It.IsAny<Comment>()), Times.Never);
        }

        [TestMethod]
        public void DeleteComment_WhenUserIsCommentOwner_DeletesComment()
        {
            // Arrange
            var user = new User { Id = 1, Username = "SampleUser" };
            var commentId = 1;
            var commentToDelete = new Comment { Id = commentId, Content = "Sample Comment", UserID = user.Id };

            // Mock repository behavior
            commentsRepositoryMock.Setup(repo => repo.GetCommentById(commentId)).Returns(commentToDelete);

            // Act
            var result = commentsService.DeleteComment(user, commentId);

            // Assert
            // Verify that the repository method was called with the correct parameters
            commentsRepositoryMock.Verify(repo => repo.GetCommentById(commentId), Times.Once);
            commentsRepositoryMock.Verify(repo => repo.DeleteComment(commentToDelete, true), Times.Once);

            // Ensure that the method returns true
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteComment_WhenUserIsAdmin_DeletesComment()
        {
            // Arrange
            var user = new User { Id = 1, Username = "AdminUser", IsAdmin = true };
            var commentId = 1;
            var commentToDelete = new Comment { Id = commentId, Content = "Sample Comment", UserID = 2 }; // Different user

            // Mock repository behavior
            commentsRepositoryMock.Setup(repo => repo.GetCommentById(commentId)).Returns(commentToDelete);

            // Act
            var result = commentsService.DeleteComment(user, commentId);

            // Assert
            // Verify that the repository method was called with the correct parameters
            commentsRepositoryMock.Verify(repo => repo.GetCommentById(commentId), Times.Once);
            commentsRepositoryMock.Verify(repo => repo.DeleteComment(commentToDelete, true), Times.Once);

            // Ensure that the method returns true
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteComment_WhenUserIsNotCommentOwnerAndNotAdmin_ThrowsAuthorizationException()
        {
            // Arrange
            var user = new User { Id = 1, Username = "SampleUser" };
            var commentId = 1;
            var commentToDelete = new Comment { Id = commentId, Content = "Sample Comment", UserID = 2 }; // Different user

            // Mock repository behavior
            commentsRepositoryMock.Setup(repo => repo.GetCommentById(commentId)).Returns(commentToDelete);

            // Act
            // Assert
            Assert.ThrowsException<AuthorizationException>(() => commentsService.DeleteComment(user, commentId));

            // Verify that the repository method was called with the correct parameters
            commentsRepositoryMock.Verify(repo => repo.GetCommentById(commentId), Times.Once);

            // Verify that the repository method was not called
            commentsRepositoryMock.Verify(repo => repo.DeleteComment(It.IsAny<Comment>(), true), Times.Never);
        }
    }
}
