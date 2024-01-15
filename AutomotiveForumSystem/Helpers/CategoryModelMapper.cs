using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Models.ViewModels;

namespace AutomotiveForumSystem.Helpers
{
    public class CategoryModelMapper : ICategoryModelMapper
    {
        public Category Map(CategoryDTO category)
        {
            var newCategory = new Category()
            {
                Name = category.Name
            };

            return newCategory;
        }

        public CategoryDTO Map(Category category)
        {
            var newCategory = new CategoryDTO()
            {
                Name = category.Name
            };

            return newCategory;
        }

        public IList<CategoryDTO> Map(IList<Category> categories)
        {
            var categoriesToReturn = new List<CategoryDTO>();

            foreach (var category in categories)
            {
                categoriesToReturn.Add(new CategoryDTO()
                {
                    Name = category.Name
                });
            }

            return categoriesToReturn;
        }

		public IList<CategoryLabelViewModel> ExtractCategoriesLabels(IList<Category> categories)
		{
			var categoryLabels = new List<CategoryLabelViewModel>();
			foreach (var category in categories)
			{
				categoryLabels.Add(new CategoryLabelViewModel()
				{
					Id = category.Id,
					Name = category.Name,
					PostsCount = category.Posts.Where(p => !p.IsDeleted).Count()
				});
			}

			return categoryLabels;
		}
	}
}
