using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YunWeiPingTai.Models
{
    public class MenuAddEditPostModel
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string IconUrl { get; set; }
        public string LinkUrl { get; set; }
        public int? Sort { get; set; }
        public string Permission { get; set; }
    }
}
