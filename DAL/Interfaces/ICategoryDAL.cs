using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICategoryDAL
    {
        void DeleteCategory(int categoryID);
        void UpdateCategory(int categoryID, string name);
        void CreateCategory(string name);
        List<CategoryDTO> GetAllCategories();
        CategoryDTO GetCategoryById(int itemId);
    }
}
