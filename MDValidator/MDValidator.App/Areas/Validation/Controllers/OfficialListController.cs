using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MDValidator.App.Services.Validation;
using MDValidator.Domain.Validation;
using MDValidator.App.Utilities;
using MDValidator.App.Areas.Validation.Models.Validation;
using MDValidator.Domain.Configuration.LaneMaster;

namespace MDValidator.App.Areas.Validation.Controllers
{
    [Area("Validation")]
    public class OfficialListController : Controller
    {
        private IHostingEnvironment _environment;
        private IOfficialListService _officialListSvc;
        private string _officialListSessionKey = "OfficialList";

        public OfficialListController(IHostingEnvironment environment, IOfficialListService officialListSvc)
        {
            _environment = environment;
            _officialListSvc = officialListSvc;
        }


        public IActionResult Index()
        {
            OfficialListViewModel vm = new OfficialListViewModel();

            var officialList = HttpContext.Session.Get<OfficialList>(_officialListSessionKey);

            if (officialList != null)
            {
                vm.OfficialList = officialList;
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ICollection<IFormFile> files)
        {
            OfficialListViewModel vm = new OfficialListViewModel();

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
            vm = _officialListSvc.ProcessFile(fileName, HttpContext.Session.Get<List<LaneMasterRecord>>("LaneMaster"));

            if (vm.Errors.Count > 0)
            {
                HttpContext.Session.Remove(_officialListSessionKey);
            }
            else
            {
                HttpContext.Session.Set<OfficialList>(_officialListSessionKey, vm.OfficialList);
            }

            System.IO.File.Delete(fileName);

            return View(vm);
        }
    }
}