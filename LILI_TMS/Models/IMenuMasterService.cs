using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LILI_TMS.Models
{
    public interface IMenuMasterService
    {
        IEnumerable<MenuMaster> GetMenuMaster();
        IEnumerable<MenuMaster> GetMenuMaster(String UserRole);
    }
}
