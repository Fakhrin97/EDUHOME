using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class ContactsController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public ContactsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var contacts = await _dbContext.Contacts
                .ToListAsync();

            return View(contacts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactCreateViewModel model)
        {
            if (!ModelState.IsValid) return View();

            await _dbContext.Contacts.AddAsync(new Contact
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
            var contact = await _dbContext.Contacts
                .Where(contact => contact.Id == id)
                .FirstOrDefaultAsync();

            var model = new ContactUpdateViewModel
            {
                Id = id,
                Number1 = contact.Number1,
                Number2 = contact.Number2,
                Email = contact.Email,
                Website = contact.Website,
                Adress = contact.Adress,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ContactUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var existContact = await _dbContext.Contacts
                 .Where(contact => contact.Id == id)
                 .FirstOrDefaultAsync();         

            existContact.Adress=model.Adress;
            existContact.Number1=model.Number1;
            existContact.Number2=model.Number2;
            existContact.Email=model.Email;
            existContact.Website=model.Website;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existContact = await _dbContext.Contacts
                .Where(contact => contact.Id == id)
                .FirstOrDefaultAsync();

            if (existContact == null) return BadRequest();

            _dbContext.Contacts.Remove(existContact);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
