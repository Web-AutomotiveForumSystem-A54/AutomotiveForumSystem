﻿using AutomotiveForumSystem.Data;
using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Repositories.Contracts;
using AutomotiveForumSystem.Exceptions;

namespace AutomotiveForumSystem.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ApplicationContext applicationContext;

        public CategoriesRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public IList<Category> GetAll(CategoryQueryParameters categoryQueryParameters)
        {
            IQueryable<Category> categories = this.applicationContext.Categories
                .Where(x => x.IsDeleted == false);

            if (!string.IsNullOrEmpty(categoryQueryParameters.Name))
            {
                categories = categories.Where(c => c.Name.Contains(categoryQueryParameters.Name));
            }

            if (!string.IsNullOrEmpty(categoryQueryParameters.SortOrder))
            {
                if (categoryQueryParameters.SortOrder == "asc")
                {
                    categories = categories.OrderBy(x => x.Name);
                }
                else if (categoryQueryParameters.SortOrder == "desc")
                {
                    categories = categories.OrderByDescending(x => x.Name);
                }
            }

            return categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            var categoryToReturn = this.applicationContext.Categories.FirstOrDefault(x => x.Id == id)
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
            var beerToUpdate = GetCategoryById(id);

            beerToUpdate.Name = category.Name;

            this.applicationContext.Update(beerToUpdate);
            this.applicationContext.SaveChanges();

            return beerToUpdate;
        }

        public bool DeleteCategory(int id)
        {
            var categoryToDelete = this.applicationContext.Categories.FirstOrDefault(c => c.Id == id)
                ?? throw new EntityNotFoundException($"Category with id{id} not found.");

            categoryToDelete.IsDeleted = true;

            this.applicationContext.Update(categoryToDelete);
            this.applicationContext.SaveChanges();

            return true;
        }

        public bool DoesCategoryExist(string name)
        {
            return this.applicationContext.Categories.Any(c => c.Name == name);
        }
    }
}