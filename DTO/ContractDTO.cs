using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ContractDTO
    {
        public int ContractID { get; set; }
        public int ProductID { get; set; }
        public int ProviderID { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime RowInsertTime { get; set; }
        public DateTime RowUpdateTime { get; set; }
    }
}
