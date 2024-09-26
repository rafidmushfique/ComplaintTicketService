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
using Microsoft.Data.SqlClient;
namespace LILI_TMS.Controllers
{
    [Authorize]
    public class ComplainTicketApprovalController : Controller
    {
        
        private readonly dbTicketManagementContext _context;
        private readonly IUserWiseBusinessAndPlantService _userbusinessandplant;
        private readonly string BusinessCode;
        private readonly string PlantCode;
        public ComplainTicketApprovalController(
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

            //var userDepartment = (from c in _context.TblUserWiseEmployeeMappings
            //                      from e in _context.TblEmployeeSetups
            //                      from d in _context.TblDepartments
            //                      where c.UserId == User.Identity.Name
            //                      && c.EmployeeCode == e.EmployeeCode
            //                      && e.DepartmentCode == d.DepartmentCode
            //                      select e.DepartmentCode).FirstOrDefault().ToString();

            var userDepartmentList = (from c in _context.TblUserWiseEmployeeMappings
                                  from e in _context.TblEmployeeSetups
                                  from d in _context.TblDepartments
                                  where c.UserId == User.Identity.Name
                                  && c.EmployeeCode == e.EmployeeCode
                                  && e.DepartmentCode == d.DepartmentCode
                                  select e).ToList();

            var userDepartment = userDepartmentList.Count()>0?userDepartmentList.FirstOrDefault().DepartmentCode:"N/A";

            //var data = await GetComplainTickets();

            var userId = User.Identity.Name;
            var uId = _context.AspNetUsers.Where(u => u.Email == userId).FirstOrDefault().Id;
            var userRoleId = _context.AspNetUserRoles.Where(x => x.UserId == uId).FirstOrDefault().RoleId; //RoleId:2=Admin

            IQueryable<TblComplainTicket> model = from c in _context.TblComplainTicket
                                    from d in _context.TblDepartments
                                    from ct in _context.TblComplainTypes
                                    where c.DepartmentCode == d.DepartmentCode && c.ComplainTypeCode == ct.ComplainTypeCode && ((userRoleId == "2") ? 1 == 1 : c.DepartmentCode == userDepartment)
                                    select new TblComplainTicket
                                    {
                                        Id = c.Id,
                                        TicketNo = c.TicketNo,
                                        TicketDate = (c.TicketDate),
                                        DepartmentName = d.DepartmentName,
                                        ComplainTypeName = ct.ComplainTypeName,
                                        ComplainDetails = c.ComplainDetails,
                                        SeverityLevelCode = c.SeverityLevelCode,
                                        StatusCode = c.StatusCode,
                                    };


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


        public IActionResult Update(int Id)
        {
            List<TblDepartment> serviceDepartmentList = new List<TblDepartment>();
           

            serviceDepartmentList = (from c in _context.TblDepartments
                                     select new TblDepartment
                                     {
                                         DepartmentName = c.DepartmentName,
                                         DepartmentCode = c.DepartmentCode,
                                         TypeCode = c.TypeCode,
                                     }).ToList();
            serviceDepartmentList.Insert(0, new TblDepartment
            {
                DepartmentName = "-Select Service Department-",
                DepartmentCode = "*",
                TypeCode= "Service"
            });
            ViewBag.ServiceDepartmentList = serviceDepartmentList.Where(x=>x.TypeCode=="Service");
            List<TblDepartment> departmentList = new List<TblDepartment>();

            departmentList = (from c in _context.TblDepartments
                              select new TblDepartment
                              {
                                  DepartmentName = c.DepartmentName,
                                  DepartmentCode = c.DepartmentCode
                              }).ToList();
            departmentList.Insert(0, new TblDepartment
            {
                DepartmentName = "-Select Department-",
                DepartmentCode = "*"
            });
            ViewBag.DepartmentList = departmentList;

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
                            from d in _context.TblUserWiseEmployeeMappings
                            where (c.EmployeeCode == d.EmployeeCode && d.UserId == User.Identity.Name)
                            select new TblEmployeeSetup
                            {
                                ApproverName = c.EmployeeName,
                                ApproverCode = c.EmployeeCode,
                               
                            }).ToList();
            ViewBag.EmployeeList = employeeList;

            TblComplainTicket model= new TblComplainTicket();





           
            model.ApprovalDate = DateTime.Now;
            var data= _context.TblComplainTicket.Where(s=>s.Id==Id).First();

            var IfExists = _context.TblComplainDepartmentApprovals.Any(x => x.TicketNo == data.TicketNo);
            if (IfExists) 
            {
                model.ApprovalNo = _context.TblComplainDepartmentApprovals.Where(s => s.TicketNo == data.TicketNo).Select(i => i.ApprovalNo).First() ;
                model.ServiceDepartmentCode = _context.TblComplainDepartmentApprovals.Where(s => s.TicketNo == data.TicketNo).Select(x => x.ServiceDepartmentCode).First();
                model.ApproverComments = _context.TblComplainDepartmentApprovals.Where(s => s.TicketNo == data.TicketNo).Select(x => x.ApproverComments).First();
                model.ApproverUser = _context.TblComplainDepartmentApprovals.Where(s => s.TicketNo == data.TicketNo).Select(x => x.Iuser).First();
            }
            else
            {
                model.ApprovalNo = GenerateApprovalNo();
            }
            

            model.Id = data.Id;
            model.TicketNo = data.TicketNo;
            model.TicketDate = data.TicketDate;
            model.DepartmentCode = data.DepartmentCode;
            model.ComplainTypeCode = data.ComplainTypeCode;
            model.SeverityLevelCode = data.SeverityLevelCode;
            model.ComplainDetails= data.ComplainDetails;
            model.Comments= data.Comments;
            model.StatusCode= data.StatusCode;
            var user = User.Identity.Name;
            var selectedMachineList = _context.TblComplainTicketMachineDetails.Where(s => s.TicketNo == data.TicketNo).ToList();
            //model.ServiceDepartmentCode = _context.TblComplainDepartmentApprovals.Where(s => s.TicketNo == data.TicketNo).Select(x => x.ServiceDepartmentCode).First();
            //model.ApproverComments = _context.TblComplainDepartmentApprovals.Where(s => s.TicketNo == data.TicketNo).Select(x => x.ApproverComments).First();
            //model.ApproverUser = _context.TblComplainDepartmentApprovals.Where(s => s.TicketNo == data.TicketNo).Select(x => x.Iuser).First();

            model.TblComplainDepartmentApprovalSmsdetails = (from c in _context.TblComplainDepartmentApprovalSmsdetails
                                                                 from e in _context.TblEmployeeSetups
                                                                 from d in _context.TblDesignations
                                                                 from dep in _context.TblDepartments
                                                                 where c.ApproverCode == e.EmployeeCode && e.DesignationCode == d.DesignationCode && e.DepartmentCode == dep.DepartmentCode
                                                                 && c.TicketNo == data.TicketNo
                                                                 select new TblComplainDepartmentApprovalSmsdetail
                                                                 {
                                                                     ApproverCode = c.ApproverCode,
                                                                     ApproverName = e.EmployeeName,
                                                                     Department = dep.DepartmentName,
                                                                     Designation = d.DesignationName,
                                                                     Smsto = c.Smsto,
                                                                     IsSmsreceiver = c.IsSmsreceiver,
                                                                 }).ToList(); ;

            model.TblComplainTicketsImageDetails = _context.TblComplainTicketsImageDetails.Where(x => x.TicketNo == data.TicketNo).ToList();
            ViewBag.SelectedMachine = selectedMachineList.ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTicketApproval(TblComplainDepartmentApproval TblComplainDepartmentApprovals)
        {

            var vId = TblComplainDepartmentApprovals.Id;
            var ticketNo = TblComplainDepartmentApprovals.TicketNo;
            var StatusCode = TblComplainDepartmentApprovals.StatusCode;
            var IsTicketUpdate = _context.TblComplainDepartmentApprovals.Any(x=>x.TicketNo== ticketNo);
            if (IsTicketUpdate)
            {
                    if (!DependencyCheck(ticketNo))
                    {
                
                        try
                        {
                            var departmentApprovalSms = "";
                     
                        var ticketInfoToUpdate = await _context.TblComplainDepartmentApprovals.FirstOrDefaultAsync(s => s.TicketNo == ticketNo);
                            ticketInfoToUpdate.Edate = DateTime.Now;
                            ticketInfoToUpdate.Euser = User.Identity.Name;
                            ticketInfoToUpdate.StatusCode = TblComplainDepartmentApprovals.StatusCode;
                            if (await TryUpdateModelAsync<TblComplainDepartmentApproval>(
                                ticketInfoToUpdate,
                                "",
                                s => s.ServiceDepartmentCode,
                                s => s.ApproverComments,
                                s => s.StatusCode

                                )) ;
                        _context.TblComplainDepartmentApprovalSmsdetails.RemoveRange(_context.TblComplainDepartmentApprovalSmsdetails.Where(d => d.TicketNo == ticketNo));
                        if (TblComplainDepartmentApprovals.TblComplainDepartmentApprovalSmsdetails != null)
                        {
                            foreach (var item in TblComplainDepartmentApprovals.TblComplainDepartmentApprovalSmsdetails.ToList())
                            {
                                TblComplainDepartmentApprovalSmsdetail prodDetail = new TblComplainDepartmentApprovalSmsdetail();
                                prodDetail.TicketNo = ticketNo;
                                prodDetail.ApprovalNo = TblComplainDepartmentApprovals.ApprovalNo;
                                prodDetail.ApproverCode = TblComplainDepartmentApprovals.ApproverCode;
                                prodDetail.Smsto = item.Smsto;
                                prodDetail.IsSmsreceiver = item.IsSmsreceiver;
                                await _context.AddAsync(prodDetail);
                            }
                            var resutl = await UpdateComplainTicketStatus(ticketNo, StatusCode);
                            await _context.SaveChangesAsync();
                        }

                    }
                        catch (Exception ex)
                        {

                            throw;

                        }
                    }
                
            }
            else {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                      
                       



                        if (DoesCodeExists(TblComplainDepartmentApprovals.ApprovalNo))
                        {
                            TblComplainDepartmentApprovals.ApprovalNo = GenerateApprovalNo();
                        }
                     
                        //var buscode = _userbusinessandplant.GetUserWiseBusinessCode();
                        //var plantCode= _userbusinessandplant.GetUserWisePlantCodes();

                        var resutl= await UpdateComplainTicketStatus(ticketNo, StatusCode);
                        TblComplainDepartmentApprovals.BusinessCode = BusinessCode;
                        TblComplainDepartmentApprovals.PlantCode = PlantCode;
                        TblComplainDepartmentApprovals.Iuser = User.Identity.Name;
                        TblComplainDepartmentApprovals.Idate = DateTime.Now;
                        await _context.AddAsync(TblComplainDepartmentApprovals);
                        await _context.SaveChangesAsync();

                      
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                   
                }
              

            }
            catch (Exception ex)
            {
                throw;
                //ModelState.AddModelError("", "Unable to save changes. " +
                //    "Try again, and if the problem persists, " +
                //    "see your system administrator.");
              
            }
            }
            return Ok();

        }
        public async Task<bool> UpdateComplainTicketStatus(string ticketNo,string statusCode)
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
        public bool DeleteTicket(int id)
        {
            try
            {
                var idExists = _context.TblComplainTicket.Any(e => e.Id == id);
                if (!idExists) {
                    NotFound();
                }
                TblComplainTicket model = _context.TblComplainTicket.Where(s => s.Id == id).First();

                var ticketNo= model.TicketNo;

                if (DependencyCheck(ticketNo))
                {
                    var approverdetail = _context.TblComplainTicketApproverSmsdetails.Where(s => s.TicketNo == ticketNo);
                    _context.TblComplainTicketApproverSmsdetails.RemoveRange(approverdetail);
                    var machineDetail = _context.TblComplainTicketMachineDetails.Where(s=>s.TicketNo==ticketNo);
                    _context.TblComplainTicketMachineDetails.RemoveRange(machineDetail);
                    var imdDetial = _context.TblComplainTicketsImageDetails.Where(s => s.TicketNo == ticketNo);
                    var imgModel= imdDetial.ToList();
                    foreach (var item in imgModel)
                    {
                        var check = RemoveFile(item.FileName);
                    }
                    _context.TblComplainTicketsImageDetails.RemoveRange(imdDetial);

                    _context.TblComplainTicket.Remove(model);
                    _context.SaveChanges();
                }




                return true;

            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error Occurred while trying to Delete data.";
                return false;
            }
            
        }
        [HttpPost]
        public async ValueTask<JsonResult> UploadFile(IFormFile file)
        {
            var sa = new JsonSerializerSettings();
            //HttpPostedFileBase file = Request.Files["file"];
            IFormFile Attachment = null;
            var (originalFileName, filename, fileLocation, extension) = await UploadFileAndReturnFileName(file);
            var data = new AttachmentFileViewModel
            {
                OriginalFileName = originalFileName,
                Filename = filename,
                FileLocation = fileLocation,
                Extension = extension
            };

            return Json(data);
        }
        [HttpPost]
        public JsonResult GetApproverDetials(string DepartmentCode)
        {
            try
            {
                var sa = new JsonSerializerSettings();
                var model =  (    from c in _context.TblEmployeeSetups
                                  from d in _context.TblDepartments
                                  from des in _context.TblDesignations
                                  where c.DepartmentCode == DepartmentCode && c.DepartmentCode==d.DepartmentCode
                                  && c.DesignationCode == des.DesignationCode
                                  && c.IsApprover == true
                                  select new TblEmployeeSetup
                                  {
                                   EmployeeName = c.EmployeeName,
                                   EmployeeCode = c.EmployeeCode,
                                   DepartmentName= d.DepartmentName,
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
        private string GenerateApprovalNo()
        {
            
            var result = _context.TblComplainDepartmentApprovals.OrderBy(x => x.Id).Select(x => x.ApprovalNo).LastOrDefault();
            var lastTicket = string.IsNullOrEmpty(result) ? "000000" : result;


            var last5digits = "1";
            if (lastTicket.Length > 4)
            {
                last5digits = lastTicket.Substring(lastTicket.Length - 4);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D4");
       
            var generatedCode = $"A-{lastNumberString}";
            return generatedCode;
        }
        private async Task<(string, string, string, string)> UploadFileAndReturnFileName(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            try
            {

                string uniqueFileName = GenerateUniqueFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);

                var newFileName = String.Concat(uniqueFileName);

                string filePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload")).Root + $@"{newFileName}";
                //string filePath = Path.Combine(_hostEnvironment.WebRootPath,"Upload"); ;
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }


                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(filePath);
                //request.Method = WebRequestMethods.Ftp.UploadFile;


                return (file.FileName, uniqueFileName, filePath, extension);
            }

            catch (WebException e)
            {
                string status = ((FtpWebResponse)e.Response).StatusDescription;
                throw new Exception(status);
            }
        }
        private static string GenerateUniqueFileName(string fileName)
        {
            var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            fileName = Path.GetFileName(fileName);
            return $"{unixTimestamp}"
                   + "_"
                   + Guid.NewGuid()
                   + Path.GetExtension(fileName);
        }
        private bool DoesCodeExists(string vCode)
        {

            return _context.TblComplainDepartmentApprovals.Any(e => e.ApprovalNo == vCode);
        }
        private bool DependencyCheck(string vTicketNo) 
        {
            var chechStatus = _context.TblComplainTicket.Where(x=>x.TicketNo==vTicketNo).Select(e => e.StatusCode).FirstOrDefault();
            if (chechStatus != "Assigned")
            {
                return false;
            }
            else {
                return true;
            }
            
        }
        private async Task<IQueryable<TblComplainTicket>> GetComplainTickets()
        {
            try
            {
                var userId = User.Identity.Name;
                SqlParameter statuParameter = new SqlParameter("@StatusCode", "New");
                SqlParameter userIdParameter = new SqlParameter("@UserId", userId);
                var model = _context.TblComplainTicket.FromSqlRaw("EXEC sp_GetComplainTicketsByStatus @StatusCode,@UserId", statuParameter, userIdParameter);
                //model = model.AsQueryable();
                //IQueryable<TblComplainTicket> data = model.AsQueryable(); 
                return model;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        [HttpPost]
        public bool RemoveFile(string fileName)
        {

            string filePathToDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", fileName);

            if (System.IO.File.Exists(filePathToDelete))
            {
                System.IO.File.Delete(filePathToDelete);
                // Optionally, you can add additional logic or messages after file deletion
            }

            // Redirect to the action or view after deletion
            return true;
        }
    }
   
}
