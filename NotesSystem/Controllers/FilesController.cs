using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using NotesSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.HttpContext.Current.Server;


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

            var items = GetFiles();
            return View(items);
        } 

        public IActionResult UploadFile(IFormFile file)
        
        {

            try {
                using (var fileStream = new FileStream(Path.Combine(dir + "/Files", file.FileName), FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }
                ViewBag.Message = "File updated succesfully.";
            }
            catch(Exception ex)
            {
                ViewBag.Message = "ups something when wrong "+ex.Message;
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

        private List<string> GetFiles()
        {
            //to access all files from this directory
           var directory = new System.IO.DirectoryInfo(dir + "~/Files");
            //to get all files if any specific name file then replace the * on the left
            //if any specific format such as .jpg then replace the * from the right 
            System.IO.FileInfo[] FileNames = directory.GetFiles("*.*");

            List<string> items = new List<string>();
            foreach(var file in FileNames)
            {
                items.Add(file.Name);
            }

            return items;
        }

        public FileResult Download(string fileName)
        {
            var FileVirtualPath = Path.Combine(dir,"Files", fileName) ;
            
            var contentType = GetContentType(fileName);
       
           
            var file = File(FileVirtualPath, contentType, Path.GetFileName(FileVirtualPath));
            if (System.IO.File.Exists(FileVirtualPath))
            {
                return file;
            }
            else
            {
                return null;
            }
              
        }

        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }

     
}
