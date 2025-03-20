using FinanceModule.Domain.Services;
using FinanceModule.Domain.Models;
using FinanceModule.Application.Interfaces;

namespace FinanceModule.Application.Commands
{
    public class CreateCategoryCommand : ICommand<Category>
    {
        private readonly CategoryFacade _facade;
        private readonly string _name;
        private readonly string _type;

        public CreateCategoryCommand(CategoryFacade facade, string name, string type)
        {
            _facade = facade;
            _name = name;
            _type = type;
        }

        public Category Execute()
        {
            return _facade.CreateCategory(_name, _type);
        }
    }
}