using System;

namespace X39.SourceGenerators.Property;

/// <summary>
/// Makes the field or class, annotated with the 'NotifyPropertyChanging' attribute, create
/// properties which raise the PropertyChanging event when changed.
/// For a decorated class, all fields will be considered for property generation and the
/// PropertyChanging event will be implemented too.
/// </summary>
/// <remarks>
/// The name of the property generated will be the Capitalized name of the field, minus any leading underscores.
/// If the field is annotated with the 'PropertyName' attribute,
/// the name of the property will be the value of the 'PropertyName' attribute.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class NotifyPropertyChangingAttribute : Attribute
{
    /// <summary>
    /// Specifies whether the decorated class or fields should emit the PropertyChanging event when the associated value is modified.
    /// </summary>
    public bool GenerateEvent { get; }

    /// <summary>
    /// Makes the field or class, annotated with the 'NotifyPropertyChanging' attribute, create
    /// properties which raise the PropertyChanging event when changed.
    /// For a decorated class, all fields will be considered for property generation and the
    /// PropertyChanging event will be implemented too.
    /// </summary>
    /// <remarks>
    /// The name of the property generated will be the Capitalized name of the field, minus any leading underscores.
    /// If the field is annotated with the 'PropertyName' attribute,
    /// the name of the property will be the value of the 'PropertyName' attribute.
    /// </remarks>
    /// <param name="generateEvent">
    ///     If true, the PropertyChanging event will be generated (as in: Added to the generated class).
    ///     If false, the PropertyChanging event will not be generated and must be supplied by the user.
    /// </param>
    public NotifyPropertyChangingAttribute(bool generateEvent = true)
    {
        GenerateEvent = generateEvent;
    }
}