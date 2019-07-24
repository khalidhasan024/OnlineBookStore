using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int? Discount { get; set; }
        public string ImagePath { get; set; }
        public int? Stock { get; set; }

        public Author Author { get; set; }
        public int AuthorId { get; set; }

        public BookCategory Category { get; set; }
        public int? CategoryId { get; set; }

        public ICollection<JoinBookTag> BookTags { get; set; }
        public ICollection<BookOrder> BookOrders { get; set; }
    }
}
