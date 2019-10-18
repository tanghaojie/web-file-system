using Abp.Domain.Repositories;
using Abp.UI;
using Abp.Web.Models;
using Abp.WebApi.Controllers;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebFileSystem.Swagger;

namespace WebFileSystem.File {
    public class FileApiController : AbpApiController {
        private readonly IRepository<Entities.EF.File, long> _fileRepository;
        public FileApiController(
           IRepository<Entities.EF.File, long> fileRepository
            )
        {
            _fileRepository = fileRepository;
        }

        public class MultipartFormDataFileStreamProvider : MultipartFormDataStreamProvider {
            private Func<string, string> _filenameGenerate = null;
            public MultipartFormDataFileStreamProvider(string path, Func<string, string> filenameGenerate) : base(path)
            {
                _filenameGenerate = filenameGenerate;
            }
            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                return (_filenameGenerate?.Invoke(headers.ContentDisposition.FileName)) ?? DateTime.Now.ToFileTime().ToString();
            }
        }

        [
            HttpPost,
            Route("Upload"),
            WrapResult,
            SwaggerFormDataMethod(
                new bool[] { true, false, false, false },
                new string[] { "file", "accessSymbolic", "owner", "group", "description" },
                new string[] { "上传文件", "权限", "所有者", "组", "描述" },
                new SwaggerFormDataMethodAttributeType[] { SwaggerFormDataMethodAttributeType.File, SwaggerFormDataMethodAttributeType.String, SwaggerFormDataMethodAttributeType.String, SwaggerFormDataMethodAttributeType.String, SwaggerFormDataMethodAttributeType.String }
            )]
        public async Task<long> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent()) { throw new UserFriendlyException("不支持的请求类型"); }
            var httpPostedFiles = HttpContext.Current.Request.Files;
            if (httpPostedFiles == null || httpPostedFiles.Count != 1) { throw new UserFriendlyException("服务器必须接收一个文件"); }

            var now = DateTime.Now;
            var nowHour = now.Year.ToString("0000") + now.Month.ToString("00") + now.Day.ToString("00") + now.Hour.ToString("00");
            string dirPath = $"{HttpContext.Current.Server.MapPath($"~/App_Data/{nowHour}")}";
            if (!Directory.Exists(dirPath)) { Directory.CreateDirectory(dirPath); }

            var filename = string.Empty;
            var ext = string.Empty;
            var provider = new MultipartFormDataFileStreamProvider(dirPath, originFilename =>
            {
                filename = !string.IsNullOrWhiteSpace(originFilename) ? originFilename : DateTime.Now.ToFileTime().ToString();
                filename = filename.Replace("\"", string.Empty);
                ext = Path.GetExtension(filename) ?? "";
                return DateTime.Now.ToFileTime().ToString() + ext;
            });

            long length = -1;
            var contentType = string.Empty;
            var localFilePath = string.Empty;
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider).ContinueWith(
                    p =>
                    {
                        if (p.IsFaulted || p.IsCanceled)
                        {
                            throw new UserFriendlyException("接收文件失败");
                        }
                        var fileData = p.Result.FileData[0];
                        contentType = fileData.Headers.ContentType.MediaType;
                        localFilePath = fileData.LocalFileName;
                        if (System.IO.File.Exists(localFilePath))
                        {
                            length = new FileInfo(localFilePath).Length;
                        }
                    });
            }
            catch
            {
                throw new UserFriendlyException("接收文件失败");
            }

            var accessSymbolic = string.Empty;
            var owner = string.Empty;
            var group = string.Empty;
            var description = string.Empty;

            var formData = provider.FormData;
            if (formData.HasKeys())
            {
                var allKeys = formData.AllKeys;
                var keyName = "accessSymbolic";
                if (allKeys.Contains(keyName) && !string.IsNullOrEmpty(formData[keyName]))
                {
                    accessSymbolic = formData[keyName];
                }
                keyName = "owner";
                if (allKeys.Contains(keyName) && !string.IsNullOrEmpty(formData[keyName]))
                {
                    owner = formData[keyName];
                }
                keyName = "group";
                if (allKeys.Contains(keyName) && !string.IsNullOrEmpty(formData[keyName]))
                {
                    group = formData[keyName];
                }
                keyName = "description";
                if (allKeys.Contains(keyName) && !string.IsNullOrEmpty(formData[keyName]))
                {
                    description = formData[keyName];
                }
            }

            long id = -1;
            try
            {
                var entity = new Entities.EF.File
                {
                    ContentType = contentType.ToLower(),
                    Description = description,
                    Filename = filename,
                    // Group = 
                    Length = length,
                    LocalFilePath = localFilePath,
                    Owner = owner,
                };
                if (!string.IsNullOrEmpty(accessSymbolic)) { entity.AccessSymbolic = accessSymbolic; }
                if (!string.IsNullOrEmpty(ext)) { entity.Extension = ext; }

                id = _fileRepository.InsertAndGetId(entity);
            }
            catch
            {
                try { System.IO.File.Delete(localFilePath); } catch { }
                throw new UserFriendlyException("接收文件失败");
            }
            return id;
        }

        [HttpGet, Route("Download/{id}")]
        public HttpResponseMessage Download(int id)
        {
            try
            {
                var entity = _fileRepository.FirstOrDefault(id);
                if (entity == null) { return new HttpResponseMessage(HttpStatusCode.NotFound); }

                string localFilePath = entity.LocalFilePath;
                if (!System.IO.File.Exists(localFilePath))
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                var fileStream = new MemoryStream(System.IO.File.ReadAllBytes(localFilePath));
                var res = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(fileStream),
                };
                res.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(localFilePath));
                res.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = entity.Filename
                };
                return res;
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}