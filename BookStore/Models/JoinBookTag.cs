using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class JoinBookTag
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
