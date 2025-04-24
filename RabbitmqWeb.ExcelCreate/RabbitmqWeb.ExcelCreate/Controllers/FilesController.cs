using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RabbitmqWeb.ExcelCreate.Models;
using RabbitmqWeb.ExcelCreate.SignalR;
using System.IO;
using System.Threading.Tasks;

namespace RabbitmqWeb.ExcelCreate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHubContext<MyHub> _hubContext;

        public FilesController(AppDbContext appDbContext, IHubContext<MyHub> hubContext)
        {
            _appDbContext = appDbContext;
            _hubContext = hubContext;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int fileid)
        {
            if (file is not { Length: > 0 })
            {
                return BadRequest();
            }

            var userfile = await _appDbContext.UserFiles.FirstAsync(x => x.Id == fileid);
            var filepath = userfile.FileName + Path.GetExtension(file.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", filepath);

            using FileStream stream = new(path, FileMode.Create);
            await file.CopyToAsync(stream);

            userfile.CreatedDate = System.DateTime.Now;
            userfile.FilePath = filepath;
            userfile.FileStatus = FileStatus.Completed;

            await _appDbContext.SaveChangesAsync();
            await _hubContext.Clients.User(userfile.UserId).SendAsync("CompletedFile");
            return Ok();

        }
    }
}
