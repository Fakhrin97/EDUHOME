using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class FooterContactsController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public FooterContactsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var contacts = await _dbContext.FooterContacts
                .ToListAsync();

            return View(contacts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FooterContactCreateViewModel model)
        {
            if (!ModelState.IsValid) return View();

            await _dbContext.FooterContacts.AddAsync(new FooterContact
            {
                Adress = model.Adress,
                Number1 = model.Number1,
                Number2 = model.Number2,
                Email = model.Email,
                Website = model.Website
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var contact = await _dbContext.FooterContacts
                .Where(contact => contact.Id == id)
                .FirstOrDefaultAsync();

            var model = new FooterContactUpdateViewModel
            {
                Id = id,
                Number1 = contact.Number1,
                Number2 = contact.Number2,
                Email = contact.Email,
                Website = contact.Website,
                Adress = contact.Adress,
                IsDeleted = contact.IsDeleted,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, FooterContactUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var existContact = await _dbContext.FooterContacts
                 .Where(contact => contact.Id == id)
                 .FirstOrDefaultAsync();

            existContact.Adress = model.Adress;
            existContact.Number1 = model.Number1;
            existContact.Number2 = model.Number2;
            existContact.Email = model.Email;
            existContact.Website = model.Website;
            existContact.IsDeleted = model.IsDeleted;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existContact = await _dbContext.FooterContacts
                .Where(contact => contact.Id == id)
                .FirstOrDefaultAsync();

            if (existContact == null) return BadRequest();

            _dbContext.FooterContacts.Remove(existContact);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
