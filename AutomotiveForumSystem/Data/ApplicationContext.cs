using AutomotiveForumSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveForumSystem.Data
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Like> Likes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Seed users

			List<User> users = new List<User>()
			{
				new User()
				{
					Id = 1,
					Username = "jonkata",
					FirstName = "John",
					LastName = "Smith",
					Email = "john@mail.com",
					Password = "1234",
					PhoneNumber = "0888 102 030",
					IsAdmin = true,
				},
				new User()
				{
					Id = 2,
					Username = "stevie",
					FirstName = "Steven",
					LastName = "Solberg",
					Email = "steven@mail.com",
					Password = "1020",
					PhoneNumber = null,
					IsAdmin = false,
				},
				new User()
				{
					Id = 3,
					Username = "vanko_54",
					FirstName = "Ivan",
					LastName = "Ivanov",
					Email = "ivan@mail.com",
					Password = "3344",
					PhoneNumber = null,
					IsAdmin = false,
				}
			};

			modelBuilder.Entity<User>().HasData(users);

			// Seed categories

			List<Category> categories = new List<Category>()
			{
				new Category { Id = 1, Name = "Tuning" },
				new Category { Id = 2, Name = "Engines" },
				new Category { Id = 3, Name = "Suspension" },
				new Category { Id = 4, Name = "Electronics" }
			};

			modelBuilder.Entity<Category>().HasData(categories);

			// Seed posts

			List<Post> posts = new List<Post>()
			{
				new Post
				{
					Id = 1,
					CategoryID = 1,
					UserID = 1,
					Title = "This is a post about the tuning of my supra.",
					Content = "Step by step tutorial.",
					CreateDate = DateTime.Now,
				},

				new Post
				{
					Id = 2,
					CategoryID = 2,
					UserID = 1,
					Title = "here i will talk about the supra mk4's 2jz engine",
					Content = "Step by step tutorial.",
					CreateDate = DateTime.Now,
				},

				new Post
				{
					Id = 3,
					CategoryID = 2,
					UserID = 1,
					Title = "the engine is incredibly small like all japanese engines",
					Content = "Step by step tutorial.",
					CreateDate = DateTime.Now,
				},

				new Post
				{
					Id = 4,
					CategoryID = 3,
					UserID = 1,
					Title = "the suspension on this car is not the best but it does the job",
					Content = "Step by step tutorial.",
					CreateDate = DateTime.Now,
				},


				new Post
				{
					Id = 5,
					CategoryID = 4,
					UserID = 1,
					Title = "this is a very old car so there's very few electronics on it",
					Content = "Step by step tutorial.",
					CreateDate = DateTime.Now,
				}
			};

			modelBuilder.Entity<Post>().HasData(posts);

			// Seed comments

			List<Comment> comments = new List<Comment>()
			{
				new Comment()
				{
					Id = 1,
					UserID = 1,
					PostID = 1,
					CreateDate = DateTime.Now,
					Content = "Awesome. I will follow your tutorial to tune my supra."
				},

				new Comment()
				{
					Id = 2,
					UserID = 2,
					PostID = 2,
					CreateDate = DateTime.Now,
					Content = "Comment number 2 with ensured min length."
				},

				new Comment()
				{
					Id = 3,
					UserID = 3,
					PostID = 3,
					CreateDate = DateTime.Now,
					Content = "Comment number 3 with ensured min length."
				},
				new Comment()
				{
					Id = 4,
					UserID = 1,
					PostID = 4,
					CreateDate = DateTime.Now,
					Content = "Comment number 4 with ensured min length."
				},
				new Comment()
				{
					Id = 5,
					UserID = 1,
					PostID = 4,
					CreateDate = DateTime.Now,
					Content = "Comment number 5 with ensured min length."
				},
				new Comment()
				{
					Id = 6,
					UserID = 1,
					PostID = 4,
					CreateDate = DateTime.Now,
					Content = "Comment number 6 with ensured min length.",
				},

				new Comment()
				{
					Id=7,
					UserID = 2,
					PostID = 4,
					ParentCommentId = 6,
					CreateDate = DateTime.Now,
					Content="This is a reply."
				}
			};

			modelBuilder.Entity<Comment>()
				.HasOne(r => r.User)
				.WithMany(u => u.Comments)
				.HasForeignKey(r => r.UserID)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Comment>()
				.HasOne(r => r.Post)
				.WithMany(b => b.Comments)
				.HasForeignKey(r => r.PostID)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Comment>().HasData(comments);

			modelBuilder.Entity<Like>()
				.HasOne(l => l.User)
				.WithMany(u => u.Likes)
				.HasForeignKey(l => l.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Like>()
				.HasOne(l => l.Post)
				.WithMany(p => p.Likes)
				.HasForeignKey(r => r.PostId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
