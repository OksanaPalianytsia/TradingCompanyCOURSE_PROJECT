using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IProductDAL
    {
        void DeleteProduct(int productID);
        void UpdateProduct(int productID, string name, int categoryID, int price, int quantity);
        void CreateProduct(string name, int categoryID, int price, int quantity);
        List<ProductDTO> GetAllProducts();
        ProductDTO GetProductById(int itemId);
        List<ProductDTO> SortProductsByName();
        List<ProductDTO> SortProductsByCategory();
        List<ProductDTO> SortProductsByQuantity();
        List<ProductDTO> SortProductsByUpdateDate();
        List<ProductDTO> GetProductsByCategories(int categoryId);
        List<ProductDTO> GetProductsByName(string name);
        List<ProductDTO> GetProductsByPrice(int price);
        void AddProductSupplies(int productID, int value);
        void ExcludeProductSupplies(int productID, int value);
    }
}
