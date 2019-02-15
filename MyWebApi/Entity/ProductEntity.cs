using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Entity
{
    public class ProductEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
