using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM.Models
{
    public class OrderWPF : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _orderId;
        private string _productId;        
        private string _quantity;
        private string _isActive;

        public int ContractID { get; set; }
        public string ProductID
        {
            get
            {
                return _productId;
            }
            set
            {
                _productId = value;
                OnPropertyChanged(nameof(ProductID));
            }
        }
        public string OrderID
        {
            get
            {
                return _orderId;
            }
            set
            {
                _orderId = value;
                OnPropertyChanged(nameof(OrderID));
            }
        }
        public string Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        public string IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }
        public DateTime RowInsertTime { get; set; }
        public DateTime RowUpdateTime { get; set; }

        public bool IsValid
        {
            get
            {
                return string.IsNullOrWhiteSpace(Error);
            }
        }
        public List<string> ValidateProperties
        {
            get
            {
                return new List<string>() { OrderID, ProductID, Quantity, IsActive };
            }
        }

        string GetErrorInfo(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(OrderID):
                    return ValidateOrderID();
                case nameof(ProductID):
                    return ValidateProductID();
                case nameof(Quantity):
                    return ValidateQuantity();
                case nameof(IsActive):
                    return ValidateIsActive();
                default:
                    return null;
            }
        }
        public string ValidateOrderID()
        {
            if (string.IsNullOrWhiteSpace(OrderID))
            {
                return "Please input a number";
            }
            if (int.TryParse(OrderID, out int result))
            {
                if (result <= 0)
                {
                    return "Please input a number greater than 0";
                }
            }
            else
            {
                return "Please input a valid number";
            }
            return null;
        }
        public string ValidateProductID()
        {
            if (string.IsNullOrWhiteSpace(ProductID))
            {
                return "Please input a number";
            }
            if (int.TryParse(ProductID, out int result))
            {
                if (result <= 0)
                {
                    return "Please input a number greater than 0";
                }
            }
            else
            {
                return "Please input a valid number";
            }
            return null;
        }

        public string ValidateQuantity()
        {
            if (string.IsNullOrWhiteSpace(Quantity))
            {
                return "Please input a number";
            }
            if (int.TryParse(Quantity, out int result))
            {
                if (result <= 0)
                {
                    return "Please input a number greater than 0";
                }
            }
            else
            {
                return "Please input a valid number";
            }
            return null;
        }
        public string ValidateIsActive()
        {
            if (string.IsNullOrWhiteSpace(IsActive))
            {
                return "Please input a bool value";
            }
            if (Boolean.TryParse(IsActive, out bool result))
            {
                if (result == false)
                {
                    return "Please input a true value";
                }
            }
            else
            {
                return "Please input a valid value";
            }
            return null;
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                return GetErrorInfo(propertyName);
            }
        }

        public string Error
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var property in ValidateProperties)
                {
                    var err = GetErrorInfo(property);
                    if (!string.IsNullOrWhiteSpace(err))
                    {
                        sb.AppendLine(err);
                    }
                }
                return sb.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Conditional("DEBUG")]
        private void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
                throw new ArgumentNullException(GetType().Name + " does not contain property: " + propertyName);
        }
    }
}
