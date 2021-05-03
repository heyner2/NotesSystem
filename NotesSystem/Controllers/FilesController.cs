using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NotesSystem.Controllers
{
    public class FilesController : Controller
    {
        private IHostingEnvironment _env;
        private string dir;

        public FilesController(IHostingEnvironment env)
        {
            _env = env;
            dir = _env.ContentRootPath;
        }

        public IActionResult IndexFile() {
           
            
            return View();
        } 

        public IActionResult UploadFile(IFormFile file)
        
        {
          
             
            using(var fileStream=new FileStream(Path.Combine(dir+"/Files",file.FileName), FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }
            return RedirectToAction("IndexFile");
        }

        public IActionResult UploadFiles(ICollection<IFormFile>  files)

        {

            foreach(var item in files)
            {
                using (var fileStream = new FileStream(Path.Combine(dir + "/Files", item.FileName), FileMode.Create, FileAccess.Write))
                {
                    item.CopyTo(fileStream);
                }
            }
           
            return RedirectToAction("IndexFile");
        }

        public IActionResult FileInModel(FileForm file)
        {

            using (var fileStream = new FileStream
                (Path.Combine(dir + "/Files", file.name), 
                FileMode.Create, FileAccess.Write))
            {
              file.file.CopyTo(fileStream);
            }
            return RedirectToAction("IndexFile");
        }
    }
}
