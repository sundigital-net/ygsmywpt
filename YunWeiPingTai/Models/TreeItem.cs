using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YunWeiPingTai.Models
{
    public class TreeItem<T>
    {
        public T Item { get; set; }
        public IEnumerable<TreeItem<T>> Children { get; set; }
    }
}
