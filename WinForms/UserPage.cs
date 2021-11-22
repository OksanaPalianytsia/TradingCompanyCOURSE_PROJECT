using BLL.Interfaces;
using DTO;
using LOG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    public partial class UserPage : Form
    {
        public IUserManager _user_manager;
        private List<ProductDTO> _products;

        protected ILogger _log = new Logger(typeof(UserPage));
        public UserPage(IUserManager user_manager)
        {
            InitializeComponent();
            _user_manager = user_manager;
            RefreshData();
            _log.Info("'UserPage' loaded");
        }

        private void RefreshData()
        {
            _products = _user_manager.ShowAllProducts();

            BindingList<ProductDTO> blProducts = new BindingList<ProductDTO>(_products);
            bindingSourceUPProducts.DataSource = blProducts;

            bindingNavigatorUPProducts.BindingSource = bindingSourceUPProducts;
            dataGridViewUPProducts.DataSource = bindingSourceUPProducts;
        }
    }
}
