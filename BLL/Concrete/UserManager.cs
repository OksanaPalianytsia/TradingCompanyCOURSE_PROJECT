using BLL.Interfaces;
using DAL.Interfaces;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Concrete
{
    public class UserManager : IUserManager
    {
        private readonly IProductDAL _productDal;
        public UserManager(IProductDAL productDal)
        {
            _productDal = productDal;
        }
        public List<ProductDTO> ShowAllProducts()
        {
            return _productDal.GetAllProducts();
        }
    }
}
