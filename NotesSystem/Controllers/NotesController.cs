using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotesSystem.Data;
using NotesSystem.Models;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)
        {
            List<Student> student = await _context.Student.Where(s => s.name.Contains(name)).ToListAsync();
            if (student.Count == 0)
            {
                student = await _context.Student.Where(s => s.name.Equals(name)).ToListAsync();
                
            }
            return View(student);
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
