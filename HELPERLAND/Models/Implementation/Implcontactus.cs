using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HELPERLAND.Controllers;
using HELPERLAND.Models.ViewModels;

namespace HELPERLAND.Models.Implementation;
public class Implcontactus{

    private readonly HelperlandContext context;
    private readonly IWebHostEnvironment webHostEnvironment;

    public Implcontactus(HelperlandContext context , IWebHostEnvironment webHostEnvironment)
    {
        this.context=context;
        this.webHostEnvironment = webHostEnvironment;
    }
    public void Add(ContactusViewmodel contact)
    {   
        ContactU newcontact= new ContactU(){
            Name=$"{contact.firstname} {contact.lastname}",
            PhoneNumber=contact.mono,
            Email=contact.email,
            Message=contact.msg,
            Subject=contact.subject,
            CreatedOn=DateTime.Now
        };
        string uniqueFilename;
        if(contact.File != null)  
        {
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "ContactUsImages");
			uniqueFilename = Guid.NewGuid().ToString() + "_" + contact.File.FileName;
			string filePath = Path.Combine(uploadsFolder, uniqueFilename);
			newcontact.UploadFileName = uniqueFilename;
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				contact.File.CopyTo(fileStream);
			}
        }
        context.ContactUs.Add(newcontact);
        context.SaveChanges();
    }
}
