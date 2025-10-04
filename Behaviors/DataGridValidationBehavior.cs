using Microsoft.Xaml.Behaviors;
using Repro.ViewModels;
using Syncfusion.UI.Xaml.Grid;

namespace Repro.Behaviors;
/// <summary>
/// INLINE EDITING BEHAVIOR
/// </summary>
public class DataGridValidationBehavior : Behavior<SfDataGrid>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.CurrentCellValidating += OnCurrentCellValidating;
        AssociatedObject.CurrentCellBeginEdit += OnCurrentCellBeginEdit;
        AssociatedObject.CurrentCellEndEdit += OnCurrentCellEndEdit;
        AssociatedObject.RowValidating += OnRowValidating;
    }

    private void OnCurrentCellBeginEdit(object? sender, CurrentCellBeginEditEventArgs e)
    {
        if (AssociatedObject.CurrentItem is ItemViewModel vm)
            vm.EditingSecret = vm.Value;

    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject.CurrentCellValidating -= OnCurrentCellValidating;
        AssociatedObject.CurrentCellEndEdit -= OnCurrentCellEndEdit;
        AssociatedObject.RowValidating -= OnRowValidating;
    }

    private void OnCurrentCellEndEdit(object? sender, CurrentCellEndEditEventArgs e)
    {
        if (AssociatedObject.CurrentItem is not ItemViewModel vm)
            return;
    }

    private void OnRowValidating(object? sender, RowValidatingEventArgs e)
    {
        if (e.RowData is not ItemViewModel item) return;

        if (true)
            item.Value = item.EditingValue;

        e.IsValid = false;
        e.ErrorMessages.Add(nameof(ItemViewModel.Platform), "Platform is wrong");

    }

    private void OnCurrentCellValidating(object? sender, CurrentCellValidatingEventArgs e)
    {
        if (e.RowData is not ItemViewModel item)
            return;

        string? error = null;

        switch (e.Column.MappingName)
        {
            case nameof(ItemViewModel.Platform):

                error = "Platform is wroooooong";

                break;
            case nameof(ItemViewModel.Value):

                break;
        }

        if (!string.IsNullOrWhiteSpace(error))
        {
            e.IsValid = false;
            e.ErrorMessage = error;
        }
    }
}