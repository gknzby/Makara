using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makara.Behaviors.Base;

/// <summary>
/// Defines a contract for behaviors that can be attached to MAUI controls
/// </summary>
/// <typeparam name="T">The type of control this behavior can attach to</typeparam>
public interface IAttachedBehavior<T> where T : BindableObject
{
    /// <summary>
    /// Gets the control instance this behavior is attached to
    /// </summary>
    T AssociatedObject { get; }

    /// <summary>
    /// Called when the behavior is attached to a control
    /// </summary>
    /// <param name="bindable">The control being attached to</param>
    void OnAttachedTo(T bindable);

    /// <summary>
    /// Called when the behavior is detaching from a control
    /// </summary>
    /// <param name="bindable">The control being detached from</param>
    void OnDetachingFrom(T bindable);
}
