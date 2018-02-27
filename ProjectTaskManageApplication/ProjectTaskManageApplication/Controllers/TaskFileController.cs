using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using WorkingTask.Services.Documents;
using Microsoft.Net.Http.Headers;
using ProjectTaskManageApplication.Filter;
using ProjectTaskManageApplication.Helper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http.Features;
using System.Text;
using ProjectTaskManageApplication.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace ProjectTaskManageApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskFileController : Controller
    {
        private readonly IDocumentService _documentService;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        public TaskFileController(IDocumentService documentService)
        {
            _documentService = documentService;
        }
        [HttpGet("Files/{fileId}")]
        //[GenerateAntiforgeryTokenCookieForAjax]
        public IActionResult Index(string fileId)
        {
            string s = fileId;

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
            var filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + @"uploads\UserDownLoadFile\";//Path.GetTempFileName();
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            foreach (var formFile in files)
            {
                var filename = ContentDispositionHeaderValue
                           .Parse(formFile.ContentDisposition)
                           .FileName
                           .Value.Trim('"');
                if (formFile.Length > 0)
                {
                    using (var stream = System.IO.File.Create($"{filePath}{filename}"))
                    {
                        await formFile.CopyToAsync(stream);
                        stream.Flush();
                    }
                }
            }
            return Ok(new { count = files.Count, size = size, filePath });
        }
        [HttpPost]
        [DisableFormValueModelBinding]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpLoad()
        {
            if(!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }
            // Used to accumulate all the form url encoded key value pairs in the 
            // request.
            var formAccumulator = new KeyValueAccumulator();
            string targetFilePath = null;
            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body); //读取表单内容的对象
            var section = await reader.ReadNextSectionAsync();     //读取
            while(section!=null)
            {
                ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);
                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        targetFilePath = Path.GetTempFileName();  //换下路径
                        using (var targetStream = System.IO.File.Create(targetFilePath))
                        {
                            await section.Body.CopyToAsync(targetStream);
;
                        }
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                        var encoding = GetEncoding(section);
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            var value = await streamReader.ReadToEndAsync();
                            if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = String.Empty;
                            }
                            formAccumulator.Append(key.Value.Trim('"'), value);
                            if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
                            {
                                throw new InvalidDataException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }
                section = await reader.ReadNextSectionAsync();

            }
            var user = new User();
            var formValueProvider = new FormValueProvider(
                BindingSource.Form,
                new FormCollection(formAccumulator.GetResults()),
                CultureInfo.CurrentCulture);
            var bindingSuccessful = await TryUpdateModelAsync(user, prefix: "",
                valueProvider: formValueProvider);
            if (!bindingSuccessful)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }
            var uploadedData = new UploadedData()
            {
                Name = user.Name,
                Age = user.Age,
                Zipcode = user.Zipcode,
                FilePath = targetFilePath
            };
            return Json(uploadedData);
        }
        private static Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }
    }
}