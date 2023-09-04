using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using REQ001.Data;
using REQ001.Data.Entities;
using REQ001.Helpers;
using REQ001.Models;

namespace REQ001.Controllers
{
    
    public class UserController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;

        public UserController(IUserHelper userHelper, DataContext context, IImageHelper imageHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _imageHelper = imageHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users                
                .ToListAsync());
        }

        public IActionResult Create()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Id = Guid.Empty.ToString(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {

                string path = string.Empty;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Users");
                }

                User user = await _userHelper.AddUserAsync(model, path);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

    }
}
