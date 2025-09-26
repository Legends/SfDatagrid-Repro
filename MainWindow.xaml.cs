using SfDatagrid_Repro.ViewModels;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.Windows.Shared;

namespace SfDatagrid_Repro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ChromelessWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private async void DataGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {

            var gRow = (e.AddedItems[0] as GridRowInfo);
            var row = gRow.RowData as ItemViewModel;
            // ...
            await Task.FromResult(true);

        }
    }
}