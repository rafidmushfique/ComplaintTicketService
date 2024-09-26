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
    public class MachineSetupController : Controller
    {
        private readonly dbTicketManagementContext _context;

        public MachineSetupController(dbTicketManagementContext context)
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
                from s in _context.TblMachineSetups
                join d in _context.TblDepartments on s.DepartmentCode equals d.DepartmentCode
                select new TblMachineSetup
                {
                    Id = s.Id,
                    MachineCode = s.MachineCode,
                    MachineName = s.MachineName,
                    DepartmentName =d.DepartmentName,
                    Comments = s.Comments,
                    ModelNumber = s.ModelNumber,
                    Manufacturer = s.Manufacturer,
                    InstallationDate = s.InstallationDate,
                    OriginCountry = s.OriginCountry,
                    Idate=s.Idate

                }


                );


           // IQueryable <TblDesignation> model = _context.TblDesignations ;


            if (!String.IsNullOrEmpty(searchString))
            {
                //model = model.Where(s => s.DesignationName.Contains(searchString) || s.DesignationCode.Contains(searchString));

                employeesList = (from s in employeesList
                                 where (s.MachineName.Contains(searchString) || s.MachineCode.Contains(searchString))
                                   select new TblMachineSetup
                                   {
                                       Id = s.Id,
                                       MachineCode = s.MachineCode,
                                       MachineName = s.MachineName,
                                       DepartmentName = s.DepartmentName,
                                       Comments = s.Comments,
                                       ModelNumber = s.ModelNumber,
                                       Manufacturer = s.Manufacturer,
                                       InstallationDate = s.InstallationDate,
                                       OriginCountry = s.OriginCountry,
                                       Idate = s.Idate
                                   });
            }

            switch (sortOrder)
            {
                case "name_desc":
                    employeesList = employeesList.OrderByDescending(s => s.MachineCode);
                    break;

                default:
                    employeesList = employeesList.OrderByDescending(s => s.Id);
                    break;
            }
            int pageSize = 7;

            return View(await PaginatedList<TblMachineSetup>.CreateAsync(employeesList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {

            TblMachineSetup model = new TblMachineSetup();

            model.InstallationDate = DateTime.Now;

            var departmentList = (from c in _context.TblDepartments
                            select new
                            {
                                DepartmentCode = c.DepartmentCode,
                                DepartmentName = c.DepartmentName
                            }
            ).ToList();

            departmentList.Insert(0, new { DepartmentCode = "", DepartmentName = "<Select Department>" });
            ViewBag.ListOfDepartment = departmentList;


            model.MachineCode = GenerateMachineCode();
            TempData["msg"] = "";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMachineSetup(TblMachineSetup model)
        {
            try
            {
                //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    if (DoesToolCodeExists(model.MachineCode))
                    {
                        model.MachineCode = GenerateMachineCode().ToString();
                        
                    }
                    
                    model.Iuser = User.Identity.Name;
                    model.BusinessCode = "2";
                    model.PlantCode = "03";
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

            TblMachineSetup model = new TblMachineSetup();
            var result = _context.TblMachineSetups.Where(s=>s.Id== vId).First();
            model.Id = result.Id;
            model.MachineCode = result.MachineCode;
            model.MachineName = result.MachineName;
            model.InstallationDate = result.InstallationDate;
            model.DepartmentCode = result.DepartmentCode;
            model.Comments = result.Comments;
            model.BusinessCode = result.BusinessCode;
            model.PlantCode = result.PlantCode;
            model.OriginCountry =  result.OriginCountry;
            model.Manufacturer = result.Manufacturer;
            model.ModelNumber = result.ModelNumber;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMachineSetup(TblMachineSetup model) {
            var vId=   model.Id;
            try
            {
                var departmentSetupToUpdate = await _context.TblMachineSetups.FirstOrDefaultAsync(s=>s.Id==vId);
                departmentSetupToUpdate.Edate= DateTime.Now;
                departmentSetupToUpdate.Euser= User.Identity.Name;
                if (await TryUpdateModelAsync<TblMachineSetup>(
                    departmentSetupToUpdate,
                    "",
                    s => s.MachineCode,
                    s => s.MachineName,
                    s => s.InstallationDate,
                    s => s.DepartmentCode,
                    s => s.Comments,
                    s => s.BusinessCode,
                    s => s.PlantCode,
                    s => s.OriginCountry,
                    s => s.Manufacturer,
                    s => s.ModelNumber
                    
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
                TblMachineSetup model = _context.TblMachineSetups.Where(s=> s.Id == vId).First();
                if (model != null)
                {
                    _context.TblMachineSetups.Remove(model);
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
        private  string GenerateMachineCode()
        {
         
           // var yearMonth = DateTime.Now.ToString("yyyyMM");
            var result =  _context.TblMachineSetups.OrderBy(x => x.Id).Select(x=>x.MachineCode).LastOrDefault();
            var lastGrn = string.IsNullOrEmpty(result) ? "00000" : result;


            var last5digits = "1";
            if (lastGrn.Length > 3)
            {
                last5digits = lastGrn.Substring(lastGrn.Length - 3);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D3");
            //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
            var generatedCode = $"M-{lastNumberString}";
            return generatedCode;
        }
        public bool DoesToolCodeExists(string vToolCode)
        {

            return _context.TblMachineSetups.Any(e => e.MachineCode == vToolCode);
        }
        #endregion
    }
}
