using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesSystem.Models
{
    public class FileForm
    {

        public string name { get; set; }

        public IFormFile file { get; set; }
    }
}
