using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MDValidator.App.Areas.Configuration.Models.ValidationRules;
using Microsoft.AspNetCore.Hosting;
using MDValidator.App.Services.Configuration.ValidationRules;
using MDValidator.Domain.Configuration.ValidationRules;
using MDValidator.App.Utilities;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;

namespace MDValidator.App.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    public class ValidationRulesController : Controller
    {
        private IHostingEnvironment _environment;
        private IValidationRulesService _validationRulesSvc;
        private string _validationHeaderSessionKey = "ValidationHeader";
        private string _validationItemsSessionKey = "ValidationItems";

        public ValidationRulesController(IHostingEnvironment environment, IValidationRulesService validationRulesSvc)
        {
            _environment = environment;
            _validationRulesSvc = validationRulesSvc;
        }


        #region HEADER

        public async Task<IActionResult> Header()
        {
            HeadersViewModel vm = new HeadersViewModel();

            var headers = HttpContext.Session.Get<List<Header>>(_validationHeaderSessionKey);

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
            vm = _validationRulesSvc.ProcessHeaderFile(fileName);

            if (vm.Errors.Count > 0)
            {
                HttpContext.Session.Remove(_validationHeaderSessionKey);
            }
            else
            {
                HttpContext.Session.Set<List<Header>>(_validationHeaderSessionKey, vm.Headers);
            }

            System.IO.File.Delete(fileName);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SaveHeader()
        {
            var hIds = HttpContext.Session.Get<List<Header>>(_validationHeaderSessionKey);
            var iIds = HttpContext.Session.Get<List<Item>>(_validationItemsSessionKey);

            List<string> headerIds = new List<string>();
            List<string> itemIds = new List<string>();

            if (hIds != null)
            {
                headerIds = hIds.Select(h => h.ValId).ToList();
            }

            if (iIds != null)
            {
                itemIds = iIds.Select(i => i.ValId).ToList();
            }

            bool isConsistent = _validationRulesSvc.CheckHeaderItemsConsistency(headerIds, itemIds);

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

            var items = HttpContext.Session.Get<List<Item>>(_validationItemsSessionKey);

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
            vm = _validationRulesSvc.ProcessItemsFile(fileName);

            if (vm.Errors.Count > 0)
            {
                HttpContext.Session.Remove(_validationItemsSessionKey);
            }
            else
            {
                HttpContext.Session.Set<List<Item>>(_validationItemsSessionKey, vm.Items);
            }

            System.IO.File.Delete(fileName);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SaveItems()
        {
            var hIds = HttpContext.Session.Get<List<Header>>(_validationHeaderSessionKey);
            var iIds = HttpContext.Session.Get<List<Item>>(_validationItemsSessionKey);

            List<string> headerIds = new List<string>();
            List<string> itemIds = new List<string>();

            if (hIds != null)
            {
                headerIds = hIds.Select(h => h.ValId).ToList();
            }

            if (iIds != null)
            {
                itemIds = iIds.Select(i => i.ValId).ToList();
            }

            bool isConsistent = _validationRulesSvc.CheckHeaderItemsConsistency(headerIds, itemIds);

            if (!isConsistent)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Data inconsistency between Header and Item tables");
            }

            Response.StatusCode = (int)HttpStatusCode.Created;
            return Json("Validation Items saved Successfully");
        }

        #endregion
    }
}