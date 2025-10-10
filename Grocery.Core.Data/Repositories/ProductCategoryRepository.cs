using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;

namespace Grocery.Core.Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly List<ProductCategory> productCategories;
        public ProductCategoryRepository()
        {
            productCategories = [
                new ProductCategory(1, 3, 1),
                new ProductCategory(2, 3, 2),
            ];
        }
        public ProductCategory Add(ProductCategory item)
        {
            productCategories.Add(item);
            return item;
        }
        public List<ProductCategory> GetAll()
        {
            return productCategories;
        }
        public List<ProductCategory> GetAllOnCategoryId(int categoryId)
        {
            return productCategories.Where(pc => pc.CategoryId == categoryId).ToList();
        }
    }
}
