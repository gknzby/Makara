using Microsoft.Maui.Controls;
using System;
using Makara.Behaviors.Base;

namespace Makara.Behaviors.List;

/// <summary>
/// Behavior that handles button actions within list items to reduce command bindings
/// </summary>
public class ListItemActionBehavior : InteractionBehaviorBase<Grid>
{
    #region Bindable Properties

    public static readonly BindableProperty ItemProperty =
        BindableProperty.Create(nameof(Item), typeof(object), typeof(ListItemActionBehavior), null);

    /// <summary>
    /// Gets or sets the data item associated with this list item
    /// </summary>
    public object Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    #endregion

    /// <summary>
    /// Called when the behavior is attached to a control
    /// </summary>
    protected override void OnBehaviorAttached()
    {
        base.OnBehaviorAttached();

        if(AssociatedObject != null)
        {
            // Find and subscribe to all buttons within the Grid
            foreach(var child in AssociatedObject.Children)
            {
                if(child is Button button)
                {
                    button.Clicked += Button_Clicked;
                }
            }
        }
    }

    /// <summary>
    /// Called when the behavior is detaching from a control
    /// </summary>
    protected override void OnBehaviorDetaching()
    {
        if(AssociatedObject != null)
        {
            // Important: Unsubscribe from events to prevent memory leaks
            foreach(var child in AssociatedObject.Children)
            {
                if(child is Button button)
                {
                    button.Clicked -= Button_Clicked;
                }
            }
        }

        base.OnBehaviorDetaching();
    }

    /// <summary>
    /// Handles button click events
    /// </summary>
    private void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && !string.IsNullOrEmpty(button.ClassId))
        {
            // Create parameter object with item and action type
            var parameter = new ActionParameter
            {
                Item = Item,
                ActionType = ParseActionType(button.ClassId)
            };

            if (Command != null && Command.CanExecute(CommandParameter))
            {
                ExecuteCommand(parameter);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Command is null or cannot execute for ListItemActionBehavior.");
            }
        }
    }

    /// <summary>
    /// Parses action type from button's ClassId
    /// </summary>
    private ItemActionType ParseActionType(string actionTypeString)
    {
        if(Enum.TryParse<ItemActionType>(actionTypeString, out var result))
        {
            return result;
        }
        return ItemActionType.Unknown;
    }
}

/// <summary>
/// Defines available item action types
/// </summary>
public enum ItemActionType
{
    Unknown,
    Edit,
    Delete,
    Share,
    ViewDetails
}

/// <summary>
/// Parameter object containing item and action information
/// </summary>
public class ActionParameter
{
    public object Item { get; set; }
    public ItemActionType ActionType { get; set; }
}