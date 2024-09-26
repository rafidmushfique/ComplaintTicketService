using System;
using System.Collections.Generic;

namespace LILI_TMS.Temp_Models
{
    public partial class TblMachineSetup
    {
        public TblMachineSetup()
        {
            TblComplainTicketMachineDetails = new HashSet<TblComplainTicketMachineDetail>();
        }

        public int Id { get; set; }
        public string MachineCode { get; set; }
        public string MachineName { get; set; }
        public DateTime InstallationDate { get; set; }
        public string DepartmentCode { get; set; }
        public string MachineDetails { get; set; }
        public string Comments { get; set; }
        public string BusinessCode { get; set; }
        public string PlantCode { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string ModelNumber { get; set; }
        public string OriginCountry { get; set; }
        public string Manufacturer { get; set; }

        public virtual TblDepartment DepartmentCodeNavigation { get; set; }
        public virtual ICollection<TblComplainTicketMachineDetail> TblComplainTicketMachineDetails { get; set; }
    }
}
