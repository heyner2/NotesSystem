using System.ComponentModel.DataAnnotations;

namespace NotesSystem.Models
{
    public class Grade
    {
        [Key]
        public int idGrade { get; set; }

        public string gradeText { get; set; }
    }
}