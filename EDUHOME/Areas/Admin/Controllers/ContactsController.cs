using Allup.Data;
using EDUHOME.Areas.Admin.Data;
using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
            var model = await _dbContext.Contacts
                .FirstOrDefaultAsync();

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            if (!model.TelIcon.IsImage())
            {
                ModelState.AddModelError("TelIcon", "Shekil Secmelisiz");
                return View();
            }

            if (!model.TelIcon.IsAllowedSize(10))
            {
                ModelState.AddModelError("TelIcon", "Shekilin olcusu 10 mbdan az omalidi");
                return View();
            }

            var unicalNameTel = await model.TelIcon.GenerateFile(Constants.ContactPath);

            if (!model.WebsiteIcon.IsImage())
            {
                ModelState.AddModelError("WebsiteIcon", "Shekil Secmelisiz");
                return View();
            }

            if (!model.WebsiteIcon.IsAllowedSize(10))
            {
                ModelState.AddModelError("WebsiteIcon", "Shekilin olcusu 10 mbdan az omalidi");
                return View();
            }

            var unicalNameWebsite = await model.TelIcon.GenerateFile(Constants.ContactPath);

            if (!model.AddressIcon.IsImage())
            {
                ModelState.AddModelError("AddressIcon", "Shekil Secmelisiz");
                return View();
            }

            if (!model.WebsiteIcon.IsAllowedSize(10))
            {
                ModelState.AddModelError("AddressIcon", "Shekilin olcusu 10 mbdan az omalidi");
                return View();
            }

            var unicalNameAddress = await model.AddressIcon.GenerateFile(Constants.ContactPath);


            await _dbContext.Contacts.AddAsync(new Contact
            {
                Address = model.Address,
                AddressImageUrl = unicalNameAddress,
                Website = model.Website,
                WebsiteImageUrl = unicalNameWebsite,
                ContactNumber = model.Number,
                ContactNumberImageUrl = unicalNameTel,
                Message = model.Message,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var contact = await _dbContext.Contacts
                .Where(contact => contact.Id == id)
                .FirstOrDefaultAsync();

            if (contact == null) return BadRequest();

            var model = new ContactUpdateViewModel
            {
                Address = contact.Address,
                AddressImageUrl = contact.AddressImageUrl,
                Website = contact.Website,
                WebsiteImageUrl = contact.WebsiteImageUrl,
                Number = contact.ContactNumber,
                TelImageUrl = contact.ContactNumberImageUrl,
                Message = contact.Message,
                IsDeleted = contact.IsDeleted,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, ContactUpdateViewModel model)
        {
            if (id == null) return BadRequest();

            var existContact = await _dbContext.Contacts
                .Where(contact => contact.Id == id)
                .FirstOrDefaultAsync();

            if (existContact == null) return BadRequest();

            var viewModel = new ContactUpdateViewModel
            {
                AddressImageUrl = model.AddressImageUrl,
                TelImageUrl = model.TelImageUrl,
                WebsiteImageUrl = model.WebsiteImageUrl,
            };

            if (!ModelState.IsValid) return View(viewModel);

            if (model.AddressIcon != null )
            {
                if (!model.AddressIcon.IsImage())
                {
                    ModelState.AddModelError("", "Shekil Secmelisiz");
                    return View(viewModel);
                }

                if (!model.AddressIcon.IsAllowedSize(10))
                {
                    ModelState.AddModelError("", "Shekilin olcusu 10 mbdan az omalidi");
                    return View(viewModel);
                }

                var path = Path.Combine(Constants.RootPath, "assets", "images", "contact", existContact.AddressImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.AddressIcon.GenerateFile(Constants.ContactPath);

                existContact.AddressImageUrl = unicalName;
            }

            if (model.TelIcon != null)
            {
                if (!model.TelIcon.IsImage())
                {
                    ModelState.AddModelError("Image", "Shekil Secmelisiz");
                    return View(viewModel);
                }

                if (!model.TelIcon.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "Shekilin olcusu 10 mbdan az omalidi");
                    return View(viewModel);
                }

                var path = Path.Combine(Constants.RootPath, "assets", "images", "contact", existContact.ContactNumberImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.TelIcon.GenerateFile(Constants.ContactPath);

                existContact.ContactNumberImageUrl = unicalName;
            }

            if (model.WebsiteIcon != null)
            {
                if (!model.WebsiteIcon.IsImage())
                {
                    ModelState.AddModelError("Image", "Shekil Secmelisiz");
                    return View(viewModel);
                }

                if (!model.WebsiteIcon.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "Shekilin olcusu 10 mbdan az omalidi");
                    return View(viewModel);
                }

                var path = Path.Combine(Constants.RootPath, "assets", "images", "contact", existContact.WebsiteImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.WebsiteIcon.GenerateFile(Constants.ContactPath);

                existContact.WebsiteImageUrl = unicalName;
            }

            existContact.IsDeleted = model.IsDeleted;
            existContact.Message = model.Message;
            existContact.Address = model.Address;
            existContact.Website = model.Website;
            existContact.ContactNumber = model.Number;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var existContact = await _dbContext.Contacts
                .Where(contact => contact.Id == id)
                .FirstOrDefaultAsync();

            if (existContact == null) return BadRequest();

            _dbContext.Contacts.Remove(existContact);

            var pathAdress = Path.Combine(Constants.RootPath, "assets", "images", "contact", existContact.AddressImageUrl);
            var pathNumber = Path.Combine(Constants.RootPath, "assets", "images", "contact", existContact.ContactNumberImageUrl);
            var pathWebsite = Path.Combine(Constants.RootPath, "assets", "images", "contact", existContact.WebsiteImageUrl);

            if (System.IO.File.Exists(pathAdress))
                System.IO.File.Delete(pathAdress);

            if (System.IO.File.Exists(pathNumber))
                System.IO.File.Delete(pathNumber);

            if (System.IO.File.Exists(pathWebsite))
                System.IO.File.Delete(pathWebsite);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
