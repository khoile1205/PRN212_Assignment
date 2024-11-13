using System;
using System.ComponentModel;

namespace Service.DTO.OrderDto
{
    public class OrderProductDto : INotifyPropertyChanged
    {
        private int? _orderId;
        private int? _productId;
        private int? _quantity;
        private DateTime? _createdTimestamp;

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
    }
}
