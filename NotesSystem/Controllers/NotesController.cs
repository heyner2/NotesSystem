using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotesSystem.Data;
using NotesSystem.Models;
using NotesSystem.Models.ViewModels;
using System.Linq.Dynamic.Core; 

namespace NotesSystem.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            var viewModel = new ViewModel();

            viewModel.students = await _context.Student.ToListAsync();
            return View(await _context.Notes.ToListAsync() );
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notes = await _context.Notes
                .FirstOrDefaultAsync(m => m.idNote == id);
            if (notes == null)
            {
                return NotFound();
            }

            return View(notes);
        }

        // GET: Notes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(ViewModel viewModel)
        {
            int id = viewModel.note.idStudent;

          
          

            if (viewModel.note != null)
            {
                _context.Add(viewModel.note);
                _context.SaveChanges();
            }
           
            List<Notes> notes = await _context.Notes.Where(n => n.idStudent.Equals(id)).ToListAsync();
            var student = await _context.Student.FindAsync(id);
            ViewModel vm = new ViewModel();
            vm.notes = notes;
            vm.student = student;
            List<Notes> notesExam = vm.notes.Where(n => n.Description.Equals("Exam")).ToList();
            List<Notes> notesHw = vm.notes.Where(n => n.Description.Equals("HomeWork")).ToList();
            foreach (var not in notesExam)
            {
                vm.totalExams += not.note;
            }
            if (vm.totalExams != 0)
            {
                vm.totalExams = vm.totalExams / notesExam.Count();

            }
           
            

            foreach (var not in notesHw)
            {
                vm.totalhomeWork += not.note;
            }
            if (vm.totalhomeWork != 0)
            {
                vm.totalhomeWork = vm.totalhomeWork / notesHw.Count(); ;

            }
           
           

            vm.finalScore = (vm.totalExams * 0.30) + (vm.totalhomeWork * 0.70);
            return View("Index",vm);
        }
      

        public IActionResult getNotes(int? id)
        {

          
            List<Notes> notes =  _context.Notes.Where(n => n.idStudent.Equals(id)).ToList();
            Student student =  _context.Student.Find(id);
            ViewModel vm = new ViewModel();
            vm.notes = notes;
            vm.student = student;
            List<Notes> notesExam = vm.notes.Where(n => n.Description.Equals("Exam")).ToList();
            List<Notes> notesHw = vm.notes.Where(n => n.Description.Equals("HomeWork")).ToList();
            foreach (var not in notesExam)
            {
                vm.totalExams += not.note;
            }
            if (vm.totalExams != 0)
            {
                vm.totalExams = vm.totalExams / notesExam.Count();

            }



            foreach (var not in notesHw)
            {
                vm.totalhomeWork += not.note;
            }
            if (vm.totalhomeWork != 0)
            {
                vm.totalhomeWork = vm.totalhomeWork / notesHw.Count(); ;

            }


            vm.finalScore = (vm.totalExams * 0.30) + (vm.totalhomeWork * 0.70);
            return View("Index", vm);
        }

        [HttpPost]

        public async Task<PartialViewResult> Create(string name)
        {
            List<Student> student = await _context.Student.Where(s => s.name.Contains(name)).ToListAsync();
            if (student.Count == 0)
            {
                student = await _context.Student.Where(s => s.name.Equals(name)).ToListAsync();
                
            }

            if (student.Count == 0)
            {
                ViewBag.errorMessage = "Student not found";
            }
           
            return PartialView("_StudentTable", student);
        }


        public  ActionResult CreateTable(string name)
        {
            List<Student> student =  _context.Student.Where(s => s.name.Contains(name)).ToList();
            if (student.Count == 0)
            {
                student = _context.Student.Where(s => s.name.Equals(name)).ToList();

            }

            if (student.Count == 0)
            {
                ViewBag.errorMessage = "Student not found";
            }

            return PartialView("_StudentTable", student);
        }


        public JsonResult GetTable(string name)
        {
            List<Student> students = _context.Student.Where(s => s.name.Contains(name)).ToList();
            if (students.Count == 0)
            {
                students = _context.Student.Where(s => s.name.Equals(name)).ToList();

            }

            if (students.Count == 0)
            {
                ViewBag.errorMessage = "Student not found";
            }

            return Json(students);
        }


        public IActionResult ListStudent()
        {
            return View();
        }

        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

        //by default every json request is a post
        [HttpPost]
        public IActionResult Json()
        {
            //here we extract the data sent by the framework datatable
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["Columns["+Request.Form["order[0][column]"].FirstOrDefault()+"][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            //we validate the data received 
            pageSize = !length.Equals("") ? Convert.ToInt32(length) : 0;
            skip = !start.Equals("") ? Convert.ToInt32(start) : 0;
            recordsTotal = 0;
            //we create a list to pass later the query
            List<ViewModelStudent> student;

            /* we create a query which will hold just the query to avoid pulling all date before pagination and search
            for optimization porpouses it faster to get just 10 item than 100000 then we equal to the model view
            to get only the values we want to pass to the view not all of the values from object we could also do 
            orderby sortColumn+""+sortColumnDir to sort but it was done later by linq.dinamyc.core to validate it has an order with if */
            IQueryable<ViewModelStudent> query = (from a in _context.Student where a.name.Contains(searchValue) select new ViewModelStudent{ 
            name=a.name,
            age=a.age
            });

            //we create the query by the search value here we are missing to validate all fields from the view
            if (searchValue != "")
            {
                query = query.Where(a => a.name.Contains(searchValue));
            }

            //we check sortcolumn and sortcolumn direction
            if(!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                //to do order by it was necessary to add the reference or package from nugget System.Linq.Dynamic.Core
                query = query.OrderBy(sortColumn + " " + sortColumnDir);
            }
            recordsTotal = query.Count();

            student = query.Skip(skip).Take(pageSize).ToList();
            return Json(new {draw=draw, recordsFiltered= recordsTotal, recordsTotal=recordsTotal, data=student});
        }
          
        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        public async Task<IActionResult> CreateSearch()
        {

            List<Student> student = await _context.Student.ToListAsync();

            return View("Create",student);
        }


        public async Task<IActionResult> View(int? id)
        {
            ViewModel viewModel = new ViewModel();
            var student = await _context.Student.FindAsync(id);
            viewModel.student = student;

            return View("View", viewModel);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notes = await _context.Notes.FindAsync(id);
            if (notes == null)
            {
                return NotFound();
            }
            return View(notes);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( Notes notes)
        {
       
                    _context.Add(notes);
                    await _context.SaveChangesAsync();

            ViewModel viewModel = new ViewModel();
            viewModel.student =(Student) _context.Student.Where(s => s.idStudent.Equals(notes.idStudent));
            viewModel.notes =  await _context.Notes.Where(n => n.idStudent.Equals(notes.idStudent)).ToListAsync();

            return View("index", viewModel);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notes = await _context.Notes
                .FirstOrDefaultAsync(m => m.idNote == id);
            if (notes == null)
            {
                return NotFound();
            }

            return View(notes);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notes = await _context.Notes.FindAsync(id);
            _context.Notes.Remove(notes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotesExists(int id)
        {
            return _context.Notes.Any(e => e.idNote == id);
        }
    }
}
