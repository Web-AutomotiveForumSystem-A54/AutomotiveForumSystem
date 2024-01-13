using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Models.ViewModels;

namespace AutomotiveForumSystem.Helpers.Contracts
{
    public interface ICategoryModelMapper
    {
        Category Map(CategoryDTO category);
        CategoryDTO Map(Category category);
        IList<CategoryDTO> Map(IList<Category> category);
        IList<CategoryLabelViewModel> ExtractCategoriesLabels(IList<Category> category);
    }
}
