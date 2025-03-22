using Makara.Behaviors.Base;

using Microsoft.Maui.Controls;

using System;

namespace Makara.Behaviors.List
{
    /// <summary>
    /// Behavior that handles button actions within list items to reduce command bindings
    /// </summary>
    public class ListItemActionBehavior : Behavior<Grid>
    {
        #region Bindable Properties

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(System.Windows.Input.ICommand),
                typeof(ListItemActionBehavior), null);

        public System.Windows.Input.ICommand? Command
        {
            get => (System.Windows.Input.ICommand?)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty ItemProperty =
            BindableProperty.Create(nameof(Item), typeof(object), typeof(ListItemActionBehavior), null);

        public object? Item
        {
            get => GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        #endregion

        private Grid? _associatedGrid;

        /// <summary>
        /// Called when the behavior is attached to a grid
        /// </summary>
        protected override void OnAttachedTo(Grid bindable)
        {
            base.OnAttachedTo(bindable);
            _associatedGrid = bindable;

            // Important: Wait for layout to complete before attaching handlers
            bindable.Loaded += OnGridLoaded;
        }

        private void OnGridLoaded(object? sender, EventArgs e)
        {
            if(_associatedGrid == null) return;

            // Unsubscribe from loaded event
            _associatedGrid.Loaded -= OnGridLoaded;

            // Attach handlers after layout is complete
            AttachEventHandlers();
        }

        /// <summary>
        /// Called when the behavior is detaching from a grid
        /// </summary>
        protected override void OnDetachingFrom(Grid bindable)
        {
            // Clean up all event handlers
            DetachEventHandlers();

            // Remove loaded event if still subscribed
            if(_associatedGrid != null)
            {
                _associatedGrid.Loaded -= OnGridLoaded;
            }

            _associatedGrid = null;
            base.OnDetachingFrom(bindable);
        }

        /// <summary>
        /// Finds and subscribes to all buttons in the grid
        /// </summary>
        private void AttachEventHandlers()
        {
            if(_associatedGrid == null) return;

            try
            {
                foreach(var child in _associatedGrid.Children)
                {
                    if(child is Button button && !string.IsNullOrEmpty(button.ClassId))
                    {
                        button.Clicked += Button_Clicked;
                    }
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in AttachEventHandlers: {ex}");
            }
        }

        /// <summary>
        /// Unsubscribes from all button events
        /// </summary>
        private void DetachEventHandlers()
        {
            if(_associatedGrid == null) return;

            try
            {
                foreach(var child in _associatedGrid.Children)
                {
                    if(child is Button button)
                    {
                        button.Clicked -= Button_Clicked;
                    }
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DetachEventHandlers: {ex}");
            }
        }

        /// <summary>
        /// Handles button click events
        /// </summary>
        private void Button_Clicked(object? sender, EventArgs e)
        {
            if(Command == null || sender is not Button button || string.IsNullOrEmpty(button.ClassId))
                return;

            // Create parameter object with item and action type
            var parameter = new ActionParameter
            {
                Item = Item,
                ActionType = ParseActionType(button.ClassId)
            };

            // Execute the command with the parameter
            if(Command.CanExecute(parameter))
            {
                Command.Execute(parameter);
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
        public object? Item { get; set; }
        public ItemActionType ActionType { get; set; }
    }
}