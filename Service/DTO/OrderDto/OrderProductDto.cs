using System;
using System.ComponentModel;

namespace Service.DTO.OrderDto
{
    public class OrderProductDto : INotifyPropertyChanged
    {
        private int? _orderId;
        private int? _productId;
        private int? _quantity;
        private string _productName;
        private DateTime? _createdTimestamp;
        private decimal? _price;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public int? OrderId
        {
            get => _orderId;
            set
            {
                if (_orderId != value)
                {
                    _orderId = value;
                    OnPropertyChanged(nameof(OrderId));
                }
            }
        }

        public int? ProductId
        {
            get => _productId;
            set
            {
                if (_productId != value)
                {
                    _productId = value;
                    OnPropertyChanged(nameof(ProductId));
                }
            }
        }

        public int? Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public DateTime? CreatedTimestamp
        {
            get => _createdTimestamp;
            set
            {
                if (_createdTimestamp != value)
                {
                    _createdTimestamp = value;
                    OnPropertyChanged(nameof(CreatedTimestamp));
                }
            }
        }
        public string ProductName
        {
            get => _productName;
            set
            {
                if (_productName != value)
                {
                    _productName = value;
                    OnPropertyChanged(nameof(ProductName));
                }
            }
        }
        public decimal? Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }
    }
}