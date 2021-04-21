using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotesSystem.Models
{
    public class Notes
    {
        [Key]
        public int idNote { get; set; }

        public string Description { get; set; }

        public double note { get; set; }

        public int idStudent { get; set; }

        public virtual Student student { get; set; }

    }
}
