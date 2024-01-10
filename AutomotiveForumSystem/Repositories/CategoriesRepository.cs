using AutomotiveForumSystem.Data;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Repositories.Contracts;
using AutomotiveForumSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveForumSystem.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ApplicationContext applicationContext;
        private readonly IPostRepository postRepository;

        public CategoriesRepository(ApplicationContext applicationContext, IPostRepository postRepository)
        {
            this.applicationContext = applicationContext;
            this.postRepository = postRepository;
        }

        public IList<Category> GetAll()
        {
            // NOTE : do not include posts for final project
            // create another method to return posts count
            var categories = this.applicationContext.Categories.Where(c => c.IsDeleted == false)
                .Include(c => c.Posts);

            return categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            var categoryToReturn = this.applicationContext.Categories.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
                ?? throw new EntityNotFoundException($"Category with id {id} not found.");

            return categoryToReturn;
        }

        public Category CreateCategory(string categoryName)
        {
            var newCategory = new Category()
            {
                Name = categoryName,
            };

            this.applicationContext.Categories.Add(newCategory);
            this.applicationContext.SaveChanges();

            return newCategory;
        }

        public Category UpdateCategory(int id, Category category)
        {
            var categoryToUpdate = GetCategoryById(id);

            categoryToUpdate.Name = category.Name;
            categoryToUpdate.IsDeleted = category.IsDeleted;

            this.applicationContext.Update(categoryToUpdate);
            this.applicationContext.SaveChanges();

            return categoryToUpdate;
        }

        public bool DeleteCategory(int id)
        {
            var categoryToDelete = this.applicationContext.Categories.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
                ?? throw new EntityNotFoundException($"Category with id {id} not found.");

            categoryToDelete.IsDeleted = true;

            this.applicationContext.Update(categoryToDelete);
            this.applicationContext.SaveChanges();

            // NOTE : check if posts have to be initialized

            foreach (var post in categoryToDelete.Posts)
            {
                postRepository.DeletePost(post);
            }

            return true;
        }

        public bool DoesCategoryExist(string name)
        {
            return this.applicationContext.Categories.Any(c => c.Name == name && !c.IsDeleted);
        }
    }
}
