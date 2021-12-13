using AppDependencies;
using BLL.Interfaces;
using DTO;
using LOG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_MVVM.Command;

namespace WPF_MVVM.ViewModels
{
    public class UserViewModel : BaseViewModel, IPageViewModel
    {
        protected ILogger _log = new Logger(typeof(UserViewModel));

        private ICommand _backToLogIn;
        private readonly IUserManager _user;
        private List<ProductDTO> _products;

        public ICommand BackToLogIn
        {
            get
            {
                return _backToLogIn;
            }
            set
            {
                _backToLogIn = value;
                this.OnPropertyChanged(nameof(BackToLogIn));
            }
        }
        public List<ProductDTO> Products
        {
            get
            {
                return _products;
            }
        }

        public void NextStep(object obj)
        {
            Mediator.Notify("GoToLogInScreen", "");
            _log.Info("GoToLogInScreen");
        }

        public UserViewModel(Dependencies d)
        {
            _user = d.user_manager;
            _products = _user.ShowAllProducts();
            BackToLogIn = new RelayCommand(NextStep);
        }
    }
}
