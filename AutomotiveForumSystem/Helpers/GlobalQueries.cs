using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models.ViewModels;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Services.Contracts;
using System.Runtime.CompilerServices;

namespace AutomotiveForumSystem.Helpers
{
	public class GlobalQueries
	{
        public static IList<Category> InitializeCategories(ICategoriesService categoriesService)
        {
			return categoriesService.GetAll();
        }
	}
}
