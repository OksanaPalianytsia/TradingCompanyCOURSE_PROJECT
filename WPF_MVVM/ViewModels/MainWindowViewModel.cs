using AppDependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_MVVM.Command;

namespace WPF_MVVM.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private IPageViewModel _currentPageViewModel;
        private Dependencies _d;
        private List<IPageViewModel> _pageViewModels;
        private LogInViewModel _lvm;
        private UserViewModel _uvm;
        private WarehouseManagerViewModel _wvm;
        private UpdateOrderViewModel _upvm;
        private CreateContractViewModel _ccvm;

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                _currentPageViewModel = value;
                OnPropertyChanged("CurrentPageViewModel");
            }
        }

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        private void GoToLogInScreen(object obj)
        {
            ChangeViewModel(PageViewModels[0]);
        }

        private void GoToUserScreen(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
        }
        private void GoToManagerScreen(object obj)
        {
            ChangeViewModel(PageViewModels[2]);
        }

        private void GoToUpdateOrderScreen(object obj)
        {
            ChangeViewModel(PageViewModels[3]);
        }
        private void GoToCreateContractScreen(object obj)
        {
            ChangeViewModel(PageViewModels[4]);
        }
        public MainWindowViewModel(Dependencies d)
        {
            _d = d;
            _lvm = new LogInViewModel(_d);
            _uvm = new UserViewModel(_d);
            _wvm = new WarehouseManagerViewModel(_d);
            _upvm = new  UpdateOrderViewModel(_wvm);
            _ccvm = new CreateContractViewModel(_wvm);
            // Add available pages and set page
            PageViewModels.Add(_lvm);
            PageViewModels.Add(_uvm);
            PageViewModels.Add(_wvm);
            PageViewModels.Add(_upvm);
            PageViewModels.Add(_ccvm);

            CurrentPageViewModel = PageViewModels[0];

            Mediator.Subscribe("GoToLogInScreen", GoToLogInScreen);
            Mediator.Subscribe("GoToUserScreen", GoToUserScreen);
            Mediator.Subscribe("GoToManagerScreen", GoToManagerScreen);
            Mediator.Subscribe("GoToUpdateOrderScreen", GoToUpdateOrderScreen);
            Mediator.Subscribe("GoToCreateContractScreen", GoToCreateContractScreen);
        }
    }
}
