using LILI_TMS.Data;
using LILI_TMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LILI_TMS.Services
{
    public class MenuMasterService:IMenuMasterService
    {
        private readonly dbTicketManagementContext _dbContext;
        //dbEFTestContext _dbContext = new dbEFTestContext();

        public MenuMasterService(dbTicketManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<MenuMaster> GetMenuMaster()
        {
            return _dbContext.MenuMaster.AsEnumerable();
        }

        public IEnumerable<MenuMaster> GetMenuMaster(string UserRole)
        {
            //var result = _dbContext.MenuMaster.Where(m => m.UserRoll == UserRole).ToList();
            var result = _dbContext.MenuMaster.Where(m => m.UserRoll == UserRole).OrderBy(m=>m.MenuIdentity).ToList();
            return result;
        }
    }
}
