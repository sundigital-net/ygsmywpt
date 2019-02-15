using MyWebApi.DTO;
using MyWebApi.Entity;
using MyWebApi.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Service
{
    public class ProductService : IProductService
    {
        private readonly MyDbContext _myDbContext;
        public ProductService(MyDbContext dbContext)
        {
            _myDbContext = dbContext;
        }
        public ProductDTO[] GetAll()
        {
            var products= _myDbContext.Products.OrderBy(t => t.Id).ToList();
            if (products == null)
                return null;
            List<ProductDTO> list = new List<ProductDTO>();
            foreach(var pro in products)
            {
                list.Add(ToDTO(pro));
            }
            return list.ToArray();
        }
        private ProductDTO ToDTO(ProductEntity entity)
        {
            ProductDTO dto = new ProductDTO()
            {
                Id = entity.Id,
                Name=entity.Name,
                Price=entity.Price,
                Description=entity.Description,
            };
            return dto;
        }
        public ProductDTO GetById(long id)
        {
            var product = _myDbContext.Products.FirstOrDefault(t => t.Id == id);
            return product == null ? null : ToDTO(product);
        }
    }
}
