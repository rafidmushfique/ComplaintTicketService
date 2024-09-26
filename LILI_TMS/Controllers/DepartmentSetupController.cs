using LILI_TMS;
using LILI_TMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LILI_TTS.Controllers
{
    [Authorize]
    public class DepartmentSetupController : Controller
    {
        private readonly dbTicketManagementContext _context;

        public DepartmentSetupController(dbTicketManagementContext context)
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
            var employees = from s in _context.TblEmployee
                            select s;
            IQueryable<TblDepartment> model = _context.TblDepartments ;


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.DepartmentName.Contains(searchString) || s.DepartmentCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.DepartmentCode);
                    break;

                default:
                    model = model.OrderBy(s => s.Idate);
                    break;
            }
            int pageSize = 7;
            return View(await PaginatedList<TblDepartment>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {

            TblDepartment model = new TblDepartment();

            var departmentTypeList = (from c in _context.TblDepartmentTypes
                            select new
                            {
                                TypeCode = c.TypeCode,
                                TypeName = c.TypeName
                            }
            ).ToList();

            departmentTypeList.Insert(0, new { TypeCode = "", TypeName = "<Select Department Type>" });
            ViewBag.ListOfDepartmentType = departmentTypeList;


            model.DepartmentCode = GenerateDepartmentCode();
            TempData["msg"] = "";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDepartmentSetup(TblDepartment model)
        {
            try
            {
               //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    if (DoesToolCodeExists(model.DepartmentCode))
                    {
                        model.DepartmentCode = GenerateDepartmentCode().ToString();
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
            List<TblDepartmentType> departmentList = new List<TblDepartmentType>();
            departmentList = _context.TblDepartmentTypes.ToList();
            ViewBag.ListOfDepartmentType = departmentList;

            TblDepartment departmentModel = new TblDepartment();
            var result = _context.TblDepartments.Where(s=>s.Id== vId).First();
            departmentModel.Id = result.Id;
            departmentModel.TypeCode = result.TypeCode;
            departmentModel.DepartmentCode = result.DepartmentCode;
            departmentModel.DepartmentName = result.DepartmentName;
            departmentModel.Comments = result.Comments;

         return View(departmentModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDepartmentSetup(TblDepartment model) {
            var vId=   model.Id;
            try
            {
                var departmentSetupToUpdate = await _context.TblDepartments.FirstOrDefaultAsync(s=>s.Id==vId);
                departmentSetupToUpdate.Edate= DateTime.Now;
                departmentSetupToUpdate.Euser= User.Identity.Name;
                if (await TryUpdateModelAsync<TblDepartment>(
                    departmentSetupToUpdate,
                    "",
                    s => s.TypeCode,
                    s => s.DepartmentCode,
                    s => s.DepartmentName,
                    s => s.Comments
                    
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
                TblDepartment model= _context.TblDepartments.Where(s=> s.Id == vId).First();
                if (model != null)
                {
                    _context.TblDepartments.Remove(model);
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
        private  string GenerateDepartmentCode()
        {
         
           // var yearMonth = DateTime.Now.ToString("yyyyMM");
            var result =  _context.TblDepartments.OrderBy(x => x.Id).Select(x=>x.DepartmentCode).LastOrDefault();
            var lastGrn = string.IsNullOrEmpty(result) ? "00000" : result;


            var last5digits = "1";
            if (lastGrn.Length > 3)
            {
                last5digits = lastGrn.Substring(lastGrn.Length - 3);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D3");
            //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
            var generatedCode = $"D-{lastNumberString}";
            return generatedCode;
        }
        public bool DoesToolCodeExists(string vToolCode)
        {

            return _context.TblDepartments.Any(e => e.DepartmentCode == vToolCode);
        }
        #endregion
    }
}
