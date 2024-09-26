using LILI_TMS;
using LILI_TMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace LILI_TTS.Controllers
{
    [Authorize]
    public class BusinessSetupController : Controller
    {
        private readonly dbTicketManagementContext _context;

        public BusinessSetupController(dbTicketManagementContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            IQueryable<TblBusinessSetupInfo> model = _context.TblBusinessSetupInfos;


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.BusinessName.Contains(searchString) || s.BusinessCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.BusinessCode);
                    break;

                default:
                    model = model.OrderBy(s => s.Idate);
                    break;
            }
            int pageSize = 7;
            return View(await PaginatedList<TblBusinessSetupInfo>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {

            TblBusinessSetupInfo model = new TblBusinessSetupInfo();
            //var newGeneratedCode = GenerateToolCode();
            model.BusinessCode = GenerateBusinessCode();
            TempData["msg"] = "";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBusinessSetup(TblBusinessSetupInfo model)
        {
            try
            {
               //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    if (DoesToolCodeExists(model.BusinessCode))
                    {
                        model.BusinessCode = GenerateBusinessCode().ToString();
                    }
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;
                    _context.Add(model);
                    await _context.SaveChangesAsync();

                   
                }
                else 
                {
                    TempData["msg"] = "Data Save Unsuccessful";
                    return View("Create", model);
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error Occured : "+ex.Message;
                return View("Create", model);

            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Update(int vId) {
            TblBusinessSetupInfo businessModel = new TblBusinessSetupInfo();
            var result = _context.TblBusinessSetupInfos.Where(s=>s.Id== vId).First();
            businessModel.Id = result.Id;
            businessModel.BusinessCode = result.BusinessCode;
            businessModel.BusinessName = result.BusinessName;
            businessModel.Address = result.Address;
            businessModel.Description = result.Description;


         return View(businessModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBusinessSetup(TblBusinessSetupInfo model) {
            var vId=   model.Id;
            try
            {
                var toolSetupToUpdate = await _context.TblBusinessSetupInfos.FirstOrDefaultAsync(s=>s.Id==vId);
                toolSetupToUpdate.Edate= DateTime.Now;
                toolSetupToUpdate.Euser= User.Identity.Name;
                if (await TryUpdateModelAsync<TblBusinessSetupInfo>(
                    toolSetupToUpdate,
                    "",
                    s => s.BusinessName,
                    s => s.Address,
                    s => s.Description
                    
                    )) ;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return View(model);
        }
        public bool Delete(int vId)
        {
            try
            {
                TblBusinessSetupInfo model= _context.TblBusinessSetupInfos.Where(s=> s.Id == vId).First();
                if (model != null)
                {
                    _context.TblBusinessSetupInfos.Remove(model);
                    _context.SaveChanges();
                    return true;
                }
                else 
                {
                   
                    return false;
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error Occurred while trying to Delete data.";
                return false;
            }
        }

        #region private classes
        private  string GenerateBusinessCode()
        {
         
           // var yearMonth = DateTime.Now.ToString("yyyyMM");
            var result =  _context.TblBusinessSetupInfos.OrderBy(x => x.Id).Select(x=>x.BusinessCode).LastOrDefault();
            var lastGrn = string.IsNullOrEmpty(result) ? "00000" : result;


            var last5digits = "1";
            if (lastGrn.Length > 3)
            {
                last5digits = lastGrn.Substring(lastGrn.Length - 3);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D3");
            //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
            var generatedCode = $"B-{lastNumberString}";
            return generatedCode;
        }
        public bool DoesToolCodeExists(string vToolCode)
        {

            return _context.TblBusinessSetupInfos.Any(e => e.BusinessCode == vToolCode);
        }
        #endregion
    }
}
