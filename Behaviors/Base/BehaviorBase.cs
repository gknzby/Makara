using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makara.Behaviors.Base;

/// <summary>
/// Base implementation of the IAttachedBehavior interface with property
/// change notification support
/// </summary>
/// <typeparam name="T">The type of control this behavior can attach to</typeparam>
public abstract class BehaviorBase<T> : Behavior<T>, IAttachedBehavior<T>
    where T : BindableObject
{
    /// <summary>
    /// Holds a weak reference to the associated object to prevent memory leaks
    /// </summary>
    private WeakReference<T> _associatedObject;

    /// <summary>
    /// Gets the control instance this behavior is attached to
    /// </summary>
    public T AssociatedObject =>
        _associatedObject != null && _associatedObject.TryGetTarget(out var target) ? target : null;

    /// <summary>
    /// Called when the behavior is attached to a control
    /// </summary>
    /// <param name="bindable">The control being attached to</param>
    protected override void OnAttachedTo(T bindable)
    {
        base.OnAttachedTo(bindable);
        _associatedObject = new WeakReference<T>(bindable);

        // Subscribe to bindable object property changes if it supports INotifyPropertyChanged
        if(bindable is INotifyPropertyChanged notifyPropertyChanged)
        {
            notifyPropertyChanged.PropertyChanged += OnAssociatedObjectPropertyChanged;
        }

        OnBehaviorAttached();
    }

    /// <summary>
    /// Called when the behavior is detaching from a control
    /// </summary>
    /// <param name="bindable">The control being detached from</param>
    protected override void OnDetachingFrom(T bindable)
    {
        OnBehaviorDetaching();

        // Unsubscribe from property changes
        if(bindable is INotifyPropertyChanged notifyPropertyChanged)
        {
            notifyPropertyChanged.PropertyChanged -= OnAssociatedObjectPropertyChanged;
        }

        _associatedObject = null;
        base.OnDetachingFrom(bindable);
    }

    /// <summary>
    /// Called when the associated object's property changes
    /// </summary>
    protected virtual void OnAssociatedObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        // Override in derived classes to handle property changes
    }

    /// <summary>
    /// Called immediately after the behavior is attached to a control
    /// </summary>
    protected virtual void OnBehaviorAttached()
    {
        // Override in derived classes for initialization logic
    }

    /// <summary>
    /// Called immediately before the behavior is detached from a control
    /// </summary>
    protected virtual void OnBehaviorDetaching()
    {
        // Override in derived classes for cleanup logic
    }

    void IAttachedBehavior<T>.OnAttachedTo(T bindable)
    {
        this.OnAttachedTo(bindable);
    }

    void IAttachedBehavior<T>.OnDetachingFrom(T bindable)
    {
        this.OnDetachingFrom(bindable);
    }
}