using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Reflection;

namespace Makara.Behaviors.List;

/// <summary>
/// Maps button click events to commands without using behaviors
/// Designed specifically for CollectionView scenarios where behaviors cause binding issues
/// </summary>
public static class ButtonCommandMapper
{
    // Attached property for the command
    public static readonly BindableProperty CommandProperty =
        BindableProperty.CreateAttached(
            "Command",
            typeof(ICommand),
            typeof(ButtonCommandMapper),
            null,
            propertyChanged: OnCommandPropertyChanged);

    // Getter for the attached property
    public static ICommand GetCommand(BindableObject view)
    {
        return (ICommand)view.GetValue(CommandProperty);
    }

    // Setter for the attached property
    public static void SetCommand(BindableObject view, ICommand value)
    {
        view.SetValue(CommandProperty, value);
    }

    // Attached property for the command parameter
    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.CreateAttached(
            "CommandParameter",
            typeof(object),
            typeof(ButtonCommandMapper),
            null);

    // Getter for the command parameter
    public static object GetCommandParameter(BindableObject view)
    {
        return view.GetValue(CommandParameterProperty);
    }

    // Setter for the command parameter
    public static void SetCommandParameter(BindableObject view, object value)
    {
        view.SetValue(CommandParameterProperty, value);
    }

    // Attached property for auto-mapping
    public static readonly BindableProperty AutoWireButtonsProperty =
        BindableProperty.CreateAttached(
            "AutoWireButtons",
            typeof(bool),
            typeof(ButtonCommandMapper),
            false,
            propertyChanged: OnAutoWireButtonsChanged);

    // Getter for auto-wire property
    public static bool GetAutoWireButtons(BindableObject view)
    {
        return (bool)view.GetValue(AutoWireButtonsProperty);
    }

    // Setter for auto-wire property
    public static void SetAutoWireButtons(BindableObject view, bool value)
    {
        view.SetValue(AutoWireButtonsProperty, value);
    }

    // Handles the AutoWireButtons property change
    private static void OnAutoWireButtonsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if(bindable is not Layout layout)
            return;

        var autoWire = (bool)newValue;

        if(autoWire)
        {
            layout.ChildAdded += OnLayoutChildAdded;

            // Wire up any existing buttons
            foreach(var button in layout.GetChildButtons())
            {
                WireUpButton(button, layout);
            }
        }
        else
        {
            layout.ChildAdded -= OnLayoutChildAdded;

            // Unwire existing buttons
            foreach(var button in layout.GetChildButtons())
            {
                UnwireButton(button);
            }
        }
    }

    // Handles the Command property change
    private static void OnCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if(bindable is Button button)
        {
            button.Clicked -= OnButtonClicked;
            if(newValue != null)
            {
                button.Clicked += OnButtonClicked;
            }
        }
    }

    // Event handler for child added to layout
    private static void OnLayoutChildAdded(object sender, ElementEventArgs e)
    {
        if(sender is Layout layout && e.Element is Button button)
        {
            WireUpButton(button, layout);
        }
    }

    // Wires up a button to use the parent's command
    private static void WireUpButton(Button button, Layout layout)
    {
        // Don't wire up if already has a command
        if(GetCommand(button) != null)
            return;

        button.Clicked -= OnButtonClicked;
        button.Clicked += OnButtonClicked;
    }

    // Unwires a button
    private static void UnwireButton(Button button)
    {
        button.Clicked -= OnButtonClicked;
    }

    // Button click handler
    private static void OnButtonClicked(object sender, EventArgs e)
    {
        if(sender is not Button button)
            return;

        // First check if button has its own command
        var command = GetCommand(button);

        // If not, try to get command from parent
        if(command == null && button.Parent is Layout layout)
        {
            command = GetCommand(layout);
        }

        if(command == null)
            return;

        // Get parameter from button or use default from parent
        object parameter = GetCommandParameter(button);

        // If no parameter set on button but the parent has ClassId-based action mapping
        if(parameter == null && !string.IsNullOrEmpty(button.ClassId) && button.Parent is Layout parentLayout)
        {
            // Create action parameter based on ClassId and parent's binding context
            parameter = new ActionParameter
            {
                Item = parentLayout.BindingContext,
                ActionType = ParseActionType(button.ClassId)
            };
        }

        if(command.CanExecute(parameter))
            command.Execute(parameter);
    }

    // Helper method to parse action type from string
    private static ItemActionType ParseActionType(string actionTypeString)
    {
        if(Enum.TryParse<ItemActionType>(actionTypeString, out var result))
        {
            return result;
        }
        return ItemActionType.Unknown;
    }

    // Extension method to get all buttons in a layout
    private static IEnumerable<Button> GetChildButtons(this Layout layout)
    {
        foreach(var child in layout.Children)
        {
            if(child is Button button)
            {
                yield return button;
            }
            else if(child is Layout childLayout)
            {
                foreach(var nestedButton in childLayout.GetChildButtons())
                {
                    yield return nestedButton;
                }
            }
        }
    }
}