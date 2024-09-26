﻿using LILI_TMS;
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
    public class ComplainTypeSetupController : Controller
    {
        private readonly dbTicketManagementContext _context;

        public ComplainTypeSetupController(dbTicketManagementContext context)
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
                from s in _context.TblComplainTypes
                join d in _context.TblDepartments on s.DepartmentCode equals d.DepartmentCode
                select new TblComplainType
                {
                    Id = s.Id,
                    ComplainTypeCode = s.ComplainTypeCode,
                    ComplainTypeName = s.ComplainTypeName,
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
                                 where (s.ComplainTypeName.Contains(searchString) || s.ComplainTypeCode.Contains(searchString))
                                   select new TblComplainType
                                   {
                                       Id = s.Id,
                                       ComplainTypeCode = s.ComplainTypeCode,
                                       ComplainTypeName = s.ComplainTypeName,
                                       DepartmentCode = s.DepartmentCode,
                                       DepartmentName = s.DepartmentName,
                                       Comments = s.Comments,
                                       Idate=s.Idate
                                   });
            }

            switch (sortOrder)
            {
                case "name_desc":
                    employeesList = employeesList.OrderByDescending(s => s.ComplainTypeCode);
                    break;

                default:
                    employeesList = employeesList.OrderBy(s => s.Idate);
                    break;
            }
            int pageSize = 7;

            return View(await PaginatedList<TblComplainType>.CreateAsync(employeesList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {

            TblComplainType model = new TblComplainType();

            var departmentList = (from c in _context.TblDepartments
                            select new
                            {
                                DepartmentCode = c.DepartmentCode,
                                DepartmentName = c.DepartmentName
                            }
            ).ToList();

            departmentList.Insert(0, new { DepartmentCode = "", DepartmentName = "<Select Department>" });
            ViewBag.ListOfDepartment = departmentList;


            model.ComplainTypeCode = GenerateComplainTypeCode();
            TempData["msg"] = "";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateComplainTypeSetup(TblComplainType model)
        {
            try
            {
               //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    if (DoesToolCodeExists(model.ComplainTypeCode))
                    {
                        model.ComplainTypeCode = GenerateComplainTypeCode().ToString();
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

            TblComplainType designationModel = new TblComplainType();
            var result = _context.TblComplainTypes.Where(s=>s.Id== vId).First();
            designationModel.Id = result.Id;
            designationModel.ComplainTypeCode = result.ComplainTypeCode;
            designationModel.ComplainTypeName = result.ComplainTypeName;
            designationModel.DepartmentCode = result.DepartmentCode;
            designationModel.Comments = result.Comments;
            designationModel.BusinessCode = result.BusinessCode;
            designationModel.PlantCode = result.PlantCode;

            return View(designationModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateComplainTypeSetup(TblDesignation model) {
            var vId=   model.Id;
            try
            {
                var departmentSetupToUpdate = await _context.TblComplainTypes.FirstOrDefaultAsync(s=>s.Id==vId);
                departmentSetupToUpdate.Edate= DateTime.Now;
                departmentSetupToUpdate.Euser= User.Identity.Name;
                if (await TryUpdateModelAsync<TblComplainType>(
                    departmentSetupToUpdate,
                    "",
                    s => s.ComplainTypeCode,
                    s => s.ComplainTypeName,
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
                TblComplainType model= _context.TblComplainTypes.Where(s=> s.Id == vId).First();
                if (model != null)
                {
                    _context.TblComplainTypes.Remove(model);
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
        private  string GenerateComplainTypeCode()
        {
         
           // var yearMonth = DateTime.Now.ToString("yyyyMM");
            var result =  _context.TblComplainTypes.OrderBy(x => x.Id).Select(x=>x.ComplainTypeCode).LastOrDefault();
            var lastGrn = string.IsNullOrEmpty(result) ? "00000" : result;


            var last5digits = "1";
            if (lastGrn.Length > 3)
            {
                last5digits = lastGrn.Substring(lastGrn.Length - 3);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D3");
            //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
            var generatedCode = $"C-{lastNumberString}";
            return generatedCode;
        }
        public bool DoesToolCodeExists(string vToolCode)
        {

            return _context.TblComplainTypes.Any(e => e.ComplainTypeCode == vToolCode);
        }
        #endregion
    }
}
