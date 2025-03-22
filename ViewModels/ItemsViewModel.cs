using CommunityToolkit.Mvvm.ComponentModel;

using Makara.Behaviors.List;
using Makara.Models;

using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Makara.ViewModels;

/// <summary>
/// ViewModel for the Items page
/// </summary>
public partial class ItemsViewModel : ObservableObject
{
    /// <summary>
    /// Collection of items to display
    /// </summary>
    public ObservableCollection<ItemModel> Items { get; } = new();

    /// <summary>
    /// Command that handles all item actions
    /// </summary>
    public ICommand ItemActionCommand { get; }

    /// <summary>
    /// Initializes a new instance of the ItemsViewModel class
    /// </summary>
    public ItemsViewModel()
    {
        // Single command handles all actions
        ItemActionCommand = new Command<ActionParameter>(ExecuteItemAction);

        // Load sample items
        LoadSampleItems();
    }

    /// <summary>
    /// Executes the appropriate action based on the action type
    /// </summary>
    private void ExecuteItemAction(ActionParameter parameter)
    {
        if (parameter == null || parameter.Item == null)
            return;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            try
            {
                switch (parameter.ActionType)
                {
                    case ItemActionType.Edit:
                        // Edit implementation
                        break;

                    case ItemActionType.Delete:
                        // Remove from collection
                        Items.Remove(parameter.Item as ItemModel);
                        break;

                    case ItemActionType.Share:
                        // Share implementation
                        break;

                    case ItemActionType.ViewDetails:
                        // View details implementation
                        break;

                    default:
                        System.Diagnostics.Debug.WriteLine($"Unknown action type: {parameter.ActionType}");
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error executing item action: {ex}");
            }
        });
    }

    /// <summary>
    /// Loads sample items for demonstration
    /// </summary>
    public void LoadSampleItems()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            try
            {
                for (int i = 1; i <= 20; i++)
                {
                    Items.Add(new ItemModel { Id = i, Name = $"Item {i}" });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading sample items: {ex}");
            }
        });
    }
}