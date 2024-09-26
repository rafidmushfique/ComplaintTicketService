using LILI_TMS;
using LILI_TMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace LILI_TTS.Controllers
{
    [Authorize]
    public class ServiceDepartmentTicketAssignmentController : Controller
    {
        private readonly dbTicketManagementContext _context;
        private readonly IUserWiseBusinessAndPlantService _userbusinessandplant;
        private readonly string BusinessCode;
        private readonly string PlantCode;
        public ServiceDepartmentTicketAssignmentController(
            dbTicketManagementContext context,
             IUserWiseBusinessAndPlantService userbusinessandplant

            )
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
            //var employees = from s in _context.TblEmployee
            //                select s;

            //var data = await GetComplainTicketsByStatus();

            //var userDepartment = (from c in _context.TblUserWiseEmployeeMappings
            //                     from e in _context.TblEmployeeSetups
            //                     from d in _context.TblDepartments
            //                     where c.UserId == User.Identity.Name
            //                     && c.EmployeeCode == e.EmployeeCode
            //                     && e.DepartmentCode== d.DepartmentCode
            //                     select e.DepartmentCode).FirstOrDefault().ToString();

            var userDepartmentList = (from c in _context.TblUserWiseEmployeeMappings
                                      from e in _context.TblEmployeeSetups
                                      from d in _context.TblDepartments
                                      where c.UserId == User.Identity.Name
                                      && c.EmployeeCode == e.EmployeeCode
                                      && e.DepartmentCode == d.DepartmentCode
                                      select e).ToList();

            var userDepartment = userDepartmentList.Count() > 0 ? userDepartmentList.FirstOrDefault().DepartmentCode : "N/A";

            var userId = User.Identity.Name;
            var uId = _context.AspNetUsers.Where(u => u.Email == userId).FirstOrDefault().Id;
            var userRoleId = _context.AspNetUserRoles.Where(x => x.UserId == uId).FirstOrDefault().RoleId; //RoleId:2=Admin

            var data =
                from c in _context.TblComplainTicket
                from ca in _context.TblComplainDepartmentApprovals
                from d in _context.TblDepartments
                where c.TicketNo== ca.TicketNo
                && ca.ServiceDepartmentCode== d.DepartmentCode
                && ((userRoleId=="2")?1==1: ca.ServiceDepartmentCode == userDepartment)
                select new TblComplainTicket
                {
                    Id = c.Id,
                    TicketNo = c.TicketNo,
                    TicketDate = c.TicketDate,
                    DepartmentName =  _context.TblDepartments.Where(x=>x.DepartmentCode==c.DepartmentCode).Select(i=>i.DepartmentName).First().ToString(),
                    ComplainDetails = c.ComplainDetails,
                    SeverityLevelCode = c.SeverityLevelCode,
                    StatusCode = c.StatusCode,
                };
            IQueryable < TblComplainTicket> ticketList = data;




            // IQueryable <TblDesignation> model = _context.TblDesignations ;


            if (!String.IsNullOrEmpty(searchString))
            {
                //model = model.Where(s => s.DesignationName.Contains(searchString) || s.DesignationCode.Contains(searchString));
                ticketList = ticketList.Where(s => s.TicketNo.Contains(searchString)
                                                    || s.DepartmentCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    ticketList = ticketList.OrderByDescending(s => s.TicketNo);
                    break;

                default:
                    ticketList = ticketList.OrderByDescending(s => s.TicketNo);
                    break;
            }
            int pageSize = 7;
           
            return View(await PaginatedList<TblComplainTicket>.CreateAsync(ticketList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateAssign(TblServiceDepartmentTicketAssignment model)
        {

            var StatusCode = "Assigned";
            var TicketNo = model.TicketNo;

            var TicketAssignmentExists = _context.TblServiceDepartmentTicketAssignments.Any(x=> x. TicketNo== model.TicketNo);
            if (TicketAssignmentExists)
            {
                var checkStatus = _context.TblComplainTicket.Where(x => x.TicketNo == TicketNo).Select(e => e.StatusCode).FirstOrDefault();
                if (checkStatus != "Completed" )
                {
                    try
                    {
                        var serviceInfoToUpdate = await _context.TblServiceDepartmentTicketAssignments.FirstOrDefaultAsync(s => s.TicketNo == TicketNo);
                        if (serviceInfoToUpdate != null)
                        {
                            if (serviceInfoToUpdate.Iuser == User.Identity.Name)
                            {
                                serviceInfoToUpdate.Edate = DateTime.Now;
                                serviceInfoToUpdate.Euser = User.Identity.Name;
                                if (await TryUpdateModelAsync<TblServiceDepartmentTicketAssignment>(
                                       serviceInfoToUpdate,
                                       "",
                                       s => s.AssignToCode,
                                       s => s.Comments
                                       )) ;
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }
                
            }
            else 
            {
                try
                {
                    //TempData["msg"] = "Data Save Unsuccessful";
                    //return View("Create", model);

                    if (ModelState.IsValid)
                    {
                        if (DoesCodeExists(model.AssignNo))
                        {
                            model.AssignNo = GenerateAssignNoCode();
                        }
                        model.StatusCode = StatusCode;

                        var result = await UpdateComplainTicketStatus(model.TicketNo, model.StatusCode);
                        model.AssignByCode = _context.TblUserWiseEmployeeMappings.Where(x => x.UserId == User.Identity.Name).Select(i => i.EmployeeCode).First();

                        model.BusinessCode = BusinessCode;
                        model.PlantCode = PlantCode;
                        model.Iuser = User.Identity.Name;
                        model.Idate = DateTime.Now;
                        _context.Add(model);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        TempData["msg"] = "Data Save Unsuccessful";

                    }
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Error Occured : " + ex.Message;


                }
            }

           
            return RedirectToAction(nameof(Index));
        }

        public IActionResult TicketAssignment(int vId) {
            var ListOfDepartment = (from c in _context.TblDepartments
                                    select new
                                    {
                                        DepartmentCode = c.DepartmentCode,
                                        DepartmentName = c.DepartmentName
                                    }
                                    ).ToList();

            ListOfDepartment.Insert(0, new { DepartmentCode = "", DepartmentName = "<Select Department>" });
            ViewBag.ListOfDepartment = ListOfDepartment;

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

            employeeList = (from c in _context.TblEmployeeSetups
                            from d in  _context.TblDepartments
                            where(c.DepartmentCode==d.DepartmentCode)
                            select new TblEmployeeSetup
                            {
                                ApproverName = c.EmployeeName,
                                ApproverCode = c.EmployeeCode,
                                DepartmentCode = c.DepartmentCode,
                                IsApprover = c.IsApprover,
                                TypeCode = d.TypeCode
                            }).ToList();

            var userDept = (from c in _context.TblUserWiseEmployeeMappings
                            from u in _context.TblEmployeeSetups
                            where c.EmployeeCode == u.EmployeeCode
                            && c.UserId == User.Identity.Name
                            select u.DepartmentCode).First().ToString();
            employeeList.Insert(0, new TblEmployeeSetup
            {
                ApproverName = "- Select Employee -",
                ApproverCode = "*",
                DepartmentCode = "*",
                IsApprover = true,
                TypeCode="",
            }); ;
            employeeList.Insert(0, new TblEmployeeSetup
            {
                ApproverName = "- Select Employee -",
                ApproverCode = "*",
                DepartmentCode= userDept,
                IsApprover = false,
                TypeCode = "Service",
            });

            ViewBag.ApproverList = employeeList.Where(s => s.IsApprover == true).ToList();
            ViewBag.AssignList = employeeList.Where(s => s.IsApprover == false && s.DepartmentCode== userDept).ToList();


           

            TblServiceDepartmentTicketAssignment model = new TblServiceDepartmentTicketAssignment();
           
            var complainTicket = _context.TblComplainTicket.Where(x=> x.Id==vId).First();
            var ServiceTicketInfo = _context.TblServiceDepartmentTicketAssignments.Where(x => x.TicketNo == complainTicket.TicketNo)?.FirstOrDefault();
            var tktNo = complainTicket.TicketNo;

            if (_context.TblServiceDepartmentTicketAssignments.Any(x => x.TicketNo == tktNo))
            {
                model.AssignNo = ServiceTicketInfo.AssignNo.ToString();
                model.Iuser = ServiceTicketInfo.Iuser.ToString();
            }
            else {
                model.AssignNo = GenerateAssignNoCode();
            }
            


            model.TicketNo = complainTicket.TicketNo;

            model.TblComplainTicketsImageDetails = _context.TblComplainTicketsImageDetails.Where(x => x.TicketNo==tktNo).ToList(); 
           model.AssignDate = DateTime.Now;
            model.TicketDate = complainTicket.TicketDate;
            model.StatusCode= complainTicket.StatusCode;
            if (complainTicket.StatusCode != "Approved" && complainTicket.StatusCode != "Rejected" && complainTicket.StatusCode != "New")
            {
                model.AssignToCode = ServiceTicketInfo.AssignToCode;
                model.AssignerComments = ServiceTicketInfo.AssignerComments;
            }


            
            model.ComplainTypeCode= complainTicket.ComplainTypeCode;
            model.Comments = complainTicket.Comments;
            model.ComplainDetails= complainTicket.ComplainDetails;
            model.SeverityLevelCode = complainTicket.SeverityLevelCode;
            var ApproverCode= _context.TblComplainDepartmentApprovals.Where(x => x.TicketNo == tktNo).Select(i => i.ApproverCode).First();
            model.ApproverCode = ApproverCode;

            model.ApproverName= _context.TblEmployeeSetups.Where(x=>x.EmployeeCode==ApproverCode).Select(i=>i.EmployeeName).First();

            var ServiceDepartmentCode= (from e in _context.TblEmployeeSetups
                                 from u in _context.TblUserWiseEmployeeMappings
                                 from d in _context.TblDepartments
                                 where (e.DepartmentCode == d.DepartmentCode && u.EmployeeCode == e.EmployeeCode && u.UserId == User.Identity.Name)
                                 select d.DepartmentCode
                                     ).First().ToString();
            model.DepartmentCode =  complainTicket.DepartmentCode;

            model.ApproverComments = _context.TblComplainDepartmentApprovals.Where(_x => _x.TicketNo == tktNo).Select(_x => _x.ApproverComments).First();
            model.ServiceDepartmentCode = ServiceDepartmentCode;


            model.DepartmentName = _context.TblDepartments.Where(x=>x.DepartmentCode==complainTicket.DepartmentCode).Select(i=>i.DepartmentName).First();
            model.ServiceDepartmentName = _context.TblDepartments.Where(x => x.DepartmentCode == ServiceDepartmentCode).Select(i => i.DepartmentName).First();


            var selectedMachineList = _context.TblComplainTicketMachineDetails.Where(s => s.TicketNo == model.TicketNo).ToList();
            ViewBag.SelectedMachine = selectedMachineList.ToList();
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
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
                return false;
            }

        }
        #region private classes
        private  string GenerateAssignNoCode()
        {
         
           // var yearMonth = DateTime.Now.ToString("yyyyMM");
            var result =  _context.TblServiceDepartmentTicketAssignments.OrderBy(x => x.Id).Select(x=>x.AssignNo).LastOrDefault();
            var lastGrn = string.IsNullOrEmpty(result) ? "0000000" : result;


            var last5digits = "1";
            if (lastGrn.Length > 4)
            {
                last5digits = lastGrn.Substring(lastGrn.Length - 4);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D4");
            //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
            var generatedCode = $"AN-{lastNumberString}";
            return generatedCode;
        }
        private bool DoesCodeExists(string AssignNo)
        {

            return _context.TblServiceDepartmentTicketAssignments.Any(e => e.AssignNo == AssignNo);
        }
        private IQueryable<TblComplainTicket> GetServiceDepartmentTickets()
        {
            try
            {
                var userId = User.Identity.Name;
                SqlParameter statuParameter = new SqlParameter("@StatusCode", "Approved");
                SqlParameter userIdParameter = new SqlParameter("@UserId", userId);
                var model =   _context.TblComplainTicket.FromSqlRaw("EXEC sp_GetComplainTicketsByStatus @StatusCode,@UserId", statuParameter, userIdParameter);
                //model = model.AsQueryable();
                //IQueryable<TblComplainTicket> data = model.AsQueryable(); 
                return model;
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }
        #endregion
    }
}
