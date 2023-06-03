using ContactManager.Db;
using ContactManager.DTOs;
using ContactManager.Implementations.Interfaces;
using ContactManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContactManager.Controllers;

public class ContactsController : Controller
{
    private readonly ContactContext _context;
    private readonly IContactService _contactService;

    public ContactsController(
        ContactContext context,
        IContactService contactService)
    {
        _context = context;
        _contactService = contactService;
    }

    public async Task<IActionResult> Index()
    {
        var contacts = await _contactService.GetContactsAsync();

        if (!contacts.Success)
            return RedirectToAction("Error");

        return View(contacts.Data);
    }

    public IActionResult Error()
    {
        var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
        return View("Error", errorViewModel);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewContactModel newContact)
    {
        var result = await _contactService.CreateContactAsync(newContact);

        if (!result.Success)
            return BadRequest();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);

        if (contact == null)
            return NotFound();

        return View(contact);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditContactModel editContact)
    {
        var contact = await _contactService.EditContactAsync(editContact);

        if (!contact.Success)
            return RedirectToAction("Error");

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int id)
    {
        var contact = await _contactService.RemoveContactAsync(id);

        if (!contact.Success)
            return NotFound();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UploadContacts(IFormFile file)
    {
        await _contactService.UploadContacts(file);

        return RedirectToAction("Index");
    }
}