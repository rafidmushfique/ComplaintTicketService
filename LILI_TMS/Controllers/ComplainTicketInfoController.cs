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
    public class ComplainTicketInfoController : Controller
    {
        
        private readonly dbTicketManagementContext _context;
        private readonly IUserWiseBusinessAndPlantService _userbusinessandplant;
        private readonly string BusinessCode;
        private readonly string PlantCode;
        public ComplainTicketInfoController(
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
            //var data = await GetComplainTickets();
            var userId = User.Identity.Name;
            var uId = _context.AspNetUsers.Where(u=>u.Email == userId).FirstOrDefault().Id;
            var userRoleId = _context.AspNetUserRoles.Where(x => x.UserId == uId).FirstOrDefault().RoleId; //RoleId:2=Admin
            IQueryable<TblComplainTicket> model =   from c in _context.TblComplainTicket
                                                    from d in _context.TblDepartments 
                                                    from ct in _context.TblComplainTypes
                                                    where ((userRoleId== "2")? 1 == 1 : c.Iuser ==  userId)
                                                    && c.DepartmentCode == d.DepartmentCode
                                                    && c.ComplainTypeCode == ct.ComplainTypeCode
                                                    select new TblComplainTicket
                                                    {
                                                        Id = c.Id,
                                                        TicketNo = c.TicketNo,
                                                        TicketDate = (c.TicketDate),
                                                        DepartmentName = d.DepartmentName,
                                                        ComplainTypeName = ct.ComplainTypeName,
                                                        ComplainDetails= c.ComplainDetails,
                                                        SeverityLevelCode= c.SeverityLevelCode,
                                                        StatusCode= c.StatusCode,
                                                    };
            //from d in _context.TblDepartments
            //from ct in _context.TblComplainTypes
            //where c.DepartmentCode == d.DepartmentCode && c.ComplainTypeCode == ct.ComplainTypeCode
            //select new TblComplainTicket {
            //    TicketNo = c.TicketNo,
            //    TicketDate = (c.TicketDate),
            //    DepartmentName = d.DepartmentName,
            //    ComplainTypeName = ct.ComplainTypeName,
            //};


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

        public IActionResult Create()
        {
            TblComplainTicket model = new TblComplainTicket();
            model.TicketNo = GenerateTicketNo();
          
            List<TblDepartment> departmentList = new List<TblDepartment>();
           
            departmentList = (from c in _context.TblDepartments
                              where c.TypeCode=="Complain"
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
            //machineSetups.Insert(0, new TblMachineSetup
            //{
            //    MachineName = "-Select Machine-",
            //    MachineCode = "*"
            //});
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
                SeverityLevelName = "-Select Severity Level-",
                SeverityLevelCode = "*"
            });
            ViewBag.SeverityLevels = severityLevels;


            model.TicketDate = DateTime.Now;
            return View(model);
        }
        public IActionResult Update(int Id)
        {
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




            TblComplainTicket model= new TblComplainTicket();
            var data= _context.TblComplainTicket.Where(s=>s.Id==Id).First();
            model.Id = data.Id;
            model.TicketNo = data.TicketNo;
            model.TicketDate = data.TicketDate;
            model.DepartmentCode = data.DepartmentCode;
            model.ComplainTypeCode = data.ComplainTypeCode;
            model.SeverityLevelCode = data.SeverityLevelCode;
            model.ComplainDetails= data.ComplainDetails;
            model.Comments = data.Comments;
            var selectedMachineList = _context.TblComplainTicketMachineDetails.Where(s => s.TicketNo == data.TicketNo).ToList();
            ViewBag.SelectedMachine = selectedMachineList.ToList();

            var attachmentList = _context.TblComplainTicketsImageDetails.Where(s => s.TicketNo == data.TicketNo).ToList();
            model.TblComplainTicketsImageDetails= attachmentList.ToList();

            var approverList = from c in _context.TblComplainTicketApproverSmsdetails
                               from e in _context.TblEmployeeSetups
                               from d in _context.TblDepartments
                               from des in _context.TblDesignations
                               where c.ApproverCode == e.EmployeeCode && c.TicketNo == data.TicketNo
                               && e.DepartmentCode == d.DepartmentCode
                               && e.DesignationCode== des.DesignationCode
                               select new TblComplainTicketApproverSmsdetail
                               {
                                   ApproverCode = c.ApproverCode,
                                   ApproverName= e.EmployeeName,
                                   Department=d.DepartmentName,
                                   Designation=des.DesignationName,
                                   Smsto = c.Smsto,
                                   IsSmsreceiver = c.IsSmsreceiver



                               };


            model.TblComplainTicketsApproverSmsdetails =  approverList.ToList();

            var EditPermission = 0;
            if (data.Iuser == User.Identity.Name)
            {
                EditPermission = 1;
            }


            ViewBag.EditPerm = EditPermission;

            return View(model);
        }
      
        public async Task<ActionResult> CreateTicket(TblComplainTicket model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (DoesToolCodeExists(model.BusinessCode))
                    {
                        model.TicketNo = GenerateTicketNo().ToString();
                    }

                    //var buscode = _userbusinessandplant.GetUserWiseBusinessCode();
                    //var plantCode= _userbusinessandplant.GetUserWisePlantCodes();
                    if (model.MachineCode != null)
                    {
                        foreach (var item in model.MachineCode)
                        {
                            TblComplainTicketMachineDetail machine = new TblComplainTicketMachineDetail();
                            machine.TicketNo = model.TicketNo;
                            machine.MachineCode = item;
                            model.TblComplainTicketMachineDetails.Add(machine);
                        }
                    }
                   
                    model.BusinessCode = BusinessCode;
                    model.StatusCode = "New";
                    model.PlantCode = PlantCode;
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;
                    await _context.AddAsync(model);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult> UpdateTicket(TblComplainTicket model)
        {
            var vId = model.Id;
            var ticektNo = model.TicketNo;
            try
            {

                if (DependencyCheck(model.TicketNo))
                {
                    var ticketInfoToUpdate = await _context.TblComplainTicket.FirstOrDefaultAsync(s => s.Id == vId);
                    ticketInfoToUpdate.Edate = DateTime.Now;
                    ticketInfoToUpdate.Euser = User.Identity.Name;
                    if (await TryUpdateModelAsync<TblComplainTicket>(
                        ticketInfoToUpdate,
                        "",
                        s => s.DepartmentCode,
                        s => s.SeverityLevelCode,
                        s => s.ComplainDetails,
                        s => s.Comments,
                        s => s.ComplainTypeCode
                        )) ;
                    _context.TblComplainTicketApproverSmsdetails.RemoveRange(_context.TblComplainTicketApproverSmsdetails.Where(s => s.TicketNo == model.TicketNo));
                    _context.TblComplainTicketMachineDetails.RemoveRange(_context.TblComplainTicketMachineDetails.Where(s => s.TicketNo == model.TicketNo));
                    _context.TblComplainTicketsImageDetails.RemoveRange(_context.TblComplainTicketsImageDetails.Where(s => s.TicketNo == model.TicketNo));
                    foreach (var item in model.TblComplainTicketsApproverSmsdetails)
                    {
                        TblComplainTicketApproverSmsdetail approver = new TblComplainTicketApproverSmsdetail();
                        approver.TicketNo = ticektNo;
                        approver.ApproverCode = item.ApproverCode;
                        approver.Smsto = item.Smsto;
                        approver.IsSmsreceiver = item.IsSmsreceiver;
                        approver.ReceiveDate = item.ReceiveDate;
                        await _context.AddAsync(approver);

                    }

                    foreach (var item in model.TblComplainTicketsImageDetails)
                    {
                        TblComplainTicketImageDetails imgDtl = new TblComplainTicketImageDetails();
                        imgDtl.TicketNo = ticektNo;
                        imgDtl.OriginalFileName = item.OriginalFileName;
                        imgDtl.FileName = item.FileName;
                        imgDtl.Location = item.Location;
                        imgDtl.DocumentType = item.DocumentType;
                        imgDtl.Iuser = User.Identity.Name;
                        imgDtl.Idate = DateTime.Now;
                        await _context.AddAsync(imgDtl);
                    }

                    foreach (var item in model.MachineCode)
                    {
                        TblComplainTicketMachineDetail machine = new TblComplainTicketMachineDetail();
                        machine.TicketNo = ticektNo;
                        machine.MachineCode = item;
                        await _context.AddAsync(machine);
                    }


                    await _context.SaveChangesAsync();


                }
                else { 
                 
                }
            }
            catch (Exception ex)
            {
                throw;
                //ModelState.AddModelError("", "Unable to save changes. " +
                //    "Try again, and if the problem persists, " +
                //    "see your system administrator.");
              
            }
            return RedirectToAction("Index");

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
        private string GenerateTicketNo()
        {

            var result = _context.TblComplainTicket.OrderBy(x => x.Id).Select(x => x.TicketNo).LastOrDefault();
            var lastTicket = string.IsNullOrEmpty(result) ? "00000" : result;


            var last5digits = "1";
            if (lastTicket.Length > 3)
            {
                last5digits = lastTicket.Substring(lastTicket.Length - 3);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D3");

            var generatedCode = $"T-{lastNumberString}";
            return generatedCode;
        }

        //private string GenerateTicketNo() {
        //    //Generate Process No.---------Start
        //    String sDate = DateTime.Now.ToString();
        //    DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
        //    //String dy = datevalue.Day.ToString("00");
        //    String mn = datevalue.Month.ToString("00");
        //    String yy = datevalue.Year.ToString().Substring(2, 2);
        //    var ticketNo = "T-" + yy + mn;
        //    String maxId = "";
        //    String maxNo = (from c in _context.TblComplainTicket.Where(c => c.TicketNo.Substring(0, 8) == ticketNo).OrderByDescending(t => t.Id) select c.ProcessNo.Substring(8, 3)).FirstOrDefault();
        //    if (maxNo == null)
        //    {
        //        maxId = "001";
        //    }
        //    else
        //    {
        //        maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
        //    }
        //    processNo = "T-" + yy + mn + maxId;

        //    //Generate Process No.---------End
        //    return processNo;
        //}
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
        private bool DoesToolCodeExists(string vTicketNo)
        {

            return _context.TblComplainTicket.Any(e => e.TicketNo == vTicketNo);
        }
        private bool DependencyCheck(string vTicketNo) 
        {
            var chechStatus = _context.TblComplainTicket.Where(x=>x.TicketNo==vTicketNo).Select(e => e.StatusCode).FirstOrDefault();
            if (chechStatus != "New")
            {
                return false;
            }
            else {
                return true;
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
    }
   
}
