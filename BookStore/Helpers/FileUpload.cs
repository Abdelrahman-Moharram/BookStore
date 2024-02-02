using BookStore.DTOs.Book;
using BookStore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Pipelines;

namespace BookStore.Helpers
{
    public class FileUpload
    {


        

        private string UploadUserImage(IFormFile Image, string inst_Image = "img/users/user.webp")
        {

            if (Image != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);

                var imagePath = Path.Combine("wwwroot", "img", "users", fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    Image.CopyToAsync(stream);
                }
                return "img/users/" + fileName;
            }
            return inst_Image;
        }

        public List<UploadedFile> UploadBook(List<IFormFile> Files, string Id)
        {
            List<UploadedFile> uploadedFiles = new();
            foreach (var file in Files)
            {

                UploadedFile uploadedFile = new()
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    bookId = Id
                };

                var path = Path.Combine("wwwroot", "books", file.FileName);

                using FileStream fileStream = new(path, FileMode.Create);
                file.CopyTo(fileStream);

                uploadedFiles.Add(uploadedFile);
            }

            return uploadedFiles;
        }
        


    }
}
