using LILI_TMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_TMS.Controllers
{
    [Authorize]
    public class TicketAssigneeController : Controller
    {
        private readonly dbTicketManagementContext _context;
        private readonly IUserWiseBusinessAndPlantService _userbusinessandplant;
        private readonly string BusinessCode;
        private readonly string PlantCode;
        public TicketAssigneeController(dbTicketManagementContext context, IUserWiseBusinessAndPlantService userbusinessandplant)
        {
            _context = context;
            _userbusinessandplant = userbusinessandplant;
            if (_userbusinessandplant != null)
            {
                BusinessCode = _userbusinessandplant.GetUserWiseBusinessCode();
                PlantCode = _userbusinessandplant.GetUserWisePlantCodes();
            }
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
            var data =
            from sd in _context.TblServiceDepartmentTicketAssignments
            join c in _context.TblComplainTicket on sd.TicketNo equals c.TicketNo
            join d in _context.TblDepartments on c.DepartmentCode equals d.DepartmentCode
            join e in _context.TblUserWiseEmployeeMappings on sd.AssignToCode equals e.EmployeeCode
            where e.UserId== User.Identity.Name
            select new TblComplainTicket
            {
                Id = c.Id,
                TicketNo = c.TicketNo,
                TicketDate = c.TicketDate,
                DepartmentName = d.DepartmentName,
                ComplainDetails = c.ComplainDetails,
                SeverityLevelCode = c.SeverityLevelCode,
                StatusCode = c.StatusCode
            };
            IQueryable<TblComplainTicket> model = data;



            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.TicketNo.Contains(searchString)
                                      || s.DepartmentCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.TicketNo);
                    break;

                default:
                    model = model.OrderByDescending(s => s.TicketNo);
                    break;
            }
            int pageSize = 7;
            return View(await PaginatedList<TblComplainTicket>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult AddAssigneeInfo(int vId)
        {



            List<TblComplainType> complainTypes = new List<TblComplainType>();

            complainTypes = (from c in _context.TblComplainTypes
                             select new TblComplainType
                             {
                                 ComplainTypeName = c.ComplainTypeName,
                                 ComplainTypeCode = c.ComplainTypeCode
                             }).ToList();
            complainTypes.Insert(0, new TblComplainType
            {
                ComplainTypeName = "-Select Type-",
                ComplainTypeCode = "*"
            });
            ViewBag.ComplainTypes = complainTypes;

            List<TblMachineSetup> machineSetups = new List<TblMachineSetup>();

            machineSetups = (from c in _context.TblMachineSetups
                             select new TblMachineSetup
                             {
                                 MachineName = c.MachineName,
                                 MachineCode = c.MachineCode
                             }).ToList();

            ViewBag.MachineSetups = machineSetups;

            List<TblSeverityLevel> severityLevels = new List<TblSeverityLevel>();

            severityLevels = (from c in _context.TblSeverityLevels
                              select new TblSeverityLevel
                              {
                                  SeverityLevelName = c.SeverityLevelName,
                                  SeverityLevelCode = c.SeverityLevelCode
                              }).ToList();
            severityLevels.Insert(0, new TblSeverityLevel
            {
                SeverityLevelName = "-Select Severity -",
                SeverityLevelCode = "*"
            });
            ViewBag.SeverityLevels = severityLevels;

            List<TblEmployeeSetup> employeeList = new List<TblEmployeeSetup>();
            var userDept = (from c in _context.TblUserWiseEmployeeMappings
                            from u in _context.TblEmployeeSetups
                            where c.EmployeeCode == u.EmployeeCode
                            && c.UserId == User.Identity.Name
                            select u.DepartmentCode).First().ToString();
            employeeList = (from c in _context.TblEmployeeSetups
                            from d in _context.TblDepartments
                            where (c.DepartmentCode == d.DepartmentCode)
                            select new TblEmployeeSetup
                            {
                                EmployeeName = c.EmployeeName,
                                EmployeeCode = c.EmployeeCode,
                                DepartmentCode = c.DepartmentCode,
                                IsApprover = c.IsApprover,
                                TypeCode = d.TypeCode
                            }).ToList();
            employeeList.Insert(0, new TblEmployeeSetup
            {
                EmployeeName = "- Select Employee -",
                EmployeeCode = "*",
                DepartmentCode = userDept,
                IsApprover = false,
                TypeCode = "Service",
            });
          

            ViewBag.AssignList = employeeList.Where(s => s.IsApprover == false && s.DepartmentCode == userDept).ToList();
           

            List<TblPart> partList = new List<TblPart>();
            partList = _context.TblParts.ToList();
            partList.Insert(0, new TblPart
            {
                PartsName = "- Select Parts -",
                PartsCode = "",
                Unit = "",
                Quantity = 0,
            });
            ViewBag.LisOfParts = partList;




            TblComplainTicket model = new TblComplainTicket();
            var complainTicket = _context.TblComplainTicket.Where(x => x.Id == vId).First();
            var tktNo = complainTicket.TicketNo;
            model.JobNo = GenerateCode();
            model.JobDate = DateTime.Now;
            model.TicketNo = complainTicket.TicketNo;

            model.TicketDate = complainTicket.TicketDate;
            model.StatusCode = complainTicket.StatusCode;
            model.ComplainTypeCode = complainTicket.ComplainTypeCode;
            model.Comments = complainTicket.Comments;
            model.ComplainDetails = complainTicket.ComplainDetails;
            model.SeverityLevelCode = complainTicket.SeverityLevelCode;
            var ApproverCode = _context.TblComplainDepartmentApprovals.Where(x => x.TicketNo == tktNo).Select(i => i.ApproverCode).First();
            model.ApproverCode = ApproverCode;
            model.TblComplainTicketsImageDetails = _context.TblComplainTicketsImageDetails.Where(x => x.TicketNo == tktNo).ToList();



            var departmentCode = (from e in _context.TblEmployeeSetups
                                  from u in _context.TblUserWiseEmployeeMappings
                                  from d in _context.TblDepartments
                                  where (e.DepartmentCode == d.DepartmentCode && u.EmployeeCode == e.EmployeeCode && u.UserId == User.Identity.Name)
                                  select d.DepartmentCode
                                     ).First().ToString();
            model.DepartmentCode = complainTicket.DepartmentCode;
            model.DepartmentName = _context.TblDepartments.Where(x => x.DepartmentCode == complainTicket.DepartmentCode).Select(i => i.DepartmentName).First();

            model.ApproverComments = _context.TblComplainDepartmentApprovals.Where(_x => _x.TicketNo == tktNo).Select(_x => _x.ApproverComments).First();
            model.ApproverInfoViewModel = (from c in _context.TblEmployeeSetups
                                           from u in _context.TblUserWiseEmployeeMappings
                                           from d in _context.TblDepartments
                                           from des in _context.TblDesignations
                                           where c.DepartmentCode == d.DepartmentCode
                                           && c.EmployeeCode == u.EmployeeCode
                                           && c.DesignationCode == des.DesignationCode
                                           && c.IsApprover == true
                                           && u.UserId == User.Identity.Name
                                           select new ApproverInfoViewModel
                                           {
                                               ApproverName = c.EmployeeName,
                                               ApproverId = c.EmployeeCode,
                                               Department = d.DepartmentName,
                                               Designation = des.DesignationName,
                                               ContactNo = c.ContactNo,
                                               SendSms = c.IsSmsreceiver,
                                           }).ToList();
            var selectedMachineList = _context.TblComplainTicketMachineDetails.Where(s => s.TicketNo == model.TicketNo).ToList();
            ViewBag.SelectedMachine = selectedMachineList.ToList();

            if (_context.TblTicketAssigneeInfos.Any(x => x.TicketNo == complainTicket.TicketNo))
            {
                var JobExecutedByCode = _context.TblTicketAssigneeInfos.Where(x => x.TicketNo == complainTicket.TicketNo).FirstOrDefault().JobExecutedByCode;
                model.JobExecutedBy = _context.TblEmployeeSetups.FirstOrDefault(x=>x.EmployeeCode== JobExecutedByCode).EmployeeName;
            }
            if (_context.TblTicketAssigneeInfoPartsDetails.Any(x => x.TicketNo == complainTicket.TicketNo)) 
            {
                var data = _context.TblTicketAssigneeInfoPartsDetails.Where(x => x.TicketNo == complainTicket.TicketNo).ToList();

                List<TblTicketAssigneeInfoPartsDetail> partDetialList = new List<TblTicketAssigneeInfoPartsDetail>();
                foreach (var item in data)
                {
                    TblTicketAssigneeInfoPartsDetail partDetail = new TblTicketAssigneeInfoPartsDetail();
                    partDetail.Id = item.Id;
                    partDetail.JobNo = item.JobNo;
                    partDetail.TicketNo = item.TicketNo;
                    partDetail.PartsCode = item.PartsCode;
                    partDetail.Quantity = item.Quantity;
                    partDetail.PartsName = _context.TblParts.FirstOrDefault(x => x.PartsCode == item.PartsCode).PartsName;

                    partDetialList.Add(partDetail);
                }
                model.TblTicketAssigneeInfoPartsDetails = partDetialList;
            }

            return View(model);
        }
        public async Task<ActionResult> CreateAssigneeInfo(TblTicketAssigneeInfo TblTicketAssigneeInfos)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var departmentCode = (from e in _context.TblEmployeeSetups
                                          from u in _context.TblUserWiseEmployeeMappings
                                          from d in _context.TblDepartments
                                          where (e.DepartmentCode == d.DepartmentCode && u.EmployeeCode == e.EmployeeCode && u.UserId == User.Identity.Name)
                                          select d.DepartmentCode
                              ).First().ToString();
                    var result = await UpdateComplainTicketStatus(TblTicketAssigneeInfos.TicketNo, TblTicketAssigneeInfos.StatusCode);
                    TblTicketAssigneeInfos.ServiceDepartmentCode = departmentCode;
                    TblTicketAssigneeInfos.BusinessCode = BusinessCode;
                    TblTicketAssigneeInfos.PlantCode = PlantCode;
                    TblTicketAssigneeInfos.Iuser = User.Identity.Name;
                    TblTicketAssigneeInfos.Idate = DateTime.Now;
                    _context.Add(TblTicketAssigneeInfos);
                    await _context.SaveChangesAsync();


                }
                catch (Exception ex)
                {

                    throw;
                }

            }
            return Ok();
        }

        public async Task<bool> UpdateComplainTicketStatus(string ticketNo, string statusCode)
        {
            try
            {
                var ticketInfoToUpdate = await _context.TblComplainTicket.FirstOrDefaultAsync(s => s.TicketNo == ticketNo);
                ticketInfoToUpdate.StatusCode = statusCode;
                ticketInfoToUpdate.Edate = DateTime.Now;
                ticketInfoToUpdate.Euser = User.Identity.Name;
                if (await TryUpdateModelAsync<TblComplainTicket>(
                    ticketInfoToUpdate,
                    "",
                    s => s.StatusCode
                    )) ;

                return true;
            }
            catch (Exception ex)
            {

                throw;
                return false;
            }

        }
        public JsonResult GetApproverDetails(string DepartmentCode)
        {
            try
            {
                var sa = new JsonSerializerSettings();
                var model = (from c in _context.TblEmployeeSetups
                             from d in _context.TblDepartments
                             from des in _context.TblDesignations
                             where c.DepartmentCode == DepartmentCode && c.DepartmentCode == d.DepartmentCode
                             && c.DesignationCode == des.DesignationCode
                             && c.IsApprover == true
                             select new TblEmployeeSetup
                             {
                                 EmployeeName = c.EmployeeName,
                                 EmployeeCode = c.EmployeeCode,
                                 DepartmentName = d.DepartmentName,
                                 DesignationName = des.DesignationName,
                                 ContactNo = c.ContactNo,
                                 IsSmsreceiver = c.IsSmsreceiver,
                             }).ToList();

                return Json(model);
            }
            catch (Exception ex)
            {

                return Json("");
            }
        }
     
        private string GenerateCode()
        {

            // var yearMonth = DateTime.Now.ToString("yyyyMM");
            var result = _context.TblServiceDepartmentTicketAssignments.OrderBy(x => x.Id).Select(x => x.AssignNo).LastOrDefault();
            var lastGrn = string.IsNullOrEmpty(result) ? "0000000" : result;


            var last5digits = "1";
            if (lastGrn.Length > 4)
            {
                last5digits = lastGrn.Substring(lastGrn.Length - 4);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D4");
            //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
            var generatedCode = $"JN-{lastNumberString}";
            return generatedCode;
        }
    }
}
