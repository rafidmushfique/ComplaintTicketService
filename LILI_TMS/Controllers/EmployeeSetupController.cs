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
    public class EmployeeSetupController : Controller
    {
        private readonly dbTicketManagementContext _context;

        public EmployeeSetupController(dbTicketManagementContext context)
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
                from s in _context.TblEmployeeSetups
                join d in _context.TblDepartments on s.DepartmentCode equals d.DepartmentCode
                join des in _context.TblDesignations on s.DesignationCode equals des.DesignationCode
                select new TblEmployeeSetup
                {
                    Id = s.Id,
                    EmployeeCode = s.EmployeeCode,
                    EmployeeName = s.EmployeeName,
                    DepartmentName = d.DepartmentName,
                    DesignationName = des.DesignationName,
                    Comments = s.Comments,
                    Idate=s.Idate

                }


                );


           // IQueryable <TblDesignation> model = _context.TblDesignations ;


            if (!String.IsNullOrEmpty(searchString))
            {
                //model = model.Where(s => s.DesignationName.Contains(searchString) || s.DesignationCode.Contains(searchString));

                employeesList = (from s in employeesList
                                 where (s.EmployeeCode.Contains(searchString) || s.EmployeeName.Contains(searchString))
                                   select new TblEmployeeSetup
                                   {
                                       Id = s.Id,
                                       EmployeeCode = s.EmployeeCode,
                                       EmployeeName = s.EmployeeName,
                                       DepartmentName = s.DepartmentName,
                                       DesignationName = s.DesignationName,
                                       Comments = s.Comments,
                                       Idate = s.Idate
                                   });
            }

            switch (sortOrder)
            {
                case "name_desc":
                    employeesList = employeesList.OrderByDescending(s => s.DesignationCode);
                    break;

                default:
                    employeesList = employeesList.OrderByDescending(s => s.EmployeeCode);
                    break;
            }
            int pageSize = 7;

            return View(await PaginatedList<TblEmployeeSetup>.CreateAsync(employeesList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {
            var unMappedUserList = (from c in _context.AspNetUsers
                                   where !_context.TblUserWiseEmployeeMappings.Any(u => u.UserId == c.Email)
                                   select new
                                   { 
                                       UserId=c.Email,
                                       UserName= c.Email,
                                   }).ToList();
            unMappedUserList.Insert(0, new  { UserId = "", UserName = "<Select User Email>" });
            ViewBag.UnMappedUserList = unMappedUserList;

            TblEmployeeSetup model = new TblEmployeeSetup();

            var departmentList = (from c in _context.TblDepartments
                            select new
                            {
                                DepartmentCode = c.DepartmentCode,
                                DepartmentName = c.DepartmentName
                            }
            ).ToList();

            departmentList.Insert(0, new { DepartmentCode = "", DepartmentName = "<Select Department>" });
            ViewBag.ListOfDepartment = departmentList;

            var designationList = (from c in _context.TblDesignations
                                  select new
                                  {
                                      DesignationCode = c.DesignationCode,
                                      DesignationName = c.DesignationName
                                  }
            ).ToList();

            designationList.Insert(0, new { DesignationCode = "", DesignationName = "<Select Designation>" });
            ViewBag.ListOfDesignation = designationList;


            model.EmployeeCode = GenerateEmployeeCode();
            TempData["msg"] = "";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployeeSetup(TblEmployeeSetup model)
        {
            try
            {
               //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    if (DoesToolCodeExists(model.EmployeeCode))
                    {
                        model.EmployeeCode = GenerateEmployeeCode().ToString();
                    }
                    var UserId = model.UserId;
                    var EmployeeCode = model.EmployeeCode;
                    model.Iuser = User.Identity.Name;
                    model.BusinessCode ="2";
                    model.PlantCode = "03";
                    model.Idate = DateTime.Now;
                    _context.Add(model);
                    await _context.SaveChangesAsync();

                   if(!string.IsNullOrEmpty(UserId))
                   { 
                      TblUserWiseEmployeeMapping map = new TblUserWiseEmployeeMapping();
                        map.UserId = UserId;
                        map.EmployeeCode = EmployeeCode;
                        _context.Add(map);
                        await _context.SaveChangesAsync();
                   }
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

            var unMappedUserList = (from c in _context.AspNetUsers
                                    where !_context.TblUserWiseEmployeeMappings.Any(u => u.UserId == c.Email)
                                    select new
                                    {
                                        UserId = c.Email,
                                        UserName = c.Email,
                                    }).ToList();
            unMappedUserList.Insert(0, new { UserId = "", UserName = "<Select User Email>" });
            ViewBag.UnMappedUserList = unMappedUserList;
            List<TblDepartment> departmentList = new List<TblDepartment>();
            departmentList = _context.TblDepartments.ToList();
            ViewBag.ListOfDepartment = departmentList;

            List<TblDesignation> designationtList = new List<TblDesignation>();
            designationtList = _context.TblDesignations.ToList();
            ViewBag.ListOfDesignationt = designationtList;

            TblEmployeeSetup employeeSetupModel = new TblEmployeeSetup();
            var result = _context.TblEmployeeSetups.Where(s=>s.Id== vId).First();
            employeeSetupModel.Id = result.Id;
            employeeSetupModel.EmployeeCode = result.EmployeeCode;
            employeeSetupModel.EmployeeName = result.EmployeeName;
            employeeSetupModel.DepartmentCode = result.DepartmentCode;
            employeeSetupModel.DesignationCode = result.DesignationCode;
            employeeSetupModel.ContactNo = result.ContactNo;
            employeeSetupModel.IsApprover = result.IsApprover;
            employeeSetupModel.IsSmsreceiver = result.IsSmsreceiver;
            employeeSetupModel.Comments = result.Comments;

            employeeSetupModel.BusinessCode = result.BusinessCode;
            employeeSetupModel.PlantCode = result.PlantCode;

            var existingMapping =  _context.TblUserWiseEmployeeMappings.Where(m => m.EmployeeCode == result.EmployeeCode).FirstOrDefault();
            if (existingMapping != null)
            {

                employeeSetupModel.UserId = existingMapping.UserId;
            }
                return View(employeeSetupModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployeeSetup(TblEmployeeSetup model) {
            var vId=   model.Id;
            try
            {
                var employeeSetupToUpdate = await _context.TblEmployeeSetups.FirstOrDefaultAsync(s=>s.Id==vId);
                employeeSetupToUpdate.Edate= DateTime.Now;
                employeeSetupToUpdate.Euser= User.Identity.Name;
                var UserId = model.UserId;
                var EmployeeCode = employeeSetupToUpdate.EmployeeCode;
                if (await TryUpdateModelAsync<TblEmployeeSetup>(
                    employeeSetupToUpdate,
                    "",
                    s => s.EmployeeCode,
                    s => s.EmployeeName,
                    s => s.DepartmentCode,
                    s => s.DesignationCode,
                    s => s.ContactNo,
                    s => s.IsApprover,
                    s => s.IsSmsreceiver,
                    s => s.Comments,
                    s => s.BusinessCode,
                    s => s.PlantCode
                    
                    )) ;
                await _context.SaveChangesAsync();

                var existingMapping = await _context.TblUserWiseEmployeeMappings.FirstOrDefaultAsync(m => m.UserId == UserId);
                if (existingMapping != null)
                {

                    existingMapping.UserId = model.UserId;
                    await _context.SaveChangesAsync();

                }
                else 
                {
                    if (!string.IsNullOrEmpty(UserId))
                    {
                        TblUserWiseEmployeeMapping map = new TblUserWiseEmployeeMapping();
                        map.UserId = UserId;
                        map.EmployeeCode = EmployeeCode;
                        _context.Add(map);
                        await _context.SaveChangesAsync();
                    }
                }
               
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
                TblEmployeeSetup model= _context.TblEmployeeSetups.Where(s=> s.Id == vId).First();
                if (model != null)
                {
                    _context.TblEmployeeSetups.Remove(model);
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
        public JsonResult GetDesignationByDepartment(string departmentCode)
        {
            
            var designationList = (from c in _context.TblDesignations
                                   where c.DepartmentCode == departmentCode 
                                   select new
                                   {
                                       DesignationCode = c.DesignationCode,
                                       DesignationName = c.DesignationName
                                   }
              ).ToList();

            designationList.Insert(0, new { DesignationCode = "", DesignationName = "Select Designation" });
           
            return Json(designationList);
        }
        #region private classes
        private  string GenerateEmployeeCode()
        {
         
           // var yearMonth = DateTime.Now.ToString("yyyyMM");
            var result =  _context.TblEmployeeSetups.OrderBy(x => x.Id).Select(x=>x.EmployeeCode).LastOrDefault();
            var lastGrn = string.IsNullOrEmpty(result) ? "000000" : result;


            var last5digits = "1";
            if (lastGrn.Length > 4)
            {
                last5digits = lastGrn.Substring(lastGrn.Length - 4);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D4");
            //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
            var generatedCode = $"E-{lastNumberString}";
            return generatedCode;
        }
        public bool DoesToolCodeExists(string vToolCode)
        {

            return _context.TblEmployeeSetups.Any(e => e.EmployeeCode == vToolCode);
        }
        #endregion
    }
}
