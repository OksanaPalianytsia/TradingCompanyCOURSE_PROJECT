using AppDependencies;
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
        private LogInViewModel _loginVM;

        protected ILogger _log = new Logger(typeof(LogInForm));
        public LogInForm(LogInViewModel log)
        {
            InitializeComponent();
            _loginVM = log;
            textBoxLIFPassword.PasswordChar = '\u2022';
            _log.Info("'Log IN'  Page loaded");
        }

        private void buttonLIFLogIn_Click(object sender, EventArgs e)
        {
            string login = textBoxLIFUsername.Text;
            string password = textBoxLIFPassword.Text;
            bool isUserLoggedIn = _loginVM.LoginFunc(login, password);
            HandleLogInResult(isUserLoggedIn);           
        }

        private void HandleLogInResult(bool isUserLoggedIn) 
        {
            if (isUserLoggedIn)
            {
                DisplayWarehouseManagerPage();
            }
            else
            {
                DisplayUserPage();
            }
        }

        private void DisplayWarehouseManagerPage() 
        {
            WarehouseManagerPage form_m = new WarehouseManagerPage(_loginVM.dependencies.ware_manager);
            form_m.Show();
            _log.Info("Go to 'WarehouseManagerPage'");
        }

        private void DisplayUserPage()
        {
            UserPage form_Ur = new UserPage(_loginVM.dependencies.user_manager);
            form_Ur.Show();
            _log.Info("Go to 'UserPage'");
        }

        public class LogInViewModel
        {
            private readonly AuthorisationManager _auth_m;
            public readonly Dependencies dependencies;
            public LogInViewModel(Dependencies d) {
                dependencies = d;
                _auth_m = d.auth_man;

            }

            public bool LoginFunc(string login, string password)
            {
                return _auth_m.Login(login, password);
            }                       
        }
    }
}
