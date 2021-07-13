using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uploadFiles.Models;

namespace uploadFiles.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
       
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] files, ResultsModel resultModel)
        {
            var ctx = HttpContext.CurrentHandler;
            var root = Server.MapPath("~/App_Data");
            var fileList = "";

            try
            {
                foreach (HttpPostedFileBase eachFile in files)
                {
                    string eachFilename = Path.GetFileName(eachFile.FileName);
                    eachFilename = eachFilename.Trim('"');
                    var localfileName = eachFile.FileName;
                    var newFile = root + "/" + eachFilename;
                    eachFile.SaveAs(newFile);

                }

                string[] fileEntries = Directory.GetFiles(root);

                Process ps = new Process();
                ps.StartInfo.FileName = "cmd.exe";
                ps.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                ps.StartInfo.UseShellExecute = false;
                ps.StartInfo.RedirectStandardOutput = true;
                ps.StartInfo.RedirectStandardError = true;
                ps.StartInfo.Arguments = $@"/c C:\Users\BanziKubheka\Desktop\Debug\OpenFilesTest.exe>C:\Users\BanziKubheka\Desktop\Debug\OpenFilesTestResults.txt --d {root}";
                ps.Start();

                ps.WaitForExit();
                
                string[] outputLines = System.IO.File.ReadAllLines(@"C:\Users\BanziKubheka\Desktop\Debug\OpenFilesTestResults.txt");

                string output = "";

                foreach (string line in outputLines.Skip(2))
                {
                    output = output + $"\n {line}";
                }

                resultModel.outputResults = output;
                
                ViewBag.Message = fileList;
                return View("FilesProcessed", resultModel);
            }
            catch
            {
                ViewBag.Message = "Error: Error occure for uploading a file.";
            }
            return View();
        }

        public FileContentResult GetFile()
        {
            byte[] fileContent = null;
            string fileType = "";
            string file_Name = "";
            
            fileContent = System.IO.File.ReadAllBytes(@"C:\Users\BanziKubheka\Desktop\Debug\OpenFilesTestResults.txt");
            fileType = ".txt";
            file_Name = "OpenFilesTestResults.txt";

            return File(fileContent, fileType, file_Name);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}