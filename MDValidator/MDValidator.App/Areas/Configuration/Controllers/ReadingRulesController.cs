using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MDValidator.App.Areas.Configuration.Models.ReadingRules;
using Microsoft.AspNetCore.Hosting;
using MDValidator.App.Services.Configuration.ReadingRules;
using MDValidator.App.Utilities;
using MDValidator.Domain.Configuration.ReadingRules;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;

namespace MDValidator.App.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    public class ReadingRulesController : Controller
    {
        private IHostingEnvironment _environment;
        private IReadingRulesService _readingRulesSvc;
        private string _readingHeaderSessionKey = "ReadingHeader";
        private string _readingItemsSessionKey = "ReadingItems";

        public ReadingRulesController(IHostingEnvironment environment, IReadingRulesService readingRulesSvc)
        {
            _environment = environment;
            _readingRulesSvc = readingRulesSvc;
        }

        #region HEADER

        public async Task<IActionResult> Header()
        {
            HeadersViewModel vm = new HeadersViewModel();

            var headers = HttpContext.Session.Get<List<Header>>(_readingHeaderSessionKey);

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
            vm = _readingRulesSvc.ProcessHeaderFile(fileName);

            if (vm.Errors.Count > 0)
            {
                HttpContext.Session.Remove(_readingHeaderSessionKey);
            }
            else
            {
                HttpContext.Session.Set<List<Header>>(_readingHeaderSessionKey, vm.Headers);
            }

            System.IO.File.Delete(fileName);

            return View(vm);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveHeader()
        {
            var hIds = HttpContext.Session.Get<List<Header>>(_readingHeaderSessionKey);
            var iIds = HttpContext.Session.Get<List<Item>>(_readingItemsSessionKey);

            List<string> headerIds = new List<string>();
            List<string> itemIds = new List<string>();

            if (hIds != null)
            {
                headerIds = hIds.Select(h => h.ReadId).ToList();
            }

            if (iIds != null)
            {
                itemIds = iIds.Select(i => i.ReadId).ToList();
            }

            bool isConsistent = _readingRulesSvc.CheckHeaderItemsConsistency(headerIds, itemIds);

            if (!isConsistent)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Data inconsistency between Header and Item tables");
            }

            Response.StatusCode = (int)HttpStatusCode.Created;
            return Json("Validation Header saved Successfully");
        }

        #endregion

        #region ITEMS

        public async Task<IActionResult> Items()
        {
            ItemsViewModel vm = new ItemsViewModel();

            var items = HttpContext.Session.Get<List<Item>>(_readingItemsSessionKey);

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
            vm = _readingRulesSvc.ProcessItemsFile(fileName);

            if (vm.Errors.Count > 0)
            {
                HttpContext.Session.Remove(_readingItemsSessionKey);
            }
            else
            {
                HttpContext.Session.Set<List<Item>>(_readingItemsSessionKey, vm.Items);
            }

            System.IO.File.Delete(fileName);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SaveItems()
        {
            var hIds = HttpContext.Session.Get<List<Header>>(_readingHeaderSessionKey);
            var iIds = HttpContext.Session.Get<List<Item>>(_readingItemsSessionKey);

            List<string> headerIds = new List<string>();
            List<string> itemIds = new List<string>();

            if (hIds != null)
            {
                headerIds = hIds.Select(h => h.ReadId).ToList();
            }

            if (iIds != null)
            {
                itemIds = iIds.Select(i => i.ReadId).ToList();
            }

            bool isConsistent = _readingRulesSvc.CheckHeaderItemsConsistency(headerIds, itemIds);

            if (!isConsistent)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Data inconsistency between Header and Item tables");
            }

            Response.StatusCode = (int)HttpStatusCode.Created;
            return Json("Reading Items saved successfully");
        }

        #endregion
    }
}