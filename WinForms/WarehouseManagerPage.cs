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

    
    public partial class WarehouseManagerPage : Form
    {
        public IWarehouseManager _ware_manager;
        private List<ProductDTO> _products;
        private List<OrderDTO> _orders;
        private List<ContractDTO> _contracts;

        protected ILogger _log = new Logger(typeof(WarehouseManagerPage));
        public WarehouseManagerPage(IWarehouseManager ware_manager)
        {
            InitializeComponent();
            _ware_manager = ware_manager;
            RefreshDataProducts();
            RefreshDataContracts();
            RefreshDataOrders();
            toolStripTextBoxWHMProductsFind.Text = "default";
            _log.Info("Warehouse Manager Page loaded");
        }
        private void RefreshDataProducts()
        {
            _products = _ware_manager.ShowAllProducts();

            BindingList<ProductDTO> blProducts = new BindingList<ProductDTO>(_products);
            bindingSourceWHMProducts.DataSource = blProducts;

            bindingNavigatorWHMProducts.BindingSource = bindingSourceWHMProducts;
            dataGridViewWHMProducts.DataSource = bindingSourceWHMProducts;
            
        }
        public void RefreshDataOrders()
        {
            _orders = _ware_manager.ShowActiveOrders();

            BindingList<OrderDTO> blOrders = new BindingList<OrderDTO>(_orders);
            bindingSourceWHMOrders.DataSource = blOrders;

            bindingNavigatorWHMOrders.BindingSource = bindingSourceWHMOrders;
            dataGridViewWHMOrders.DataSource = bindingSourceWHMOrders;

        }
        public void RefreshDataContracts()
        {
            _contracts = _ware_manager.ShowActiveContracts();

            BindingList<ContractDTO> blContracts = new BindingList<ContractDTO>(_contracts);
            bindingSourceWHMContracts.DataSource = blContracts;

            bindingNavigatorWHMContracts.BindingSource = bindingSourceWHMContracts;
            dataGridViewWHMContrcts.DataSource = bindingSourceWHMContracts;
        }

        private void SortDataProductsByName()
        {
            _products = _ware_manager.SortProductsByName();

            BindingList<ProductDTO> blProducts = new BindingList<ProductDTO>(_products);
            bindingSourceWHMProducts.DataSource = blProducts;

            bindingNavigatorWHMProducts.BindingSource = bindingSourceWHMProducts;
            dataGridViewWHMProducts.DataSource = bindingSourceWHMProducts;
        }
        private void SortDataProductsByCategories()
        {
            _products = _ware_manager.SortProductsByCategory();

            BindingList<ProductDTO> blProducts = new BindingList<ProductDTO>(_products);
            bindingSourceWHMProducts.DataSource = blProducts;

            bindingNavigatorWHMProducts.BindingSource = bindingSourceWHMProducts;
            dataGridViewWHMProducts.DataSource = bindingSourceWHMProducts;
        }
        private void SortDataProductsByQuantity()
        {
            _products = _ware_manager.SortProductsByQuantity();

            BindingList<ProductDTO> blProducts = new BindingList<ProductDTO>(_products);
            bindingSourceWHMProducts.DataSource = blProducts;

            bindingNavigatorWHMProducts.BindingSource = bindingSourceWHMProducts;
            dataGridViewWHMProducts.DataSource = bindingSourceWHMProducts;
        }
        private void SortDataProductsByUpdateTime()
        {
            _products = _ware_manager.SortProductsByUpdateDate();

            BindingList<ProductDTO> blProducts = new BindingList<ProductDTO>(_products);
            bindingSourceWHMProducts.DataSource = blProducts;

            bindingNavigatorWHMProducts.BindingSource = bindingSourceWHMProducts;
            dataGridViewWHMProducts.DataSource = bindingSourceWHMProducts;
        }

        private void FindDataProductsByName(string name)
        {
            _products = _ware_manager.FindProductsByName(name);

            BindingList<ProductDTO> blProducts = new BindingList<ProductDTO>(_products);
            bindingSourceWHMProducts.DataSource = blProducts;

            bindingNavigatorWHMProducts.BindingSource = bindingSourceWHMProducts;
            dataGridViewWHMProducts.DataSource = bindingSourceWHMProducts;
        }

        private void FindDataProductsByCategory(int categoryID)
        {
            _products = _ware_manager.FindProductsByCategories(categoryID);

            BindingList<ProductDTO> blProducts = new BindingList<ProductDTO>(_products);
            bindingSourceWHMProducts.DataSource = blProducts;

            bindingNavigatorWHMProducts.BindingSource = bindingSourceWHMProducts;
            dataGridViewWHMProducts.DataSource = bindingSourceWHMProducts;
        }

        private void FindDataProductsByPrice(int price)
        {
            _products = _ware_manager.FindProductsByPrice(price);

            BindingList<ProductDTO> blProducts = new BindingList<ProductDTO>(_products);
            bindingSourceWHMProducts.DataSource = blProducts;

            bindingNavigatorWHMProducts.BindingSource = bindingSourceWHMProducts;
            dataGridViewWHMProducts.DataSource = bindingSourceWHMProducts;
        }

        public void CreateContract(int productID, int providerID, int quantity, bool isActive = true)
        {
            _ware_manager.CreateContract(productID, providerID, quantity, isActive);
        }

        public void UpdateOrder(int orderID, int productID, int quantity, bool isActive)
        {
            _ware_manager.UpdateOrder(orderID, productID, quantity, isActive);
        }

        private void toolStripComboBoxWHMProductsSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolStripComboBoxWHMProductsSort.SelectedIndex)
            {
                case 0:
                    SortDataProductsByName();
                    break;
                case 1:
                    SortDataProductsByCategories();
                    break;
                case 2:
                    SortDataProductsByQuantity();
                    break;
                case 3:
                    SortDataProductsByUpdateTime();
                    break;
                case 4:
                    RefreshDataProducts();
                    break;
            }
        }

        private void toolStripTextBoxWHMProductsFind_TextChanged(object sender, EventArgs e)
        {
            string property_value = toolStripTextBoxWHMProductsFind.Text;

            switch (toolStripComboBoxWHMProductsFind.SelectedIndex)
            {
                case 0:
                    FindDataProductsByName(property_value);
                    break;
                case 1:
                    if (property_value.Length != 0)
                    {
                        FindDataProductsByCategory(Int32.Parse(property_value));
                    }
                    break;
                case 2:
                    if (property_value.Length != 0)
                    {
                        FindDataProductsByPrice(Int32.Parse(property_value));
                    }
                    break;
                default:
                    RefreshDataProducts();
                    break;

            }
        }

        private void dataGridViewWHMOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int value = Int32.Parse(dataGridViewWHMOrders.Rows[e.RowIndex].Cells[0].Value.ToString());
            _ware_manager.DeactivateOrderById(value);
            RefreshDataOrders();
            RefreshDataProducts();
        }

        private void dataGridViewWHMContrcts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int value = Int32.Parse(dataGridViewWHMContrcts.Rows[e.RowIndex].Cells[0].Value.ToString());
            _ware_manager.DeactivateContractById(value);
            RefreshDataContracts();
            RefreshDataProducts();
        }

        private void toolStripButtonWHMOrdersUpdate_Click(object sender, EventArgs e)
        {
            UpdateOrder form_Order = new UpdateOrder(this);
            form_Order.Show();
            _log.Info(" Go 'Update Order' Page");
        }

        private void toolStripButtonWHMContractsAdd_Click(object sender, EventArgs e)
        {
            AddNewContract form_Contract = new AddNewContract(this);
            form_Contract.Show();
            _log.Info(" Go 'Add Contract' Page");
        }
    }
}
