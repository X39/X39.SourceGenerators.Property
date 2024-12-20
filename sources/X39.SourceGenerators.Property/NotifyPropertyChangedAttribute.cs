using System;

namespace X39.SourceGenerators.Property;

/// <summary>
/// Makes the field or class, annotated with the 'NotifyPropertyChanged' attribute, create
/// properties which raise the PropertyChanged event when changed.
/// For a decorated class, all fields will be considered for property generation and the
/// PropertyChanged event will be implemented too.
/// </summary>
/// <remarks>
/// The name of the property generated will be the Capitalized name of the field, minus any leading underscores.
/// If the field is annotated with the 'PropertyName' attribute,
/// the name of the property will be the value of the 'PropertyName' attribute.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class NotifyPropertyChangedAttribute : Attribute
{
    /// <summary>
    /// Indicates whether the PropertyChanged event should be generated for the decorated
    /// class or fields when the associated value changes.
    /// </summary>
    public bool GenerateEvent { get; }

    /// <summary>
    /// Makes the field or class, annotated with the 'NotifyPropertyChanged' attribute, create
    /// properties which raise the PropertyChanged event when changed.
    /// For a decorated class, all fields will be considered for property generation and the
    /// PropertyChanged event will be implemented too.
    /// </summary>
    /// <remarks>
    /// The name of the property generated will be the Capitalized name of the field, minus any leading underscores.
    /// If the field is annotated with the 'PropertyName' attribute,
    /// the name of the property will be the value of the 'PropertyName' attribute.
    /// </remarks>
    /// <param name="generateEvent">
    ///     If true, the PropertyChanged event will be generated (as in: Added to the generated class).
    ///     If false, the PropertyChanged event will not be generated and must be supplied by the user.
    /// </param>
    public NotifyPropertyChangedAttribute(bool generateEvent = true)
    {
        GenerateEvent = generateEvent;
    }
}

#pragma warning disable CS1574, CS1584, CS1581, CS1580
#pragma warning restore CS1574, CS1584, CS1581, CS1580