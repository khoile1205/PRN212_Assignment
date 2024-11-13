using System;
using System.ComponentModel;

public class OrderDto : INotifyPropertyChanged
{
    private int _id;
    private decimal? _totalOrderPrice;
    private decimal? _customerPay;
    private string? _status;
    private string? _reason;
    private DateTime? _createdTimestamp;
    private DateTime? _updatedTimestamp;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public int Id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
    }

    public decimal? TotalOrderPrice
    {
        get => _totalOrderPrice;
        set
        {
            if (_totalOrderPrice != value)
            {
                _totalOrderPrice = value;
                OnPropertyChanged(nameof(TotalOrderPrice));
            }
        }
    }

    public decimal? CustomerPay
    {
        get => _customerPay;
        set
        {
            if (_customerPay != value)
            {
                _customerPay = value;
                OnPropertyChanged(nameof(CustomerPay));
            }
        }
    }

    public string? Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    public string? Reason
    {
        get => _reason;
        set
        {
            if (_reason != value)
            {
                _reason = value;
                OnPropertyChanged(nameof(Reason));
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

    public DateTime? UpdatedTimestamp
    {
        get => _updatedTimestamp;
        set
        {
            if (_updatedTimestamp != value)
            {
                _updatedTimestamp = value;
                OnPropertyChanged(nameof(UpdatedTimestamp));
            }
        }
    }
}
