using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesSystem.Models
{
    public class ViewModel
    {
        public List<Student>  students { get; set; }
        public Student student { get; set; }
        public List<Professor>  professors { get; set; }
        public Professor professor { get; set; }
        public List<Notes>    notes { get; set; }

         public Notes    note { get; set; }

        public double avgHomeWork { get; set; }

        public double avgExam { get; set; }

        public double totalhomeWork { get; set; }

        public double totalExams { get; set; }

        public double finalScore { get; set; }
    }
}
