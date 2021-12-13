using BLL.Interfaces;
using DTO;
using LOG;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_MVVM.Command;
using WPF_MVVM.Models;

namespace WPF_MVVM.ViewModels
{
    public class CreateContractViewModel : BaseViewModel, IPageViewModel
    {
        protected ILogger _log = new Logger(typeof(CreateContractViewModel));

        private WarehouseManagerViewModel _ware_model;
        private readonly IWarehouseManager _ware_manager;
        private ObservableCollection<ProductDTO> _products;
        private ObservableCollection<ProviderDTO> _providers;
        private ICommand _backToManagerView;
        private ICommand _createContract;
        private ContractWPF _contract;

        public ContractWPF Contract
        {
            get
            {
                return _contract;
            }
            set
            {
                _contract = value;
                this.OnPropertyChanged(nameof(Contract));
            }
        }
        public string ProviderID
        {
            get
            {
                return Contract.ProviderID;
            }
            set
            {
                Contract.ProviderID = value;
                this.OnPropertyChanged(nameof(ProviderID));
            }
        }
        public string ProductID
        {
            get
            {
                return Contract.ProductID;
            }
            set
            {
                Contract.ProductID = value;
                this.OnPropertyChanged(nameof(ProductID));
            }
        }
        public string Quantity
        {
            get
            {
                return Contract.Quantity;
            }
            set
            {
                Contract.Quantity = value;
                this.OnPropertyChanged(nameof(Quantity));
            }
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
        public ObservableCollection<ProviderDTO> Providers
        {
            get
            {
                return _providers;
            }
            set
            {
                _providers = value;
                this.OnPropertyChanged(nameof(Providers));
            }
        }

        public ICommand BackToManagerView
        {
            get
            {
                return _backToManagerView;
            }
            set
            {
                _backToManagerView = value;
                this.OnPropertyChanged(nameof(BackToManagerView));
            }
        }

        public ICommand CreateContract
        {
            get
            {
                return _createContract;
            }
            set
            {
                _createContract = value;
                this.OnPropertyChanged(nameof(CreateContract));
            }
        }
        public bool IsValid
        {
            get
            {
                return Contract.IsValid;
            }
        }

        public void BackStep(object obj)
        {
            Mediator.Notify("GoToManagerScreen", "");
            _log.Info("GoToManagerScreen");

        }

        private void CreateContractEvent(object obj)
        {
            if (IsValid)
            {
                _ware_manager.CreateContract(int.Parse(Contract.ProductID), int.Parse(Contract.ProviderID), int.Parse(Contract.Quantity), true);
                _ware_model.Contracts = new ObservableCollection<ContractDTO>(_ware_manager.ShowActiveContracts());
                _log.Info("Creating Conntract");
            }

        }
        public CreateContractViewModel(WarehouseManagerViewModel wmv)
        {
            _ware_model = wmv;
            _ware_manager = wmv.WareManager;
            _products = new ObservableCollection<ProductDTO>(_ware_manager.ShowAllProducts());
            _providers = new ObservableCollection<ProviderDTO>(_ware_manager.ShowAllProviders());

            _contract = new ContractWPF();

            BackToManagerView = new RelayCommand(BackStep);

            Mediator.Subscribe("CreateContractEvent", CreateContractEvent);

            CreateContract = new RelayCommand(x =>
            {
                Mediator.Notify("CreateContractEvent", "");
            });
        }
    }
}
