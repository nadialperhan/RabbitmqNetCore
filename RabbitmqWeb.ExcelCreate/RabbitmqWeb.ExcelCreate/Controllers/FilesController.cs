using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitmqWeb.ExcelCreate.Models;
using System.IO;
using System.Threading.Tasks;

namespace RabbitmqWeb.ExcelCreate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public FilesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
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

            return Ok();

        }
    }
}
