using FinanceModule.Domain.Models;
using System.Collections.Generic;

namespace FinanceModule.Domain.Services
{
    public class CategoryFacade
    {
        private List<Category> _categories = new List<Category>();
        private int _nextId = 1;

        public Category CreateCategory(string name, string type)
        {
            if (string.IsNullOrEmpty(name) || (type != "Income" && type != "Expense"))
                throw new ArgumentException("Неверные данные категории.");

            var category = new Category
            {
                Id = _nextId++,
                Name = name,
                Type = type
            };
            _categories.Add(category);
            return category;
        }

        public IEnumerable<Category> GetCategories() => _categories;

        public void DeleteCategory(int categoryId)
        {
            var category = _categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
                throw new ArgumentException("Категория не найдена.");

            // Удаляем все операции, связанные с этой категорией
            //var operationsToRemove = _operationFacade.GetOperations()
            //    .Where(op => op.CategoryId == categoryId)
            //    .ToList();

            //foreach (var operation in operationsToRemove)
            //{
            //    _operationFacade.DeleteOperation(operation.Id);
            //}

            // Удаляем категорию
            _categories.Remove(category);
        }
    }
}