using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MDValidator.App.Areas.Configuration.Models.LaneMaster;
using Microsoft.AspNetCore.Hosting;
using MDValidator.App.Services.Configuration.LaneMaster;
using MDValidator.Domain.Configuration.LaneMaster;
using MDValidator.App.Utilities;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace MDValidator.App.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    public class LaneMasterController : Controller
    {
        private IHostingEnvironment _environment;
        private ILaneMasterService _laneMasterSvc;
        private string _laneMasterSessionKey = "LaneMaster";

        public LaneMasterController(IHostingEnvironment environment, ILaneMasterService laneMasterSvc)
        {
            _environment = environment;
            _laneMasterSvc = laneMasterSvc;
        }

        public async Task<IActionResult> Index()
        {
            LaneMasterViewModel vm = new LaneMasterViewModel();

            var laneMaster = HttpContext.Session.Get<List<LaneMasterRecord>>(_laneMasterSessionKey);

            if (laneMaster != null)
            {
                vm.LaneMaster = laneMaster;
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ICollection<IFormFile> files)
        {
            LaneMasterViewModel vm = new LaneMasterViewModel();

            var uploads = Path.Combine(_environment.WebRootPath, "uploads");

            if (files.Count < 1)
            {
                return View(vm);
            }

            var file = files.First();

            string fileName = Path.Combine(uploads, file.FileName);

            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            //validate file
            vm = _laneMasterSvc.ProcessFile(fileName);

            if (vm.Errors.Count > 0)
            {
                HttpContext.Session.Remove(_laneMasterSessionKey);
            }
            else
            {
                HttpContext.Session.Set<List<LaneMasterRecord>>(_laneMasterSessionKey, vm.LaneMaster);
            }

            System.IO.File.Delete(fileName);

            return View(vm);
        }
    }
}