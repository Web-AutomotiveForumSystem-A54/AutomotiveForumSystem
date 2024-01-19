using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models.ViewModels;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Services.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveForumSystem.Helpers
{
	public class GlobalQueries
	{
		public static IList<Category> InitializeCategoriesFromDatabase(ICategoriesService categoriesService)
		{
			return categoriesService.GetAll();
		}

		public static IList<Tag> InitializeTagsFromDatabase(ITagsService tagsService)
		{
			return tagsService.GetAll();
			//return new List<Tag>()
			//{
			//	new Tag()
			//	{
			//		Id = 1,
			//		Name = "bmw",
			//	},
			//	new Tag()
			//	{
			//		Id = 2,
			//		Name = "vw",
			//	},
			//	new Tag()
			//	{
			//		Id = 3,
			//		Name = "engine",
			//	},
			//	new Tag()
			//	{
			//		Id = 4,
			//		Name = "tire",
			//	},
			//	new Tag()
			//	{
			//		Id = 5,
			//		Name = "rim",
			//	},
			//	new Tag()
			//	{
			//		Id = 6,
			//		Name = "visual",
			//	},
			//	new Tag()
			//	{
			//		Id = 6,
			//		Name = "tuning",
			//	},
			//	// tagsService.GetAll();
			//};
		}

		public static void InitializeLayoutBasedData(Controller controller,
			ICategoriesService categoriesService,
			ITagsService tagsService,
			IUsersService usersService,
			IPostService postService,
			ICategoryModelMapper categoryModelMapper)
		{
			var allCategories = InitializeCategoriesFromDatabase(categoriesService);
			var categoryLabels = categoryModelMapper.ExtractCategoriesLabels(allCategories);

			var allTags = InitializeTagsFromDatabase(tagsService);

			controller.ViewData["CategoryLabels"] = categoryLabels;
			controller.ViewData["TagLabels"] = allTags;
			controller.ViewData["TotalPostsCount"] = postService.GetTotalPostCount();
			controller.ViewData["MembersCount"] = usersService.GetAll().Count;
		}
	}
}
