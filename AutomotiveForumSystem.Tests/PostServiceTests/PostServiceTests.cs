using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;
using AutomotiveForumSystem.Repositories.Contracts;
using AutomotiveForumSystem.Services;
using Moq;

namespace AutomotiveForumSystem.Tests.PostServiceTests
{
    [TestClass]
    public class PostServiceTests
    {
        private Mock<IPostRepository> postRepositoryMock;
        private PostService postService;

        public PostServiceTests()
        {
            postRepositoryMock = new Mock<IPostRepository>();
            postService = new PostService(postRepositoryMock.Object);
        }
        [TestMethod]
        public void CreatePost_WhenUserNotBlocked_CallsRepositoryCreatePostAndReturnsPost()
        {
            var currentUser = new User { Id = 1, Username = "testUser", IsBlocked = false };
            var post = new Post { /* Initialize your post properties here */ };

            // Act
            var result = postService.CreatePost(post, currentUser);

            // Assert
            // Verify that the repository method was called with the correct parameters
            postRepositoryMock.Verify(repo => repo.CreatePost(post, currentUser), Times.Once);

            // Ensure that the returned post is the same as the input post
            Assert.AreEqual(post, result);
        }

        [TestMethod]
        public void CreatePost_WhenUserBlocked_ThrowsUserBlockedException()
        {

            var currentUser = new User { Id = 1, Username = "blockedUser", IsBlocked = true };
            var post = new Post { /* Initialize your post properties here */ };

            // Act
            // Assert
            Assert.ThrowsException<UserBlockedException>(() => postService.CreatePost(post, currentUser));

            // Verify that the repository method is not called when the user is blocked
            postRepositoryMock.Verify(repo => repo.CreatePost(It.IsAny<Post>(), It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void DeletePost_WhenUserIsAdmin_DeletesPost()
        {
            // Arrange
            var postId = 1;
            var currentUser = new User { Id = 1, Username = "adminUser", IsAdmin = true };
            var postToDelete = new Post { Id = postId, UserID = currentUser.Id };

            // Mock repository behavior
            postRepositoryMock.Setup(repo => repo.GetPostById(postId)).Returns(postToDelete);

            // Act
            postService.DeletePost(postId, currentUser);

            // Assert
            // Verify that the repository method was called with the correct parameters
            postRepositoryMock.Verify(repo => repo.DeletePost(postToDelete), Times.Once);
        }

        [TestMethod]
        public void DeletePost_WhenUserIsPostCreator_DeletesPost()
        {
            // Arrange
            var postId = 1;
            var currentUser = new User { Id = 1, Username = "postCreator", IsAdmin = false };
            var postToDelete = new Post { Id = postId, UserID = currentUser.Id };

            // Mock repository behavior
            postRepositoryMock.Setup(repo => repo.GetPostById(postId)).Returns(postToDelete);

            // Act
            postService.DeletePost(postId, currentUser);

            // Assert
            // Verify that the repository method was called with the correct parameters
            postRepositoryMock.Verify(repo => repo.DeletePost(postToDelete), Times.Once);
        }

        [TestMethod]
        public void DeletePost_WhenUserIsNotAdminOrPostCreator_ThrowsAuthorizationException()
        {
            // Arrange
            var postId = 1;
            var currentUser = new User { Id = 2, Username = "regularUser", IsAdmin = false };
            var postToDelete = new Post { Id = postId, UserID = 1 }; // Different user created the post

            // Mock repository behavior
            postRepositoryMock.Setup(repo => repo.GetPostById(postId)).Returns(postToDelete);

            // Act
            // Assert
            Assert.ThrowsException<AuthorizationException>(() => postService.DeletePost(postId, currentUser));

            // Verify that the repository method is not called when authorization fails
            postRepositoryMock.Verify(repo => repo.DeletePost(It.IsAny<Post>()), Times.Never);
        }

        [TestMethod]
        public void GetAll_ReturnsPostsFromRepository()
        {
            // Arrange
            var postQueryParameters = new PostQueryParameters(); // You might need to set up query parameters as needed
            var expectedPosts = new List<Post>
        {
            new Post { Id = 1, Title = "Post 1" },
            new Post { Id = 2, Title = "Post 2" },
            // Add more posts as needed
        };

            // Mock repository behavior
            postRepositoryMock.Setup(repo => repo.GetAll(postQueryParameters)).Returns(expectedPosts);

            // Act
            var result = postService.GetAll(postQueryParameters).ToList();

            // Assert
            // Verify that the repository method was called with the correct parameters
            postRepositoryMock.Verify(repo => repo.GetAll(postQueryParameters), Times.Once);

            // Ensure that the returned posts match the expected posts
            CollectionAssert.AreEqual(expectedPosts, result);
        }

        [TestMethod]
        public void GetPostById_ReturnsPostFromRepository()
        {
            // Arrange
            var postId = 1;
            var expectedPost = new Post { Id = postId, Title = "Sample Post" };

            // Mock repository behavior
            postRepositoryMock.Setup(repo => repo.GetPostById(postId)).Returns(expectedPost);

            // Act
            var result = postService.GetPostById(postId);

            // Assert
            // Verify that the repository method was called with the correct parameters
            postRepositoryMock.Verify(repo => repo.GetPostById(postId), Times.Once);

            // Ensure that the returned post matches the expected post
            Assert.AreEqual(expectedPost, result);
        }

        [TestMethod]
        public void GetPostsByUser_ReturnsPostsFromRepository()
        {
            // Arrange
            var userId = 1;
            var postQueryParameters = new PostQueryParameters { Category = "SampleCategory", Title = "SampleTitle" };

            var expectedPosts = new List<Post>
        {
            new Post { Id = 1, Title = "Post 1", UserID = userId, Category = new Category { Name = "SampleCategory" } },
            new Post { Id = 2, Title = "Post 2", UserID = userId, Category = new Category { Name = "SampleCategory" } },
            // Add more posts as needed
        };

            // Mock repository behavior
            postRepositoryMock.Setup(repo => repo.GetPostsByUser(userId, postQueryParameters)).Returns(expectedPosts);

            // Act
            var result = postService.GetPostsByUser(userId, postQueryParameters).ToList();

            // Assert
            // Verify that the repository method was called with the correct parameters
            postRepositoryMock.Verify(repo => repo.GetPostsByUser(userId, postQueryParameters), Times.Once);

            // Ensure that the returned posts match the expected posts
            CollectionAssert.AreEqual(expectedPosts, result);
        }

        [TestMethod]
        public void UpdatePost_WhenUserIsPostCreator_UpdatesPost()
        {
            // Arrange
            var postId = 1;
            var currentUser = new User { Id = 1, Username = "postCreator" };
            var postToUpdate = new Post { Id = postId, UserID = currentUser.Id };
            var updatedPost = new Post { Id = postId, Title = "Updated Title", UserID = currentUser.Id };

            // Mock repository behavior
            postRepositoryMock.Setup(repo => repo.GetPostById(postId)).Returns(postToUpdate);
            postRepositoryMock.Setup(repo => repo.UpdatePost(postId, It.IsAny<Post>())).Returns(updatedPost);

            // Act
            var result = postService.Update(postId, new Post { Title = "Updated Title" }, currentUser);

            // Assert
            // Verify that the repository methods were called with the correct parameters
            postRepositoryMock.Verify(repo => repo.GetPostById(postId), Times.Once);
            postRepositoryMock.Verify(repo => repo.UpdatePost(postId, It.IsAny<Post>()), Times.Once);

            // Ensure that the returned post matches the expected result
            Assert.IsNotNull(result);
            Assert.AreEqual(postId, result.Id);
            Assert.AreEqual(currentUser.Id, result.UserID);
            Assert.AreEqual("Updated Title", result.Title);
        }


        [TestMethod]
        public void UpdatePost_WhenUserIsNotPostCreator_ThrowsAuthorizationException()
        {
            // Arrange
            var postId = 1;
            var currentUser = new User { Id = 2, Username = "otherUser" };
            var postToUpdate = new Post { Id = postId, UserID = 1 }; // Different user created the post

            // Mock repository behavior
            postRepositoryMock.Setup(repo => repo.GetPostById(postId)).Returns(postToUpdate);

            // Act
            // Assert
            Assert.ThrowsException<AuthorizationException>(() => postService.Update(postId, new Post(), currentUser));

            // Verify that the repository methods were called with the correct parameters
            postRepositoryMock.Verify(repo => repo.GetPostById(postId), Times.Once);
            postRepositoryMock.Verify(repo => repo.UpdatePost(It.IsAny<int>(), It.IsAny<Post>()), Times.Never);
        }
    }
}