using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BookCategory
    {
        public int Id { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        public ICollection<Book> Books { get; set; }

    }
}
