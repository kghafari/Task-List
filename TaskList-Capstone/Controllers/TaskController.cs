using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskList_Capstone.Models;

namespace TaskList_Capstone.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskListDbContext _context;

        public TaskController(TaskListDbContext context)
        {
            _context = context;
        }

        //c(R)ud - Read tasks    
        public IActionResult Index(string searchBy, string sortBy, string search)
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Tasks> thisUsersTasks = _context.Tasks.Where(x => x.UserId == id).ToList();            

            //search cases
            switch (searchBy)
            {
                case "Name":
                    //search by name stuff
                    List<Tasks> foundByNameTasks = thisUsersTasks.Where(x => x.TaskName.Contains(search) || search == null).ToList();
                    return View(foundByNameTasks);
                case "DueDate":
                    //search by date stuff
                    List<Tasks> foundByDateTasks = thisUsersTasks.Where(x => x.DueDate.ToShortDateString().ToString().Contains(search) || search == null).ToList();
                    return View(foundByDateTasks);

                case "Description":
                    //search by description stuff
                    List<Tasks> foundByDescripTasks = thisUsersTasks.FindAll(x => x.Description.Contains(search) || search == null).ToList();
                    return View(foundByDescripTasks);
                default:
                    break;
            }

            //sort cases
            switch (sortBy)
            {
                case "Name":
                    List<Tasks> sortByNameTasks = thisUsersTasks.OrderBy(x => x.TaskName).ToList();
                    return View(sortByNameTasks);
                case "DueDate":
                    List<Tasks> sortByDateTasks = thisUsersTasks.OrderBy(x => x.DueDate).ToList();
                    return View(sortByDateTasks);
                case "Category":
                    List<Tasks> sortByCategoryTasks = thisUsersTasks.OrderBy(x => x.Category).ToList();
                    return View(sortByCategoryTasks);
                default:
                    break;
            }
            return View(thisUsersTasks);
        }


        //public IActionResult Index2(string searchBy, string sortBy, string search)
        //{
        //    string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    List<Tasks> thisUsersTasks = _context.Tasks.Where(x => x.UserId == id).ToList();

        //    //search cases
        //    switch (searchBy)
        //    {
        //        case "Name":
        //            //search by name stuff
        //            List<Tasks> foundByNameTasks = thisUsersTasks.Where(x => x.TaskName.Contains(search) || search == null).ToList();
        //            return View(foundByNameTasks);
        //        case "DueDate":
        //            //search by date stuff
        //            List<Tasks> foundByDateTasks = thisUsersTasks.Where(x => x.DueDate.ToShortDateString().ToString().Contains(search) || search == null).ToList();
        //            return View(foundByDateTasks);

        //        case "Description":
        //            //search by description stuff
        //            List<Tasks> foundByDescripTasks = thisUsersTasks.FindAll(x => x.Description.Contains(search) || search == null).ToList();
        //            return View(foundByDescripTasks);
        //        default:
        //            break;
        //    }

        //    //sort cases

        //    switch (sortBy)
        //    {
        //        case "Name":
        //            List<Tasks> sortByNameTasks = thisUsersTasks.OrderBy(x => x.TaskName).ToList();
        //            return View(sortByNameTasks);
        //        case "DueDate":
        //            return View();

        //        default:
        //            break;
        //    }



        //    return View(thisUsersTasks);
        //}



        //(C)rud - Create tasks


        //c(R)ud - Read tasks   
        [HttpGet]
        public IActionResult AddTask()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTask(Tasks newTask)
        {
            newTask.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(ModelState.IsValid)
            {
                _context.Tasks.Add(newTask);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //cru(D) - Delete tasks
        public IActionResult DeleteTask(int id)
        {
            Tasks foundTask = _context.Tasks.Find(id);
            if(foundTask != null)
            {
                _context.Remove(foundTask);
                _context.SaveChanges();
                
            }
            return RedirectToAction("Index");
        }

        //cr(U)d - Update tasks
        //setup action to mark task complete
        public IActionResult MarkTaskComplete(int id)
        {
            Tasks foundTask = _context.Tasks.Find(id);
            if(foundTask != null)
            {
                foundTask.Complete = true;

                _context.Entry(foundTask).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(foundTask);
                _context.SaveChanges();
               
            }
            return RedirectToAction("Index");
        }

        //edit task
        [HttpGet]
        public IActionResult EditTask(int id)
        {
            Tasks foundTask = _context.Tasks.Find(id);
            if(foundTask != null)
            {
                return View(foundTask);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditTask(Tasks updatedTask)
        {
            Tasks dbTask = _context.Tasks.Find(updatedTask.Id);
            if(ModelState.IsValid)
            {
                dbTask.TaskName = updatedTask.TaskName;
                dbTask.DueDate = updatedTask.DueDate;
                dbTask.Category = updatedTask.Category;
                dbTask.Description = updatedTask.Description;
                dbTask.Complete = updatedTask.Complete;
                
                _context.Entry(dbTask).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(dbTask);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }



    }
}