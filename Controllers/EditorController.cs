using Congratulator.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace Congratulator.Controllers
{
    public class EditorController : Controller
    {
        private readonly DBManager dbManager = new DBManager();
        private readonly IWebHostEnvironment webHostEnvironment;

        public EditorController(IWebHostEnvironment webHostEnvironment) : base()
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(dbManager.GetPeopleSortByDate(true));
        }

        public IActionResult Create()
        {
            Person person = new Person();

            return PartialView("_CreateModalView", person);
        }

        [HttpPost]
        public IActionResult Create(Person person)
        {
            if (!string.IsNullOrEmpty(person.Name) || !string.IsNullOrEmpty(person.BirthdayDate))
            {
                if (person.UploadPhoto != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(person.UploadPhoto.FileName);
                    string extention = Path.GetExtension(person.UploadPhoto.FileName);
                    person.PhotoName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extention;
                    string path = Path.Combine(wwwRootPath + "/personsPhotos/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        person.UploadPhoto.CopyTo(fileStream);
                    }
                }

                dbManager.CreatePerson(person);

                ModelState.Clear();
            }

            return RedirectToAction(nameof(Index), "Editor");
        }

        public IActionResult Edit(int id)
        {
            Person person = dbManager.GetPerson(id);
            string wwwRootPath = webHostEnvironment.WebRootPath;
            string path = Path.Combine(wwwRootPath + "/personsPhotos/", person.PhotoName);
            using (var stream = System.IO.File.OpenRead(path))
            {
                person.UploadPhoto = new FormFile(stream, 0, stream.Length, person.PhotoName, Path.GetFileName(stream.Name));
            }

            return PartialView("_EditModalView", person);
        }

        [HttpPost]
        public IActionResult Edit(Person person)
        {
            if (person.UploadPhoto != null)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;

                string currentPhotoPath = Path.Combine(wwwRootPath + "/personsPhotos/", dbManager.GetPerson(person.ID).PhotoName);

                if (System.IO.File.Exists(currentPhotoPath)) 
                {
                    System.IO.File.Delete(currentPhotoPath);
                } 

                string fileName = Path.GetFileNameWithoutExtension(person.UploadPhoto.FileName);
                string extention = Path.GetExtension(person.UploadPhoto.FileName);
                person.PhotoName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extention;
                string path = Path.Combine(wwwRootPath + "/personsPhotos/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    person.UploadPhoto.CopyTo(fileStream);
                }
            }

            dbManager.SavePerson(person.ID, person);

            ModelState.Clear();

            return RedirectToAction(nameof(Index), "Editor");
        }

        [HttpPost]
        public IActionResult Delete(Person person)
        {
            if (dbManager.IsUserHas(person.ID))
            {
                if (!string.IsNullOrEmpty(dbManager.GetPerson(person.ID).PhotoName))
                {
                    string currentPhotoPath = Path.Combine(webHostEnvironment.WebRootPath + "/personsPhotos/",
                        dbManager.GetPerson(person.ID).PhotoName);

                    if (System.IO.File.Exists(currentPhotoPath))
                    {
                        System.IO.File.Delete(currentPhotoPath);
                    }
                }

                dbManager.DeletePerson(person.ID);
            }

            ModelState.Clear();

            return RedirectToAction(nameof(Index), "Editor");
        }
    }
}
