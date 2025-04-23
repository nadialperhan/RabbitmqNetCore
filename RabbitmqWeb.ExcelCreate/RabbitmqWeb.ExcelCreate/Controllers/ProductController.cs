using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitmqWeb.ExcelCreate.Models;
using RabbitmqWeb.ExcelCreate.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitmqWeb.ExcelCreate.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly RabbitMqPublisher _rabbitMqPublisher;
        public ProductController(UserManager<IdentityUser> userManager, AppDbContext appDbContext, RabbitMqPublisher rabbitMqPublisher)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateProductExcel()
        {
            var user =await _userManager.FindByNameAsync(User.Identity.Name);
            var filename=$"product-excel-{Guid.NewGuid().ToString().Substring(1,10)}";

            UserFile userFile = new UserFile();
            userFile.UserId = user.Id;
            userFile.FileName = filename;
            userFile.FileStatus = FileStatus.Creating;

            await _appDbContext.UserFiles.AddAsync(userFile);
            await _appDbContext.SaveChangesAsync();

            _rabbitMqPublisher.Publish(new Shared.CreateExcelMessage()
            {
                FileId = userFile.Id
                //UserId = userFile.UserId
            });

            TempData["ProductCreatingExcel"] = true;
            return RedirectToAction(nameof(Files));
        }
        public async Task<IActionResult> Files()
        {
            var user =await _userManager.FindByNameAsync(User.Identity.Name);

            return View(await _appDbContext.UserFiles.Where(x => x.UserId == user.Id).ToListAsync());

        }

    }
}
