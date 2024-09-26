using LILI_TMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
namespace LILI_TMS.Services
{
    public class UserWiseBusinessAndPlantService : IUserWiseBusinessAndPlantService
    {
        private readonly dbTicketManagementContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserWiseBusinessAndPlantService(dbTicketManagementContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IEnumerable<TblUserWiseBusinessAndPlantCode> GetUserWiseBusinessAndPlantCodes() {
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            var model = from c in _context.TblUserWiseBusinessAndPlantCodes
                        where c.UserId == userName
                        select new TblUserWiseBusinessAndPlantCode
                        {
                            UserId = c.UserId,
                            BusinessCode = c.BusinessCode,
                            PlantCode = c.PlantCode,
                        };
            return model;
        }
        public string GetUserWiseBusinessCode()=> (_context.TblUserWiseBusinessAndPlantCodes
                                                    .Where(s => s.UserId == (_httpContextAccessor.HttpContext.User.Identity.Name))
                                                    .Select(i => i.BusinessCode)).FirstOrDefault().ToString();

        public string GetUserWisePlantCodes() => (_context.TblUserWiseBusinessAndPlantCodes
                                                    .Where(s => s.UserId == (_httpContextAccessor.HttpContext.User.Identity.Name))
                                                    .Select(i => i.PlantCode)).FirstOrDefault().ToString();

    }
    }

