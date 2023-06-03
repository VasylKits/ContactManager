using ContactManager.Db;
using ContactManager.DTOs;
using ContactManager.Implementations.Interfaces;
using ContactManager.Models;
using ContactManager.Shared;
using ContactManager.Shared.Result.Interfaces.Generics;
using ContactManager.Shared.Result.Interfaces.Implementations.Generics;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ContactManager.Implementations.Services;

public class ContactService : IContactService
{
    private readonly ContactContext _context;

    public ContactService(ContactContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<Contact>>> GetContactsAsync()
    {
        try
        {
            var contacts = await _context.Contacts.ToListAsync();

            if (contacts.Count == 0)
                return Result<List<Contact>>.CreateFailed(ErrorModel.ContactsNotFound);

            return Result<List<Contact>>.CreateSuccess(contacts);
        }
        catch (Exception e)
        {
            return Result<List<Contact>>.CreateFailed(e.Message);
        }
    }

    public async Task<IResult<Contact>> CreateContactAsync(NewContactModel newContact)
    {
        try
        {
            var contact = new Contact()
            {
                Name = newContact.Name,
                DateOfBirth = newContact.DateOfBirth,
                Married = newContact.Married,
                Phone = newContact.Phone,
                Salary = newContact.Salary
            };

            var result = _context.Contacts.Add(contact);

            await _context.SaveChangesAsync();

            return result is not null
                ? Result<Contact>.CreateSuccess(contact)
                : Result<Contact>.CreateFailed(ErrorModel.ContactIsNotCreated);
        }
        catch (Exception e)
        {
            return Result<Contact>.CreateFailed(e.Message);
        }
    }

    public async Task<IResult<Contact>> EditContactAsync(EditContactModel editContact)
    {
        try
        {
            var contact = await _context.Contacts.FindAsync(editContact.Id);

            if (contact is null)
                return Result<Contact>.CreateFailed(ErrorModel.ContactsNotFound);

            contact.Name = editContact.Name;
            contact.DateOfBirth = editContact.DateOfBirth;
            contact.Married = editContact.Married;
            contact.Phone = editContact.Phone;
            contact.Salary = editContact.Salary;

            var result = _context.Contacts.Update(contact);

            await _context.SaveChangesAsync();

            return result is not null
                ? Result<Contact>.CreateSuccess(contact)
                : Result<Contact>.CreateFailed(ErrorModel.ContactIsNotEdited);
        }
        catch (Exception e)
        {
            return Result<Contact>.CreateFailed(e.Message);
        }
    }

    public async Task<IResult<Contact>> RemoveContactAsync(int id)
    {
        try
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact is null)
                return Result<Contact>.CreateFailed(ErrorModel.ContactsNotFound);

            var result = _context.Contacts.Remove(contact);

            await _context.SaveChangesAsync();

            return result is not null
                ? Result<Contact>.CreateSuccess(contact)
                : Result<Contact>.CreateFailed(ErrorModel.ContactIsNotDeleted);

        }
        catch (Exception e)
        {
            return Result<Contact>.CreateFailed(e.Message);
        }
    }

    public async Task<IResult<List<Contact>>> UploadContacts(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return Result<List<Contact>>.CreateFailed(ErrorModel.FileNotFound);

            var contacts = new List<Contact>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var csvContacts = csv.GetRecords<CsvContact>();

                foreach (var csvContact in csvContacts.ToList())
                {
                    DateTime dateOfBirth = DateTime.Now;

                    if (!string.IsNullOrWhiteSpace(csvContact.DateOfBirth))
                    {
                        dateOfBirth = DateTime.ParseExact(csvContact.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    }

                    var contact = new Contact
                    {
                        Name = csvContact.Name,
                        DateOfBirth = dateOfBirth,
                        Married = bool.Parse(csvContact.Married),
                        Phone = csvContact.Phone,
                        Salary = decimal.Parse(csvContact.Salary)
                    };

                    contacts.Add(contact);
                }
            }

            var result = _context.Contacts.AddRangeAsync(contacts);
            await _context.SaveChangesAsync();

            return result is not null
                ? Result<List<Contact>>.CreateSuccess(contacts)
                : Result<List<Contact>>.CreateFailed(ErrorModel.ContactIsNotUploaded);
        }
        catch(Exception e)
        {
            return Result<List<Contact>>.CreateFailed(ErrorModel.ContactIsNotUploaded);
        }
    }
}
