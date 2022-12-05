using Allup.Data;
using EDUHOME.Areas.Admin.Data;
using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class EventsController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public EventsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _dbContext.Events
                .Include(ev => ev.EventTeachers)
                .ThenInclude(ev => ev.Teacher)
                .ToListAsync();

            return View(events);
        }

        public async Task<IActionResult> Create()
        {
            var teachers = await _dbContext.Teachers
                .Where(teacher=>!teacher.IsDeleted)
                .ToListAsync();

            var teachersSelectedListItem = new List<SelectListItem>();

            teachers.ForEach(teacher => teachersSelectedListItem.Add(new SelectListItem(teacher.Name, teacher.Id.ToString())));

            var model = new EventCreateViewModel
            {
                Teachers = teachersSelectedListItem,
            };

            return View(model);  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateViewModel model)
        {
            var teachers = await _dbContext.Teachers
                .Where(teacher => !teacher.IsDeleted)
                .ToListAsync();

            var teachersSelectedListItem = new List<SelectListItem>();

            teachers.ForEach(teacher => teachersSelectedListItem.Add(new SelectListItem(teacher.Name, teacher.Id.ToString())));

            var viewModel = new EventCreateViewModel
            {
                Teachers = teachersSelectedListItem,
            };

            if(!ModelState.IsValid) return View(viewModel);


            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Shekil Secmelisiz");
                return View(viewModel);
            }

            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "Shekilin olcusu 10 mbdan az omalidi");
                return View(viewModel);
            }

            var unicalName = await model.Image.GenerateFile(Constants.EventPath);

            var createEvent = new Event
            {
                Title = model.Title,
                Content = model.Content,
                Reply = model.Reply,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Venue = model.Venue,    
                ImageUrl = unicalName,                 

            };

            var eventTeachers = new List<EventTeacher>();

            foreach (var id in model.TeachersId)
            {
                var eventTeacher = await _dbContext.Teachers
                    .Where(t => t.Id == id)
                    .FirstOrDefaultAsync();               

                eventTeachers.Add(new EventTeacher
                {
                    EventId = createEvent.Id,
                    TeacherId = eventTeacher.Id,
                });
                
            }

            createEvent.EventTeachers = eventTeachers;  

            await _dbContext.Events.AddAsync(createEvent);

            await _dbContext.SaveChangesAsync();    

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {           
            var existEvent = await _dbContext.Events
                .Where(ev => ev.Id == id)
                .Include(ev=>ev.EventTeachers)                            
                .FirstOrDefaultAsync();
            
            if (existEvent == null) return NotFound();

            var teachers = await _dbContext.Teachers
                .Where(teacher => !teacher.IsDeleted)
                .ToListAsync();

            List<int> selecedId = new();

            foreach (var item in existEvent.EventTeachers)
            {
                selecedId.Add(item.TeacherId);
            }   

            var teachersSelectedListItem = new List<SelectListItem>();

            teachers.ForEach(teacher => teachersSelectedListItem.Add(new SelectListItem(teacher.Name, teacher.Id.ToString())));

            var model = new EventUpdateViewModel
            {               
                Content =existEvent.Content,
                Title =  existEvent.Title,
                Venue = existEvent.Venue,   
                StartTime = existEvent.StartTime,   
                EndTime = existEvent.EndTime,   
                ImageUrl = existEvent.ImageUrl, 
                Reply = existEvent.Reply,   
                Teachers = teachersSelectedListItem,
                TeachersId =selecedId,
                IsDeleted = existEvent.IsDeleted,   
                
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id ,EventUpdateViewModel model)
        {
            var existEvent = await _dbContext.Events
                .Where(ev => ev.Id == id)
                .Include(ev=>ev.EventTeachers)
                .FirstOrDefaultAsync();

            if (existEvent == null) return BadRequest();

            var teachers = await _dbContext.Teachers
                .Where(teacher => !teacher.IsDeleted)
                .ToListAsync();

            List<int> selecedId = new();

            foreach (var item in existEvent.EventTeachers)
            {
                selecedId.Add(item.TeacherId);
            }

            var teachersSelectedListItem = new List<SelectListItem>();

            teachers.ForEach(teacher => teachersSelectedListItem.Add(new SelectListItem(teacher.Name, teacher.Id.ToString())));

            var viewModel = new EventUpdateViewModel
            {                
                Teachers = teachersSelectedListItem,
                TeachersId = selecedId,
            };

            if (!ModelState.IsValid) return View(viewModel);

            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Shekil Secmelisiz");
                    return View(viewModel);
                }

                if (!model.Image.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "Shekilin olcusu 10 mbdan az omalidi");
                    return View(viewModel);
                }

                var path = Path.Combine(Constants.RootPath, "assets", "images", "event", existEvent.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.Image.GenerateFile(Constants.EventPath);

                existEvent.ImageUrl = unicalName;
            }

            existEvent.IsDeleted = model.IsDeleted;
            existEvent.Title = model.Title; 
            existEvent.StartTime = model.StartTime;
            existEvent.EndTime = model.EndTime;
            existEvent.Reply = model.Reply;
            existEvent.Content = model.Content; 
            existEvent.Venue = model.Venue;

            var eventTeachers = new List<EventTeacher>();

            foreach (var teacherId in model.TeachersId)
            {
                var eventTeacher = await _dbContext.Teachers
                    .Where(t => t.Id == teacherId)
                    .FirstOrDefaultAsync();

                eventTeachers.Add(new EventTeacher
                {
                    EventId = existEvent.Id,
                    TeacherId = eventTeacher.Id,
                });

            }

            existEvent.EventTeachers = eventTeachers;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var existEvent = await _dbContext.Events
                .Where(ev => ev.Id == id)
                .FirstOrDefaultAsync();

            if (existEvent == null) return BadRequest();

            _dbContext.Events.Remove(existEvent);

            var path = Path.Combine(Constants.RootPath, "assets", "images", "event", existEvent.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
