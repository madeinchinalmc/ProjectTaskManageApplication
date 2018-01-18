using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using WorkingTask.Services.Documents;

namespace ProjectTaskManageApplication.Controllers
{
    public class TaskFileController : Controller
    {
        private readonly IDocumentService _documentService;
        public TaskFileController(IDocumentService documentService)
        {
            _documentService = documentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var filePath = Path.GetTempFileName();
            foreach(var formFile in files)
            {
                if(formFile.Length > 0)
                {
                    using(var stream = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size = size, filePath });
        }

    }
}