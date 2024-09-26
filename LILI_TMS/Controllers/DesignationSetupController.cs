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
    public class DesignationSetupController : Controller
    {
        private readonly dbTicketManagementContext _context;

        public DesignationSetupController(dbTicketManagementContext context)
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
            //var employees = from s in _context.TblEmployee
            //                select s;

            var employeesList = (
                from s in _context.TblDesignations
                join d in _context.TblDepartments on s.DepartmentCode equals d.DepartmentCode
                select new TblDesignation
                {
                    Id = s.Id,
                    DesignationCode = s.DesignationCode,
                    DesignationName = s.DesignationName,
                    DepartmentCode = s.DepartmentCode,
                    DepartmentName =d.DepartmentName,
                    Comments = s.Comments,
                    Idate=s.Idate

                }


                );


           // IQueryable <TblDesignation> model = _context.TblDesignations ;


            if (!String.IsNullOrEmpty(searchString))
            {
                //model = model.Where(s => s.DesignationName.Contains(searchString) || s.DesignationCode.Contains(searchString));

                employeesList = (from s in employeesList
                                 where (s.DesignationName.Contains(searchString) || s.DesignationCode.Contains(searchString))
                                   select new TblDesignation
                                   {
                                       Id = s.Id,
                                       DesignationCode = s.DepartmentCode,
                                       DesignationName = s.DesignationName,
                                       DepartmentCode = s.DepartmentCode,
                                       DepartmentName = s.DepartmentName,
                                       Comments = s.Comments,
                                       Idate=s.Idate
                                   });
            }

            switch (sortOrder)
            {
                case "name_desc":
                    employeesList = employeesList.OrderByDescending(s => s.DesignationCode);
                    break;

                default:
                    employeesList = employeesList.OrderBy(s => s.Idate);
                    break;
            }
            int pageSize = 7;

            return View(await PaginatedList<TblDesignation>.CreateAsync(employeesList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {

            TblDesignation model = new TblDesignation();

            var departmentList = (from c in _context.TblDepartments
                            select new
                            {
                                DepartmentCode = c.DepartmentCode,
                                DepartmentName = c.DepartmentName
                            }
            ).ToList();

            departmentList.Insert(0, new { DepartmentCode = "", DepartmentName = "<Select Department>" });
            ViewBag.ListOfDepartment = departmentList;


            model.DesignationCode = GenerateDesignationCode();
            TempData["msg"] = "";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDesignationSetup(TblDesignation model)
        {
            try
            {
               //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    if (DoesToolCodeExists(model.DepartmentCode))
                    {
                        model.DepartmentCode = GenerateDesignationCode().ToString();
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
            List<TblDepartment> departmentList = new List<TblDepartment>();
            departmentList = _context.TblDepartments.ToList();
            ViewBag.ListOfDepartment = departmentList;

            TblDesignation designationModel = new TblDesignation();
            var result = _context.TblDesignations.Where(s=>s.Id== vId).First();
            designationModel.Id = result.Id;
            designationModel.DesignationCode = result.DesignationCode;
            designationModel.DesignationName = result.DesignationName;
            designationModel.DepartmentCode = result.DepartmentCode;
            designationModel.Comments = result.Comments;

         return View(designationModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDesignationSetup(TblDesignation model) {
            var vId=   model.Id;
            try
            {
                var departmentSetupToUpdate = await _context.TblDesignations.FirstOrDefaultAsync(s=>s.Id==vId);
                departmentSetupToUpdate.Edate= DateTime.Now;
                departmentSetupToUpdate.Euser= User.Identity.Name;
                if (await TryUpdateModelAsync<TblDesignation>(
                    departmentSetupToUpdate,
                    "",
                    s => s.DesignationCode,
                    s => s.DesignationName,
                    s => s.DepartmentCode,
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
                TblDesignation model= _context.TblDesignations.Where(s=> s.Id == vId).First();
                if (model != null)
                {
                    _context.TblDesignations.Remove(model);
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
        private  string GenerateDesignationCode()
        {
         
           // var yearMonth = DateTime.Now.ToString("yyyyMM");
            var result =  _context.TblDesignations.OrderBy(x => x.Id).Select(x=>x.DesignationCode).LastOrDefault();
            var lastGrn = string.IsNullOrEmpty(result) ? "000000" : result;


            var last5digits = "1";
            if (lastGrn.Length > 3)
            {
                last5digits = lastGrn.Substring(lastGrn.Length - 3);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D3");
            //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
            var generatedCode = $"DG-{lastNumberString}";
            return generatedCode;
        }
        public bool DoesToolCodeExists(string vToolCode)
        {

            return _context.TblDesignations.Any(e => e.DesignationCode == vToolCode);
        }
        #endregion
    }
}
