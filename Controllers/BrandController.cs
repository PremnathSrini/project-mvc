using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_mvc.Data;
using project_mvc.Models;

namespace project_mvc.Controllers
{
	public class BrandController : Controller
    {

        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BrandController(AppDbContext dbContext,IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
		}

       
        [HttpGet]
        public IActionResult Index()
        {
            List<Brand> brands = _dbContext.Brand.ToList();
            return View(brands);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
		public IActionResult Create(Brand brand)
		{

			string WebRootPath = _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string NewFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(WebRootPath, @"images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                using(var fileStream = new FileStream(Path.Combine(upload, NewFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                brand.BrandLogo = @"images\brand" + NewFileName + extension ;

            }


            if (ModelState.IsValid)
            { 
                _dbContext.Brand.Add(brand);
                _dbContext.SaveChanges();

                TempData["success"] = "Record created succesfully";
                return RedirectToAction(nameof(Index));
            }
			return View();
		}

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x=>x.Id==id);
            return View(brand);
        }

		[HttpGet]
		public IActionResult Edit(Guid id)
		{
			Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);
			return View(brand);
		}

        [HttpPost]
        public IActionResult Edit(Brand brand) 
        {
			string WebRootPath = _webHostEnvironment.WebRootPath;

			var file = HttpContext.Request.Form.Files;

			if (file.Count > 0)
			{
				string NewFileName = Guid.NewGuid().ToString();

				var upload = Path.Combine(WebRootPath, @"images\brand");

				var extension = Path.GetExtension(file[0].FileName);

				var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

				if(objFromDb.BrandLogo != null)
                {
					var oldImgPath = Path.Combine(WebRootPath, objFromDb.BrandLogo.Trim('\\'));

					if (System.IO.File.Exists(oldImgPath))
					{
						System.IO.File.Delete(oldImgPath);
					}
				}

				using (var fileStream = new FileStream(Path.Combine(upload, NewFileName + extension), FileMode.Create))
				{
					file[0].CopyTo(fileStream);
				}
				brand.BrandLogo = @"images\brand" + NewFileName + extension;

			}
			if (ModelState.IsValid)
            {
				var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);
                objFromDb.Name = brand.Name;
                objFromDb.EstablishedYear = brand.EstablishedYear;
                if(brand.BrandLogo != null)
                {
                    objFromDb.BrandLogo = brand.BrandLogo;
                }

				_dbContext.Brand.Update(objFromDb);
                _dbContext.SaveChanges();
                TempData["warning"] = "Record Updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

		[HttpGet]
		public IActionResult Delete(Guid id)
		{
			Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);
			return View(brand);
		}

        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            string WebRootPath = _webHostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(WebRootPath))
            {
                var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                if (objFromDb.BrandLogo != null)
                {
                    var oldImgPath = Path.Combine(WebRootPath, objFromDb.BrandLogo.Trim('\\'));

                    if (System.IO.File.Exists(oldImgPath))
                    {
                        System.IO.File.Delete(oldImgPath);
                    }
                }
                
            }
			_dbContext.Brand.Remove(brand);
			_dbContext.SaveChanges();
			TempData["Delete"] = "Record Deleted successfully";
			return RedirectToAction(nameof(Index));
		}
	}
}
