using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IProviderDAL
    {
        void DeleteProvider(int providerID);
        void UpdateProvider(int providerID, string name);
        void CreateProvider(string name);
        List<ProviderDTO> GetAllProviders();
        ProviderDTO GetProviderById(int providerID);
    }
}
