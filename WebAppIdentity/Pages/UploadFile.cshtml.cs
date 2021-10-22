using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppIdentity.Attributes;

namespace WebAppIdentity.Pages
{

    public class UploadFileModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        public UploadFileModel(IWebHostEnvironment environment)
        {
            this._environment = environment;
        }

        [BindProperty(), Display(Name = "File")]
        public IFormFile UploadedFile { get; set; }


        public async void OnPostUploadFile()
        {
            if (!ModelState.IsValid)
            {
                return;
            }


            var fileFolder = Path.Combine(this._environment.ContentRootPath, "uploads");
            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }
            var filePath = Path.Combine(fileFolder, this.UploadedFile.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await this.UploadedFile.CopyToAsync(fileStream);
            }

        }
        public void OnGet()
        {
        }
    }
}
