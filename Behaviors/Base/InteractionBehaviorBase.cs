using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Makara.Behaviors.Base;

public abstract class InteractionBehaviorBase<T> : BehaviorBase<T> where T : BindableObject
{
    #region Bindable Properties

    /// <summary>
    /// Bindable property for the command to execute on interaction
    /// </summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(InteractionBehaviorBase<T>),
            null, propertyChanged: OnCommandChanged);

    /// <summary>
    /// Bindable property for the parameter to pass to the command
    /// </summary>
    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(InteractionBehaviorBase<T>),
            null, propertyChanged: OnCommandParameterChanged);

    /// <summary>
    /// Gets or sets the command to execute when the interaction occurs
    /// </summary>
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the parameter to pass to the command
    /// </summary>
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    #endregion

    /// <summary>
    /// Called when the Command property changes
    /// </summary>
    private static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var behavior = (InteractionBehaviorBase<T>)bindable;
        behavior.OnCommandChanged((ICommand)oldValue, (ICommand)newValue);
    }

    /// <summary>
    /// Called when the CommandParameter property changes
    /// </summary>
    private static void OnCommandParameterChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var behavior = (InteractionBehaviorBase<T>)bindable;
        behavior.OnCommandParameterChanged(oldValue, newValue);
    }

    /// <summary>
    /// Called when the Command property changes
    /// </summary>
    /// <param name="oldValue">The previous command</param>
    /// <param name="newValue">The new command</param>
    protected virtual void OnCommandChanged(ICommand oldValue, ICommand newValue)
    {
        // Override in derived classes if needed
    }

    /// <summary>
    /// Called when the CommandParameter property changes
    /// </summary>
    /// <param name="oldValue">The previous parameter</param>
    /// <param name="newValue">The new parameter</param>
    protected virtual void OnCommandParameterChanged(object oldValue, object newValue)
    {
        // Override in derived classes if needed
    }

    /// <summary>
    /// Executes the command with the current parameter if the command can execute
    /// </summary>
    /// <returns>True if the command was executed, false otherwise</returns>
    protected bool ExecuteCommand()
    {
        return ExecuteCommand(CommandParameter);
    }

    /// <summary>
    /// Executes the command with the specified parameter if the command can execute
    /// </summary>
    /// <param name="parameter">The parameter to pass to the command</param>
    /// <returns>True if the command was executed, false otherwise</returns>
    protected bool ExecuteCommand(object parameter)
    {
        if(Command == null)
            return false;

        if(Command.CanExecute(parameter))
        {
            Command.Execute(parameter);
            return true;
        }

        return false;
    }
}
