using ContactManager.DTOs;
using ContactManager.Models;
using ContactManager.Shared.Result.Interfaces.Generics;

namespace ContactManager.Implementations.Interfaces;

public interface IContactService
{
    Task<IResult<List<Contact>>> GetContactsAsync();
    Task<IResult<Contact>> CreateContactAsync(NewContactModel newContact);
    Task<IResult<Contact>> EditContactAsync(EditContactModel editContact);
    Task<IResult<Contact>> RemoveContactAsync(int id);
    Task<IResult<List<Contact>>> UploadContacts(IFormFile file);
}
