
using Repro.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace Repro.ViewModels;

public class MainViewModel : IMainViewModel, INotifyPropertyChanged //, ILocalizable
{
    #region ### PROPERTIES AND VARS ###




    bool _isGridInEditMode;
    public bool IsGridEditing
    {
        get => _isGridInEditMode;
        set
        {
            if (_isGridInEditMode == value) return;

            _isGridInEditMode = value;
            OnPropertyChanged();
            AddNewSecretCommand.RaiseCanExecuteChanged();

        }
    }

    public bool IsContextmenuOpen { get; set; }



    private ItemViewModel _selectedSecret = null!;

    public ItemViewModel SelectedSecret
    {
        get => _selectedSecret;
        set
        {
            if (!EqualityComparer<ItemViewModel?>.Default.Equals(_selectedSecret, value))
            {
                foreach (var item in AllItems)
                    item.IsBeingEdited = false;

                _selectedSecret = value;
                OnPropertyChanged();
                //OnSecretSelectedAsync();
            }
        }
    }
    public ItemViewModel? PreviousVersion { get; set; }


    #endregion REGION PROPERTIES AND VARS

    #region ### ObservableCollections ###


    ObservableCollection<ItemViewModel> _AllItems = new();
    public ObservableCollection<ItemViewModel> AllItems
    {
        get => _AllItems;
        private set
        {
            _AllItems = value;
            OnPropertyChanged();
        }
    }

    ObservableCollection<ItemViewModel> _filteredItems = new();
    public ObservableCollection<ItemViewModel> FilteredItems
    {
        get => _filteredItems;
        set
        {
            _filteredItems = value;
            OnPropertyChanged();
        }
    }


    #endregion ObservableCollections

    #region ### Events ###
    //public event EventHandler<CultureInfo>? LanguageChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region ### Constructor ###

    public MainViewModel() // NEW
    {


        SetupCommandEventhandler();
        AllItems = new ObservableCollection<ItemViewModel>();
        for (int i = 0; i < 5; i++)
        {
            AllItems.Add(new ItemViewModel(i, $"Platform {i}", $"Value {i}", $"Account {i}"));
        }


    }


    #endregion

    #region ### COMMANDS EVENTHANDLER ###

    private void SetupCommandEventhandler()
    {
        BeginEditCommand = new RelayCommand<ItemViewModel>(OnBeginEdit);
        EndEditCommand = new AsyncCommand<ItemViewModel>(OnEndEdit);
    }

    #endregion COMMANDS SETUP

    /// <summary>
    /// not used here in this repro
    /// but could be used for filtering the view?
    /// </summary>
    internal void RefreshView()
    {
        FilteredItems.Clear();
        var SearchText = string.Empty; // this.SearchTextBox.Text;   
        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? AllItems
            : AllItems.Where(x =>
                x.Platform?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false);

        foreach (var item in filtered)
            FilteredItems.Add(item);
    }


    #region ### COMMANDS DECLARATION ###


    public ICommand OpenEditCommand { get; private set; } = null!;
    public AsyncCommand SaveEditAsyncCommand { get; private set; } = null!;
    public ICommand CancelEditCommand { get; private set; } = null!;
    public ICommand ChangeLanguageCommand { get; private set; } = null!;
    public AsyncCommand AddNewSecretCommand { get; private set; } = null!;
    public ICommand BeginEditCommand { get; private set; } = null!;
    public ICommand EndEditCommand { get; private set; } = null!;

    //public ICommand DeleteItemCommand { get; private set; } = null!;
    public ICommand DoubleClickCommand { get; private set; } = null!;

    public ICommand UpdateSecretCommand { get; private set; } = null!;
    public ICommand SelectionChangedCommand { get; private set; } = null!;

    public AsyncCommand AddNewItemCommand => throw new NotImplementedException();

    public ICommand UpdateItemCommand => throw new NotImplementedException();

    public ItemViewModel SelectedItem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public ICommand DeleteItemCommand => throw new NotImplementedException();

    #endregion REGION COMMANDS


    private void OnBeginEdit(ItemViewModel item)
    {
        PreviousVersion = item.Copy();
        item.IsBeingEdited = true;
    }


    private async Task OnEndEdit(ItemViewModel item)
    {
        item.IsBeingEdited = false;

        await Task.FromResult(0);
        //if (!SecretItemValueComparer.Default.Equals(item, PreviousVersion))
        //{
        //    var (isValid, error) = SecretsManager.IsValidSecretItem(item.ToDomain());

        //    if (!isValid)
        //    {
        //        _messageService.ShowInfoMessage(ValidationMessageMapper.ToMessage(error));
        //        return;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            // Update the secret if valid
        //            await UpdateSecretAsync(item);
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, UI.ex_UpdatingSecret);
        //            _messageService.ShowErrorMessage(UI.ex_UpdatingSecret);
        //        }
        //    }

        //}
        PreviousVersion = null;
    }



}


