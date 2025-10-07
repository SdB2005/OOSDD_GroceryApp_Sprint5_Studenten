using Grocery.Core.Interfaces.Services;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Grocery.Core.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _repo;

        public ProductCategoryService(IProductCategoryRepository repo)
        {
            _repo = repo;
        }

        public ProductCategory Add(ProductCategory item) => _repo.Add(item);

        public List<ProductCategory> GetAll() => _repo.GetAll();

        public List<ProductCategory> GetAllOnCategoryId(int id)
        {
            return _repo.GetAll().Where(pc => pc.CategoryId == id).ToList();
        }
    }
}
