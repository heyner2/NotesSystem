using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotesSystem.Models
{
    public class Student
    {
        [Key]
        public int idStudent { get; set; }

        public string name { get; set; }

        public int Idgrade { get; set; }

        public int age { get; set; }

        public virtual Grade grade { get; set; }


    }
}
