using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.ViewModels
{
    public class IndexViewModel
    {
        public int PageNumber { get; set; }
        public bool CanGoNextPage { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
