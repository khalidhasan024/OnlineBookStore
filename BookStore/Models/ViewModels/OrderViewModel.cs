using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public IdentityUser User { get; set; }
        public List<BookOrder> BookOrders { get; set; }
    }
}
