using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class TagList
    {
        [Key]
        public int TagID { get; set; }
        [Required]
        [Display(Name ="Tag Name")]
        public string TagName { get; set; }
    }
}
