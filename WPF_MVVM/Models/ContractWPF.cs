using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM.Models
{
    public class ContractWPF : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _productId;
        private string _providerId;
        private string _quantity;

        public string ContractID { get; set; }
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
        public string ProviderID
        {
            get
            {
                return _providerId;
            }
            set
            {
                _providerId = value;
                OnPropertyChanged(nameof(ProviderID));
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
        public bool IsActive { get; set; }
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
                return new List<string>() { ProductID, ProviderID, Quantity };
            }
        }

        string GetErrorInfo(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ProductID):
                    return ValidateProductID();
                case nameof(ProviderID):
                    return ValidateProviderID();
                case nameof(Quantity):
                    return ValidateQuantity();
                default:
                    return null;
            }
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

        public string ValidateProviderID()
        {
            if (string.IsNullOrWhiteSpace(ProviderID))
            {
                return "Please input a number";
            }
            if (int.TryParse(ProviderID, out int result))
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
