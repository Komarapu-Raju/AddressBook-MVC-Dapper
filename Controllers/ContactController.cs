﻿using AddressBook.Models;
using AddressBook.Services;
using AddressBook.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactServices _contactServices;
        private readonly IMapper mapper;

        public ContactController(IContactServices contactServices, IMapper mapper)
        {
            _contactServices = contactServices;
            this.mapper = mapper;
        }

        public IActionResult AddContact()
        {
            return View("ContactForm");
        }

        public IActionResult EditContact(int id)
        {
            return View("ContactForm", mapper.Map<Contact>(_contactServices.GetContactById(id)));
        }

        [HttpPost]
        public IActionResult ContactForm(Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (_contactServices.DoesContactExist(contact.Id))
                {
                    _contactServices.UpdateContact(contact);
                    return RedirectToAction("ContactDetails", new { contact.Id });
                }

                _contactServices.AddContact(contact);
                return RedirectToAction("ContactDetails", new { _contactServices.GetContactsList().Last<ContactListViewModel>().Id });
            }

            return View(contact);
        }

        public IActionResult ContactDetails(int id)
        {
            return View(_contactServices.GetContactById(id));
        }

        public IActionResult DeleteContact(int id)
        {
            _contactServices.DeleteContact(id);
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            ContactListViewModel? contact = _contactServices.GetContactsList().FirstOrDefault();

            if (contact != null)
            {
                return RedirectToAction("ContactDetails", new { contact.Id });
            }

            return View();
        }

        public IActionResult CloseForm()
        {
            return RedirectToAction("Index");
        }
    }
}
