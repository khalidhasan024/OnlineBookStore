using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.ViewModels
{
    public class OrderIndexViewModel
    {
        public List<Order> Orders { get; set; }
        public string Title { get; set; }
    }
}
