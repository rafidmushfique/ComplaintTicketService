
using System;
using System.Collections.Generic;

namespace LILI_TMS.Models
{
    public interface IUserWiseBusinessAndPlantService
    {
        IEnumerable<TblUserWiseBusinessAndPlantCode> GetUserWiseBusinessAndPlantCodes();
        string GetUserWiseBusinessCode();
        string GetUserWisePlantCodes();
    }
}
