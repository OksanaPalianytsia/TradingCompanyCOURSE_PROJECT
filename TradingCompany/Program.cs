using BLL.Concrete;
using DAL.ADO;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms;

namespace TradingCompany
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            
            UserDAL user = new UserDAL(ConfigurationManager.ConnectionStrings["Shop"].ConnectionString);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AuthorisationManager auth_man = new AuthorisationManager(user);
            Application.Run(new LogInForm(auth_man));
        }
    }
}
