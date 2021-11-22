using BLL.Concrete;
using DAL.ADO;
using LOG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    public partial class LogInForm : Form
    {
        static ProductDAL productDAL = new ProductDAL(ConfigurationManager.ConnectionStrings["Shop"].ConnectionString);
        static OrderDAL orderDAL = new OrderDAL(ConfigurationManager.ConnectionStrings["Shop"].ConnectionString);
        static ContractDAL contractDAL = new ContractDAL(ConfigurationManager.ConnectionStrings["Shop"].ConnectionString);
        static ProviderDAL providerDAL = new ProviderDAL(ConfigurationManager.ConnectionStrings["Shop"].ConnectionString);
        private readonly WarehouseManager ware_manager = new WarehouseManager(productDAL, orderDAL, contractDAL, providerDAL);
        private readonly UserManager user_manager = new UserManager(productDAL);

        private readonly AuthorisationManager _auth_m;
        private string login;
        private string password;

        protected ILogger _log = new Logger(typeof(LogInForm));
        public LogInForm(AuthorisationManager auth_m)
        {
            InitializeComponent();
            _auth_m = auth_m;
            textBoxLIFPassword.PasswordChar = '\u2022';
            _log.Info("'Log IN'  Page loaded");
        }

        private void buttonLIFLogIn_Click(object sender, EventArgs e)
        {
            login = textBoxLIFUsername.Text;
            password = textBoxLIFPassword.Text;
            bool result = _auth_m.Login(login, password);
            switch (result)
            {
                case false:
                    UserPage form_Ur = new UserPage(user_manager);
                    form_Ur.Show();
                    _log.Info("Go to 'UserPage'");
                    break;
                case true:
                    WarehouseManagerPage form_m = new WarehouseManagerPage(ware_manager);
                    form_m.Show();
                    _log.Info("Go to 'WarehouseManagerPage'");
                    break;
            }
        }
    }
}
