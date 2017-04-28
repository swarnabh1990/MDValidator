using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MDValidator.App.Areas.Configuration.Models.ExtractionRules;
using Microsoft.AspNetCore.Hosting;
using MDValidator.App.Services.Configuration.ExtractionRules;
using MDValidator.App.Utilities;
using MDValidator.Domain.Configuration.ExtractionRules;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;

namespace MDValidator.App.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    public class ExtractionRulesController : Controller
    {
        private IHostingEnvironment _environment;
        private IExtractionRulesService _extractionRulesSvc;
        private string _extractionHeaderSessionKey = "ExtractionHeader";
        private string _extractionItemsSessionKey = "ExtractionItems";

        public ExtractionRulesController(IHostingEnvironment environment, IExtractionRulesService extractionRulesSvc)
        {
            _environment = environment;
            _extractionRulesSvc = extractionRulesSvc;
        }

        #region HEADER

        public async Task<IActionResult> Header()
        {
            HeadersViewModel vm = new HeadersViewModel();

            var headers = HttpContext.Session.Get<List<Header>>(_extractionHeaderSessionKey);

            if (headers != null)
            {
                vm.Headers = headers;
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Header(ICollection<IFormFile> files)
        {
            HeadersViewModel vm = new HeadersViewModel();

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
            vm = _extractionRulesSvc.ProcessHeaderFile(fileName);

            if (vm.Errors.Count > 0)
            {
                HttpContext.Session.Remove(_extractionHeaderSessionKey);
            }
            else
            {
                HttpContext.Session.Set<List<Header>>(_extractionHeaderSessionKey, vm.Headers);
            }

            System.IO.File.Delete(fileName);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SaveHeader()
        {
            var hIds = HttpContext.Session.Get<List<Header>>(_extractionHeaderSessionKey);
            var iIds = HttpContext.Session.Get<List<Item>>(_extractionItemsSessionKey);

            List<string> headerIds = new List<string>();
            List<string> itemIds = new List<string>();

            if (hIds != null)
            {
                headerIds = hIds.Select(h => h.ExtrId).ToList();
            }

            if (iIds != null)
            {
                itemIds = iIds.Select(i => i.ExtrId).ToList();
            }

            bool isConsistent = _extractionRulesSvc.CheckHeaderItemsConsistency(headerIds, itemIds);

            if (!isConsistent)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Data inconsistency between Header and Item tables");
            }

            Response.StatusCode = (int)HttpStatusCode.Created;
            return Json("Ectraction Header saved Successfully");
        }

        #endregion

        #region ITEMS

        public async Task<IActionResult> Items()
        {
            ItemsViewModel vm = new ItemsViewModel();

            var items = HttpContext.Session.Get<List<Item>>(_extractionItemsSessionKey);

            if (items != null)
            {
                vm.Items = items;
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Items(ICollection<IFormFile> files)
        {
            ItemsViewModel vm = new ItemsViewModel();

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
            vm = _extractionRulesSvc.ProcessItemsFile(fileName);

            if (vm.Errors.Count > 0)
            {
                HttpContext.Session.Remove(_extractionItemsSessionKey);
            }
            else
            {
                HttpContext.Session.Set<List<Item>>(_extractionItemsSessionKey, vm.Items);
            }

            System.IO.File.Delete(fileName);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SaveItems()
        {
            var hIds = HttpContext.Session.Get<List<Header>>(_extractionHeaderSessionKey);
            var iIds = HttpContext.Session.Get<List<Item>>(_extractionItemsSessionKey);

            List<string> headerIds = new List<string>();
            List<string> itemIds = new List<string>();

            if (hIds != null)
            {
                headerIds = hIds.Select(h => h.ExtrId).ToList();
            }

            if (iIds != null)
            {
                itemIds = iIds.Select(i => i.ExtrId).ToList();
            }

            bool isConsistent = _extractionRulesSvc.CheckHeaderItemsConsistency(headerIds, itemIds);

            if (!isConsistent)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Data inconsistency between Header and Item tables");
            }

            Response.StatusCode = (int)HttpStatusCode.Created;
            return Json("Extraction Items saved Successfully");
        }

        #endregion
    }
}