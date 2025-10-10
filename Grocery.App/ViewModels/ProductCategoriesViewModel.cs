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
        public ObservableCollection<ProductCategory> ProductCategories { get; set; } = [];
        public ObservableCollection<Product> AvailableProducts { get; set; } = [];
        public ObservableCollection<Product> UnassignedProducts { get; set; } = [];

        [ObservableProperty]
        Category category;

        private string searchText = "";
        public string SearchText
        {
            get => searchText;
            set
            {
                SetProperty(ref searchText, value);
                PerformSearch(value);
            }
        }

        public ProductCategoriesViewModel(IProductCategoryService productCategoryService, IProductService productService)
        {
            _productCategoryService = productCategoryService;
            _productService = productService;
        }

        private void GetAvailableProducts()
        {
            AvailableProducts.Clear();
            ProductCategories.Clear();

            if (Category is null)
                return;

            // Get all product-category links for this category
            var productCategories = _productCategoryService.GetAllOnCategoryId(Category.Id);

            foreach (var pc in productCategories)
            {
                // Get the product for each link
                var product = _productService.Get(pc.ProductId);
                if (product != null)
                {
                    AvailableProducts.Add(product);
                    ProductCategories.Add(pc);
                }
            }
        }

        private void GetUnassignedProducts()
        {
            UnassignedProducts.Clear();

            // Get all products
            var allProducts = _productService.GetAll();

            // Get all products already assigned to any category
            var assignedProductIds = _productCategoryService.GetAll().Select(pc => pc.ProductId).ToHashSet();

            // Filter products not assigned to any category
            var unassigned = allProducts.Where(p => !assignedProductIds.Contains(p.Id)).ToList();

            foreach (var product in unassigned)
                UnassignedProducts.Add(product);
        }

        partial void OnCategoryChanged(Category? oldValue, Category newValue)
        {
            GetAvailableProducts();
            GetUnassignedProducts();
        }

        [RelayCommand]
        public void AddProductToCategory(Product product)
        {
            if (Category is null || product is null)
                return;

            var productCategory = new ProductCategory(0, Category.Id, product.Id);
            _productCategoryService.Add(productCategory);

            ProductCategories.Add(productCategory);
            UnassignedProducts.Remove(product);
        }

        [RelayCommand]
        public void PerformSearch(string searchText)
        {
            GetUnassignedProducts();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var filtered = UnassignedProducts.Where(p => p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                UnassignedProducts.Clear();
                foreach (var product in filtered)
                    UnassignedProducts.Add(product);
            }
        }
    }
}
