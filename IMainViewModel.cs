using SfDatagrid_Repro.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SfDatagrid_Repro
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        ObservableCollection<ItemViewModel> AllItems { get; }
        //AsyncCommand AddNewItemCommand { get; }

        ICommand BeginEditCommand { get; }
        ICommand EndEditCommand { get; }
        ICommand DoubleClickCommand { get; }

        bool IsContextmenuOpen { get; set; }


    }
}
