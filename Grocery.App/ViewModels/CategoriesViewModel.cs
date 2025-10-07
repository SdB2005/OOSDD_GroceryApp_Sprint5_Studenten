using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class CategoriesViewModel : BaseViewModel
    {
        private readonly ICategoryService _categoryService;
        public ObservableCollection<Category> Categories { get; set; }
        public CategoriesViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            Categories = new(_categoryService.GetAll());
        }
        [RelayCommand]
        public async Task SelectCategory(Category category)
        {
            Dictionary<string, object> parameters = new()
            {
                { nameof(Category), category }
            };
            await Shell.Current.GoToAsync($"{nameof(Views.ProductCategoriesView)}", true, parameters);
        }
    }
}
