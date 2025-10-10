using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    [QueryProperty(nameof(Category), nameof(Category))]
    public partial class ProductCategoriesViewModel : BaseViewModel
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductService _productService;
        private string searchtText = "";
        public ObservableCollection<ProductCategory> ProductCategories { get; set; } = [];
        public ObservableCollection<Product> AvailableProducts { get; set; } = [];

        [ObservableProperty]
        Category category;

        public ProductCategoriesViewModel(IProductCategoryService productCategoryService, IProductService productService)
        {
            _productCategoryService = productCategoryService;
            _productService = productService;
        }

        partial void OnCategoryChanged(Category? oldValue, Category newValue)
        {
            // Laad de gekoppelde producten als de categorie verandert
            GetAvailableProducts();
        }

        private void GetAvailableProducts()
        {
            AvailableProducts.Clear();
            ProductCategories.Clear();

            if (Category is null)
                return;

            // Haal alle koppelingen voor deze categorie op
            var productCategories = _productCategoryService.GetAllOnCategoryId(Category.Id);

            foreach (var pc in productCategories)
            {
                // Haal het product op
                var product = _productService.Get(pc.ProductId);
                if (product != null)
                {
                    AvailableProducts.Add(product);
                    ProductCategories.Add(pc);
                }
            }
        }
        [RelayCommand]
        public void AddProduct(Product product)
        {
            if (Category is null || product is null)
                return;

            // Maak een nieuwe ProductCategory aan
            var productCategory = new ProductCategory(0, Category.Id, product.Id);

            // Voeg toe via de service
            _productCategoryService.Add(productCategory);

            // Voeg toe aan de ObservableCollection voor de UI
            ProductCategories.Add(productCategory);
        }
        [RelayCommand]
        public void PerformSearch(string searchText)
        {

        }
    }
}
