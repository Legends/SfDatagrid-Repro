using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;


namespace SfDatagrid_Repro.ViewModels;

public class ItemViewModel : INotifyPropertyChanged, IEquatable<ItemViewModel>, IEditableObject, IDataErrorInfo
{

    #region Properties
    private Dictionary<string, object>? _storedValues;

    public int ID { get; set; }

    bool _IsHighlighted;
    public bool IsHighlighted
    {
        get => _IsHighlighted;
        set
        {
            if (_IsHighlighted != value)
            {
                _IsHighlighted = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _isBeingEdited;

    [JsonIgnore]
    public bool IsBeingEdited
    {
        get => _isBeingEdited;
        set
        {
            _isBeingEdited = value;
            OnPropertyChanged();
        }
    }

    private string? _platform;

    public string? Platform
    {
        get => _platform;
        set
        {
            if (_platform != value)
            {
                _platform = value;
                OnPropertyChanged();
            }
        }
    }

    private string? _value = string.Empty;
    public string? Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                OnPropertyChanged();
            }
        }
    }

    public string? EditingValue
    {
        get;
        set;
    }

    private string? _account;
    public string? Account
    {
        get => _account;
        set
        {
            if (_account != value)
            {
                _account = value;
                OnPropertyChanged();
            }
        }
    }



    #endregion

    [JsonConstructor]
    public ItemViewModel(int id, string platform, string secret, string? account = null)
    {
        ID = id;
        Platform = platform;
        Value = secret;
        Account = account;
    }


    #region IEditableObject Implementation

    public void BeginEdit()
    {
        _storedValues = BackUp();
    }

    public void CancelEdit()
    {
        if (_storedValues == null)
            return;

        foreach (var item in _storedValues)
        {
            var itemProperties = GetType().GetTypeInfo().DeclaredProperties;
            var pDesc = itemProperties.FirstOrDefault(p => p.Name == item.Key);
            pDesc?.SetValue(this, item.Value);
        }
    }

    public void EndEdit()
    {
        _storedValues?.Clear();
        _storedValues = null;

        Debug.WriteLine("End Edit Called");
    }

    public ItemViewModel? Copy()
    {
        return this.MemberwiseClone() as ItemViewModel;
    }

    #endregion

    #region IDataErrorInfo Implementation

    [JsonIgnore]
    public string Error => null!;

    [JsonIgnore]
    public string this[string columnName]
    {
        get
        {
            var errors = new List<string>();
            // ... add error messages for specific properties

            return string.Join(" ", errors);
        }
    }


    #endregion

    #region IEquatable & Overrides

    public bool Equals(ItemViewModel? other)
    {
        return ReferenceEquals(this, other) || other is not null && string.Equals(Platform, other.Platform, StringComparison.Ordinal) &&
               string.Equals(Value, other.Value, StringComparison.Ordinal) &&
               string.Equals(Account, other.Account, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj)
    {
        return obj is ItemViewModel other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Platform is null ? 0 : StringComparer.Ordinal.GetHashCode(Platform),
            Value is null ? 0 : StringComparer.Ordinal.GetHashCode(Value),
            Account is null ? 0 : StringComparer.Ordinal.GetHashCode(Account)
        );
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #endregion

    #region Backup Logic

    protected Dictionary<string, object> BackUp()
    {
        var dict = new Dictionary<string, object>();
        var itemProperties = GetType().GetTypeInfo().DeclaredProperties;

        foreach (var pDescriptor in itemProperties)
        {
            if (pDescriptor.CanWrite)
                dict.Add(pDescriptor.Name, pDescriptor.GetValue(this)!);
        }

        return dict;
    }

    #endregion

    public void UpdateSelf(ItemViewModel changed)
    {
        this.Platform = changed.Platform;
        this.Value = changed.Value;
        this.Account = changed.Account;
    }

}
