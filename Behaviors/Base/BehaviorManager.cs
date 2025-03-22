using Makara.Behaviors.List;
using Microsoft.Extensions.Logging;
using System.Windows.Input;

/// <summary>
/// Implementation of IBehaviorManager that manages behavior creation and lifecycle
/// </summary>
public class BehaviorManager : IBehaviorManager
{
    private readonly ILogger<BehaviorManager>? _logger;

    public BehaviorManager(ILogger<BehaviorManager>? logger = null)
    {
        _logger = logger;
    }

    public ListItemActionBehavior CreateListItemActionBehavior(Grid target, ICommand command, object? item)
    {
        if(target == null)
            throw new ArgumentNullException(nameof(target), "Target grid cannot be null");

        if(command == null)
            throw new ArgumentNullException(nameof(command), "Command cannot be null");

        _logger?.LogDebug("Creating ListItemActionBehavior for grid");

        return CreateAndAttachBehavior<ListItemActionBehavior, Grid>(target, behavior =>
        {
            behavior.Command = command;
            behavior.Item = item;
        });
    }

    public TBehavior CreateAndAttachBehavior<TBehavior, TTarget>(TTarget target, Action<TBehavior>? configureBehavior = null)
        where TBehavior : Behavior<TTarget>, new()
        where TTarget : VisualElement  // Changed from Element to VisualElement
    {
        if(target == null)
            throw new ArgumentNullException(nameof(target), $"Target of type {typeof(TTarget).Name} cannot be null");

        // Check if behavior already exists
        var existingBehavior = FindBehavior<TBehavior, TTarget>(target);
        if(existingBehavior != null)
        {
            _logger?.LogDebug($"Found existing {typeof(TBehavior).Name} behavior on {typeof(TTarget).Name}");
            configureBehavior?.Invoke(existingBehavior);
            return existingBehavior;
        }

        // Create new behavior
        var behavior = new TBehavior();
        configureBehavior?.Invoke(behavior);

        // Attach behavior to target
        target.Behaviors.Add(behavior);

        _logger?.LogDebug($"Created and attached new {typeof(TBehavior).Name} to {typeof(TTarget).Name}");

        return behavior;
    }

    public void DetachBehaviors<TBehavior, TTarget>(TTarget? target)
        where TBehavior : Behavior<TTarget>
        where TTarget : VisualElement  // Changed from Element to VisualElement
    {
        if(target == null)
        {
            _logger?.LogDebug($"DetachBehaviors called with null target for {typeof(TBehavior).Name}");
            return;
        }

        var behaviorsToRemove = target.Behaviors
            .OfType<TBehavior>()
            .ToList();

        if(behaviorsToRemove.Count == 0)  // Fixed method group error by using Count property
        {
            _logger?.LogDebug($"No {typeof(TBehavior).Name} behaviors found to detach");
            return;
        }

        foreach(var behavior in behaviorsToRemove)
        {
            target.Behaviors.Remove(behavior);
        }

        _logger?.LogDebug($"Detached {behaviorsToRemove.Count} {typeof(TBehavior).Name} behaviors from {typeof(TTarget).Name}");
    }

    public TBehavior? FindBehavior<TBehavior, TTarget>(TTarget target)
        where TBehavior : Behavior<TTarget>
        where TTarget : VisualElement  // Changed from Element to VisualElement
    {
        if(target == null)
            throw new ArgumentNullException(nameof(target), $"Target of type {typeof(TTarget).Name} cannot be null");

        return target.Behaviors.OfType<TBehavior>().FirstOrDefault();
    }
}

/// <summary>
/// Manages the creation, attachment, and lifecycle of behaviors
/// </summary>
public interface IBehaviorManager
{
    /// <summary>
    /// Creates and attaches a ListItemActionBehavior to a grid
    /// </summary>
    ListItemActionBehavior CreateListItemActionBehavior(Grid target, ICommand command, object? item);

    /// <summary>
    /// Creates a behavior and attaches it to a target
    /// </summary>
    TBehavior CreateAndAttachBehavior<TBehavior, TTarget>(TTarget target, Action<TBehavior>? configureBehavior = null)
        where TBehavior : Behavior<TTarget>, new()
        where TTarget : VisualElement;  // Changed from Element to VisualElement

    /// <summary>
    /// Detaches all behaviors of a specific type from the target element
    /// </summary>
    void DetachBehaviors<TBehavior, TTarget>(TTarget? target)
        where TBehavior : Behavior<TTarget>
        where TTarget : VisualElement;  // Changed from Element to VisualElement

    /// <summary>
    /// Gets an existing behavior from a control if it exists
    /// </summary>
    TBehavior? FindBehavior<TBehavior, TTarget>(TTarget target)
        where TBehavior : Behavior<TTarget>
        where TTarget : VisualElement;  // Changed from Element to VisualElement
}
