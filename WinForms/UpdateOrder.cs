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
    public partial class UpdateOrder : Form
    {
        IWarehouseManager manag_3_form;
        WarehouseManagerPage form_settings;
        private List<OrderDTO> _orders;
        private List<ProductDTO> _products;

        protected ILogger _log = new Logger(typeof(UpdateOrder));
        public UpdateOrder(WarehouseManagerPage form)
        {
            InitializeComponent();
            form_settings = form;
            manag_3_form = form._ware_manager;
            RefreshOrders();
            RefreshProducts();
            _log.Info("'UpdateOrder' Page loaded");
        }
        private void RefreshOrders()
        {
            _orders = manag_3_form.ShowActiveOrders();
            dataGridViewUpdateOrdersOrders.DataSource = _orders;
        }
        private void RefreshProducts()
        {
            _products = manag_3_form.ShowAllProducts();
            dataGridViewUpdateOrdersProducts.DataSource = _products;
        }

        private void buttonUpdateOrderCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            _log.Info("Canceled updating");
        }

        private void buttonUpdateOrderSave_Click(object sender, EventArgs e)
        {
            int order_id = Int32.Parse(textBoxUpdateOrderOrderID.Text);
            int prod_id = Int32.Parse(textBoxUpdateOrderProductID.Text);
            int quantity = Int32.Parse(textBoxUpdateOrderQuantity.Text);
            bool is_active = Boolean.Parse(textBoxUpdateOrderIsActive.Text.ToString());
            form_settings.UpdateOrder(order_id, prod_id, quantity, is_active);
            form_settings.RefreshDataOrders();
            RefreshOrders();
            _log.Info("Updating existing order");
        }
    }
}
