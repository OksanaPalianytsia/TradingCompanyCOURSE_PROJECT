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
using static WinForms.LogInForm;
using AppDependencies;
using System.Windows;

namespace TradingCompany
{
    class Program
    {
        //[STAThread]
        static void Main(string[] args)
        {
            // RunApplication();
        }

        //private static void RunApplication()
        //{
        //    var application = new App();
        //    application.Run();
        //}
    }

    //class Program
    //{
    //    [STAThread]
    //    static void Main(string[] args)
    //    {



    //        Application.EnableVisualStyles();
    //        Application.SetCompatibleTextRenderingDefault(false);

    //        Dependencies d = new Dependencies();


    //        LogInViewModel linvw = new LogInViewModel(d);


    //        Application.Run(new LogInForm(linvw));
    //    }


    //}
}
