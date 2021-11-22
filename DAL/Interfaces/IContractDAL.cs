using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IContractDAL
    {
        void DeleteContract(int contractID);
        void UpdateContract(int contractID, int productID, int providerID, int quantity, bool isActive);
        void CreateContract(int productID, int providerID, int quantity);
        void DeActivateContractById(int contractId);
        List<ContractDTO> GetAllContracts();
        List<ContractDTO> GetActiveContracts();
        ContractDTO GetContractById(int itemId);
    }
}
