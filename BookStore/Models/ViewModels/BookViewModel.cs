using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.ViewModels
{
    public class BookViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<BookCategory> BookCategories { get; set; }
        public IEnumerable<Author> Authors { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
