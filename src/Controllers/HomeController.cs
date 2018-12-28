using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImageEmbedTool.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ImageEmbedTool.Controllers
{
    public class HomeController : Controller
    {

        private string base64String;
        private IHostingEnvironment hostingEnv;
        public HomeController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }

        public string ImageToBase64(string file)
        {
            //var path = "D:/00hackathon/starterForked/src/Models/download.png";
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(this.hostingEnv.WebRootPath + "\\uploadedImages\\" + file))
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Gallery()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact(string name)
        {

            ViewData["Message"] = "Your contact page.";
            ViewData["Base64"] = this.ImageToBase64(name);
            ViewData["Url"] = Url.Content("~/uploadedImages/" + name);

            return PartialView();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
