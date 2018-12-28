using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;


namespace ImageEmbedTool.Controllers
{

    [Produces("application/json")]
    [Route("api/Service")]
    public class ServiceController : Controller
    {
        private string base64String;
        private IHostingEnvironment hostingEnv;
        public ServiceController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }

        // GET: api/Service
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Service/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        public string ImageToBase64(string file)
        {
            //var path = "D:/00hackathon/starterForked/src/Models/download.png";
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(file))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                    GetDataURL(file, base64String);
                    return base64String;
                }
            }
        }
        public static string GetDataURL(string imgFile, string base64String)
        {
            var img = "<img src=\"data:image/"
                        + Path.GetExtension(imgFile).Replace(".", "")
                        + ";base64,"
                        + base64String + "\" />";
            return img;
        }
        public System.Drawing.Image Base64ToImage()
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }

        // POST: api/Service
        [HttpPost]
        public string Post(IList<IFormFile> chunkFile, IList<IFormFile> UploadFiles, [FromServices] INodeServices nodeServices)
        {
            long size = 0;
            try
            {
                // for chunk-upload
                foreach (var file in chunkFile)
                {
                    var filename = ContentDispositionHeaderValue
                                        .Parse(file.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                   
                    filename = hostingEnv.WebRootPath + $@"\{filename}";
                    size += file.Length;

                    if (!System.IO.File.Exists(filename))
                    {
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    else
                    {
                        using (FileStream fs = System.IO.File.Open(filename, FileMode.Append))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }

            // for normal upload
            try
            {
                foreach (var file in UploadFiles)
                {
                    
                    var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName

                                    .Trim('"');
                    var imageName = filename;
                    if (string.IsNullOrWhiteSpace(this.hostingEnv.WebRootPath))
                    {
                        this.hostingEnv.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    }
                    filename = hostingEnv.WebRootPath + $@"\uploadedImages\{filename}";
                    size += file.Length;
                    if (!System.IO.File.Exists(filename))
                    {
                        
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                            nodeServices.InvokeAsync<string>("save.js", imageName, Path.GetExtension(imageName));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
            return "Done";
        }

        // PUT: api/Service/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
