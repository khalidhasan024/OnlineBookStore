using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tag Name")]
        public string Name { get; set; }


        public ICollection<JoinBookTag> BookTags { get; set; }

    }
}
