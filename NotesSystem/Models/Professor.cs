using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotesSystem.Models
{
    public class Professor
    {
        [Key]
        public int idProfessor { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public List<Grade> grades { get; set; }
    }
}
