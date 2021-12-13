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
using WPF_MVVM.Models;

namespace WPF_MVVM.ViewModels
{
    public class UpdateOrderViewModel : BaseViewModel, IPageViewModel
    {
        protected ILogger _log = new Logger(typeof(UpdateOrderViewModel));

        private WarehouseManagerViewModel _ware_model;
        private readonly IWarehouseManager _ware_manager;
        private ObservableCollection<ProductDTO> _products;
        private ObservableCollection<OrderDTO> _orders;
        private ICommand _backToManagerView;
        private ICommand _updateOrder;
        private  OrderWPF _order;

        public OrderWPF Order
        {
            get
            {
                return _order;
            }
            set 
            {
                _order = value;
                this.OnPropertyChanged(nameof(Order));
            }
        }
        public string OrderID
        {
            get
            {
                return Order.OrderID;
            }
            set
            {
                Order.OrderID = value;
                this.OnPropertyChanged(nameof(OrderID));
            }
        }
        public string ProductID
        {
            get
            {
                return Order.ProductID;
            }
            set
            {
                Order.ProductID = value;
                this.OnPropertyChanged(nameof(ProductID));
            }
        }
        public string Quantity
        {
            get
            {
                return Order.Quantity;
            }
            set
            {
                Order.Quantity = value;
                this.OnPropertyChanged(nameof(Quantity));
            }
        }
        public string IsActive
        {
            get
            {
                return Order.IsActive;
            }
            set
            {
                Order.IsActive = value;
                this.OnPropertyChanged(nameof(IsActive));
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

        public ICommand UpdateOrder
        {
            get
            {
                return _updateOrder;
            }
            set
            {
                _updateOrder = value;
                this.OnPropertyChanged(nameof(UpdateOrder));
            }
        }
        public bool IsValid
        {
            get
            {
                return Order.IsValid;
            }
        }

        public void BackStep(object obj)
        {
            Mediator.Notify("GoToManagerScreen", "");
            _log.Info("GoToManagerScreen");
        }

        private void UpdateOrderEvent(object obj)
        {
            if (IsValid)
            {
                _ware_manager.UpdateOrder(int.Parse(Order.OrderID), int.Parse(Order.ProductID), int.Parse(Order.Quantity), Convert.ToBoolean(Order.IsActive));
                Orders = new ObservableCollection<OrderDTO>(_ware_manager.ShowActiveOrders());
                _ware_model.Orders = new ObservableCollection<OrderDTO>(_ware_manager.ShowActiveOrders());
                _log.Info("Updating Order");
            }
            
        }
        public UpdateOrderViewModel(WarehouseManagerViewModel wmv)
        {
            _ware_model = wmv;
            _ware_manager = wmv.WareManager;
            _products = new ObservableCollection<ProductDTO>(_ware_manager.ShowAllProducts());
            _orders = new ObservableCollection<OrderDTO>(_ware_manager.ShowActiveOrders());

            _order = new OrderWPF();

            BackToManagerView = new RelayCommand(BackStep);

            Mediator.Subscribe("UpdateOrderEvent", UpdateOrderEvent);

            UpdateOrder = new RelayCommand(x =>
            {
                Mediator.Notify("UpdateOrderEvent", "");
            });
        }
    }
}
