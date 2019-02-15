using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebApi.DTO;

namespace MyWebApi.IService
{
    public interface IProductService
    {
        ProductDTO GetById(long id);
        ProductDTO[] GetAll();
    }
}
