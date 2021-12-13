using AppDependencies;
using BLL.Interfaces;
using DTO;
using LOG;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_MVVM.Command;

namespace WPF_MVVM.ViewModels
{
    public class WarehouseManagerViewModel : BaseViewModel, IPageViewModel
    {
        protected ILogger _log = new Logger(typeof(WarehouseManagerViewModel));

        private readonly IWarehouseManager _ware_manager;
        private ObservableCollection<ProductDTO> _products;
        private ObservableCollection<OrderDTO> _orders;
        private ObservableCollection<ContractDTO> _contracts;
        private ICommand _backToLogIn;
        private ICommand _nextToUpdateOrder;
        private ICommand _nextToCreateContract;
        private ICommand _deactivateOrder;
        private ICommand _deactivateContract;
        private ICommand _sortProducts;
        private ICommand _findProducts;
        private string _idOrder;
        private string _idContract;
        private string _selectedOptionForSort;
        private string _selectedOptionForFind;
        private string _textForFind;
        public IWarehouseManager WareManager
        {
            get
            {
                return _ware_manager;
            }
        }
        public string IDOrder
        {
            get
            {
                return _idOrder;
            }
            set
            {
                _idOrder = value;
                this.OnPropertyChanged(nameof(IDOrder));
            }
        }
        public string IDContract
        {
            get
            {
                return _idContract;
            }
            set
            {
                _idContract = value;
                this.OnPropertyChanged(nameof(IDContract));
            }
        }

        public string SelectedOptionForSort
        {
            get
            {
                return _selectedOptionForSort;
            }
            set
            {
                _selectedOptionForSort = value.ToString();
                this.OnPropertyChanged(nameof(SelectedOptionForSort));
            }
        }
        public string SelectedOptionForFind
        {
            get
            {
                return _selectedOptionForFind;
            }
            set
            {
                _selectedOptionForFind = value.ToString();
                this.OnPropertyChanged(nameof(SelectedOptionForFind));
            }
        }

        public string TextForFind
        {
            get
            {
                return _textForFind;
            }
            set
            {
                _textForFind = value.ToString();
                this.OnPropertyChanged(nameof(TextForFind));
            }
        }
        public List<string> Elements
        {
            get 
            {
                return new List<string>() { "Name", "Categories", "Quantity", "Update time", "Default" };
            }
        }

        public List<string> ElementsForFind
        {
            get
            {
                return new List<string>() { "Name", "Category", "Price"};
            }
        }
        public ICommand NextToUpdateContract
        {
            get
            {
                return _nextToUpdateOrder;
            }
            set
            {
                _nextToUpdateOrder = value;
                this.OnPropertyChanged(nameof(NextToUpdateContract));
            }
        }
        public ICommand NextToCreateContract
        {
            get
            {
                return _nextToCreateContract;
            }
            set
            {
                _nextToCreateContract = value;
                this.OnPropertyChanged(nameof(NextToCreateContract));
            }
        }
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
        public ICommand DeactivateOrder
        {
            get
            {
                return _deactivateOrder;
            }
            set
            {
                _deactivateOrder = value;
                this.OnPropertyChanged(nameof(DeactivateOrder));
            }
        }
        public ICommand DeactivateContract
        {
            get
            {
                return _deactivateContract;
            }
            set
            {
                _deactivateContract = value;
                this.OnPropertyChanged(nameof(DeactivateContract));
            }
        }

        public ICommand SortProducts
        {
            get
            {
                return _sortProducts;
            }
            set
            {
                _sortProducts = value;
                this.OnPropertyChanged(nameof(SortProducts));
            }
        }

        public ICommand FindProducts
        {
            get
            {
                return _findProducts;
            }
            set
            {
                _findProducts = value;
                this.OnPropertyChanged(nameof(FindProducts));
            }
        }

        public void BackStep(object obj)
        {
            Mediator.Notify("GoToLogInScreen", "");
            _log.Info("GoToLogInScreen");
        }

        public void NextStepUpdate(object obj)
        {
             Mediator.Notify("GoToUpdateOrderScreen", "");
            _log.Info("GoToUpdateOrderScreen");
        }
        public void NextStepCreate(object obj)
        {
            Mediator.Notify("GoToCreateContractScreen", "");
            _log.Info("GoToCreateContractScreen");
        }
        public ObservableCollection<ProductDTO> Products
        {
            get
            {
                return _products;
            }
            set
            {
                _products = value;
                this.OnPropertyChanged(nameof(Products));
            }
        }
        public ObservableCollection<OrderDTO> Orders
        {
            get
            {
                return _orders;
            }
            set 
            {
                _orders = value;
                this.OnPropertyChanged(nameof(Orders));
            }
        }
        public ObservableCollection<ContractDTO> Contracts
        {
            get
            {
                return _contracts;
            }
            set
            {
                _contracts = value;
                this.OnPropertyChanged(nameof(Contracts));
            }
        }

        private void DeactivateOrderEvent(object obj)
        {
            int value = Int32.Parse(IDOrder);
            _ware_manager.DeactivateOrderById(value);
            Orders = new ObservableCollection<OrderDTO>(_ware_manager.ShowActiveOrders());
            _log.Info("Deactivating Order");
        }

        private void DeactivateContractEvent(object obj)
        {
            int value = Int32.Parse(IDContract);
            _ware_manager.DeactivateContractById(value);
            Contracts = new ObservableCollection<ContractDTO>(_ware_manager.ShowActiveContracts());
            _log.Info("Deactivating Contract");
        }

        private void SortProductsEvent(object obj)
        {
            switch (SelectedOptionForSort) 
            {
                case "Name":
                    Products = new ObservableCollection<ProductDTO>(_ware_manager.SortProductsByName());
                    break;
                case "Categories":
                    Products = new ObservableCollection<ProductDTO>(_ware_manager.SortProductsByCategory());
                    break;
                case "Quantity":
                    Products = new ObservableCollection<ProductDTO>(_ware_manager.SortProductsByQuantity());
                    break;
                case "Update time":
                    Products = new ObservableCollection<ProductDTO>(_ware_manager.SortProductsByUpdateDate());
                    break;
                default:
                    Products = new ObservableCollection<ProductDTO>(_ware_manager.ShowAllProducts());
                    break;
            }
            _log.Info("Sorting Products");
        }
        private void FindProductsEvent(object obj)
        {
            switch (SelectedOptionForFind)
            {
                case "Name":
                    Products = new ObservableCollection<ProductDTO>(_ware_manager.FindProductsByName(TextForFind));
                    break;
                case "Category":
                    Products = new ObservableCollection<ProductDTO>(_ware_manager.FindProductsByCategories(Int32.Parse(TextForFind)));
                    break;
                case "Price":
                    Products = new ObservableCollection<ProductDTO>(_ware_manager.FindProductsByPrice(Int32.Parse(TextForFind)));
                    break;
                default:
                    Products = new ObservableCollection<ProductDTO>(_ware_manager.ShowAllProducts());
                    break;
            }
            _log.Info("FindingProducts");
        }

        public WarehouseManagerViewModel(Dependencies d)
        {
            _ware_manager = d.ware_manager;
            _products = new ObservableCollection<ProductDTO>(_ware_manager.ShowAllProducts());
            _orders = new ObservableCollection<OrderDTO>(_ware_manager.ShowActiveOrders());
            _contracts = new ObservableCollection<ContractDTO>(_ware_manager.ShowActiveContracts());

            BackToLogIn = new RelayCommand(BackStep);
            NextToCreateContract = new RelayCommand(NextStepCreate);
            NextToUpdateContract = new RelayCommand(NextStepUpdate);

            Mediator.Subscribe("DeactivateOrderEvent", DeactivateOrderEvent);
            Mediator.Subscribe("DeactivateContractEvent", DeactivateContractEvent);
            Mediator.Subscribe("SortProductsEvent", SortProductsEvent);
            Mediator.Subscribe("FindProductsEvent", FindProductsEvent);

            DeactivateOrder = new RelayCommand(x =>
            {
                Mediator.Notify("DeactivateOrderEvent", "");
            });
            DeactivateContract = new RelayCommand(x =>
            {
                Mediator.Notify("DeactivateContractEvent", "");
            });

            SortProducts = new RelayCommand(x =>
            {
                Mediator.Notify("SortProductsEvent", "");
            });

            FindProducts = new RelayCommand(x =>
            {
                Mediator.Notify("FindProductsEvent", "");
            });
        }
    }
}
