using ScheduleAndStockManagement.Enums;

namespace ScheduleAndStockManagement.Controls;

public partial class InventoryTypeSelector : ContentView
{
    public static readonly BindableProperty SelectedInventoryTypeProperty =
        BindableProperty.Create(
            nameof(SelectedInventoryType),
            typeof(InventoryTypeDesignation),
            typeof(InventoryTypeSelector),
            default(InventoryTypeDesignation),
            BindingMode.TwoWay,
            propertyChanged: OnSelectedInventoryTypeChanged);

    public InventoryTypeDesignation SelectedInventoryType
    {
        get => (InventoryTypeDesignation)GetValue(SelectedInventoryTypeProperty);
        set => SetValue(SelectedInventoryTypeProperty, value);
    }

    public InventoryTypeSelector()
    {
        InitializeComponent();
    }

    private static void OnSelectedInventoryTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not InventoryTypeSelector control || newValue == null)
            return;

        // Update RadioButton selection
        foreach (var child in control.RadioButtonsContainer.Children)
        {
            if (child is RadioButton rb)
            {
                rb.IsChecked = rb.Value?.ToString() == newValue.ToString();
            }
        }
    }

    private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value)
            return;

        if (sender is RadioButton rb &&
            Enum.TryParse<InventoryTypeDesignation>(rb.Value?.ToString(), out var result))
        {
            SelectedInventoryType = result;
        }
    }
}
