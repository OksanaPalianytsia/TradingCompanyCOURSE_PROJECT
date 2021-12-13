using BLL.Concrete;
using DAL.ADO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDependencies
{
    public class Dependencies
    {
        static ProductDAL productDAL = new ProductDAL(ConfigurationManager.ConnectionStrings["Shop"].ConnectionString);
        static OrderDAL orderDAL = new OrderDAL(ConfigurationManager.ConnectionStrings["Shop"].ConnectionString);
        static ContractDAL contractDAL = new ContractDAL(ConfigurationManager.ConnectionStrings["Shop"].ConnectionString);
        static ProviderDAL providerDAL = new ProviderDAL(ConfigurationManager.ConnectionStrings["Shop"].ConnectionString);
        public readonly WarehouseManager ware_manager = new WarehouseManager(productDAL, orderDAL, contractDAL, providerDAL);
        public readonly UserManager user_manager = new UserManager(productDAL);

        static UserDAL user = new UserDAL(ConfigurationManager.ConnectionStrings["Shop"].ConnectionString);

        public AuthorisationManager auth_man = new AuthorisationManager(user);
    }
}
