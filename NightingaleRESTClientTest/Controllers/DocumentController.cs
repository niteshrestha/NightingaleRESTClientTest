using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentParser.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        #region Fields
        private readonly IWebHostEnvironment _environment;
        #endregion

        #region Constructors
        public DocumentController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        #endregion

        #region Controllers
        [HttpPost("Parse")]
        public async Task<IActionResult> Parse(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string documentDirectory = Path.Combine(_environment.WebRootPath, "documents");
                if (!Directory.Exists(documentDirectory))
                {
                    Directory.CreateDirectory(documentDirectory);
                }
                string documentName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                string documentPath = Path.Combine(documentDirectory, documentName);
                using (FileStream fs = new FileStream(documentPath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
                return Ok(new { Message = $"Success at {documentPath}" });
            }
            else
            {
                return BadRequest(new { message = "No files were provided" });
            }
        }
        #endregion
    }
}
