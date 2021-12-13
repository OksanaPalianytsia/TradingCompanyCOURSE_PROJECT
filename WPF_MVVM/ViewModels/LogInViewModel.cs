using AppDependencies;
using BLL.Interfaces;
using LOG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_MVVM.Command;

namespace WPF_MVVM.ViewModels
{
    public class LogInViewModel : BaseViewModel, IPageViewModel, INotifyPropertyChanged
    {
        protected ILogger _log = new Logger(typeof(LogInViewModel));

        private string _userName;
        private ICommand _nextScreen;
        private readonly IAuthorisationManager _auth;

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                this.OnPropertyChanged(nameof(UserName));
            }
        }
        public string Password
        {
            private get; set;
        }

        public bool Login()
        {
            return _auth.Login(UserName, Password);
        }
        public ICommand NextScreen
        {
            get
            {
                return _nextScreen;
            }
            set
            {
                _nextScreen = value;
                this.OnPropertyChanged(nameof(NextScreen));
            }
        }

        public void NextStep(object obj)
        {
            if (Login())
            {
                Mediator.Notify("GoToManagerScreen", "");
                _log.Info("GoToManagerScreen");
            }
            else
            {
                Mediator.Notify("GoToUserScreen", "");
                _log.Info("GoToUserScreen");
            }
        }

        public LogInViewModel(Dependencies d)
        {
            _auth = d.auth_man;
            NextScreen = new RelayCommand(NextStep);
        }

    }
}
