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
    public partial class AddNewContract : Form
    {
        IWarehouseManager manag_2_form;
        WarehouseManagerPage form_settings;
        private List<ProductDTO> _products;
        private List<ProviderDTO> _providers;

        protected ILogger _log = new Logger(typeof(AddNewContract));
        public AddNewContract(WarehouseManagerPage form)
        {
            InitializeComponent();
            form_settings = form;
            manag_2_form = form._ware_manager;
            RefreshProducts();
            RefreshProviders();
            _log.Info("'AddNewContract' Page loaded");
        }
        private void RefreshProducts()
        {
            _products = manag_2_form.ShowAllProducts();
            dataGridViewAddContractProducts.DataSource = _products;
        }
        private void RefreshProviders()
        {
            _providers = manag_2_form.ShowAllProviders();
            dataGridViewAddContractProviders.DataSource = _providers;
        }

        private void buttonAddContractSave_Click(object sender, EventArgs e)
        {
            int prod_id = Int32.Parse(textBoxAddContractProductID.Text);
            int provider_id = Int32.Parse(textBoxAddContractQuantity.Text);
            int quantity = Int32.Parse(textBoxAddContractQuantity.Text);
            form_settings.CreateContract(prod_id, provider_id, quantity);
            form_settings.RefreshDataContracts();
            this.Close();
            _log.Info("Adding new contract");
        }

        private void buttonAddContractCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            _log.Info("Canceled adding");
        }
    }
}
