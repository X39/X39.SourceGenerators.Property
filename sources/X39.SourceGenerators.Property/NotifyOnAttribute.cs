﻿using System;

namespace X39.SourceGenerators.Property;

/// <summary>
/// Allows to mark a property to be part of the notify changed/-ing chain
/// of a generated property.
/// </summary>
/// <remarks>
/// This attribute will not error if the referenced property name is not
/// generated by the generator. It will just not work as intended in
/// that case.
/// </remarks>
/// <example>
/// <code>
/// // [...]
/// private float _amount;
///
/// [NotifyOn(nameof(Amount))]
/// public string Money => $"{Amount} €";
/// // [...]
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class NotifyOnAttribute : Attribute
{
    /// <summary>
    /// The property this property wants to be notified on.
    /// </summary>
    public string Property { get; }

    /// <summary>
    /// Allows to mark a property to be part of the notify changed/-ing chain
    /// of a generated property.
    /// </summary>
    /// <param name="property">The name of the property to be notified on.</param>
    public NotifyOnAttribute(string property)
    {
        Property = property;
    }
}