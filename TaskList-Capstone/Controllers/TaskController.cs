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
        public IActionResult Index()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Tasks> thisUsersTasks = _context.Tasks.Where(x => x.UserId == id).ToList();

            return View(thisUsersTasks);
        }

        //(C)rud - Create tasks
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