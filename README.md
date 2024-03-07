# X39.SourceGenerators.Property

This library adds a source generator that generates properties for a given class.

# Quick Start

Add the nuget package to your project.
After that, make the classes you want to generate properties for partial and add corresponding attributes to the class,
eg.:

```csharp
using X39.SourceGenerators.Property;

[NotifyPropertyChanged(true)]
public partial class MyClass
{
    private int _myProperty;
}
```

This will generate a partial class with the following content:

```csharp
#nullable enable
public partial class MyClass
    : System.ComponentModel.INotifyPropertyChanged
{
    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(MyProperty)));
        }
    }
}
```

# When and how are properties generated?

The properties are generated when one of the (Attributes)[#Attributes] is placed on the class or field.
The source generator will then generate a partial class with the same name as the original class and add the properties
to it.

The property **name** will be the name of the field with the first letter capitalized and an optional underscore
removed.
Additionally, if the field is suffixed with `Field`, the suffix will be removed too.
See (`PropertyNameAttribute`)[#PropertyNameAttribute] to customize the property name.

For a more "example driven" approach to this explanation, see the following table:

| Field Name | Property Name |
|------------|---------------|
| _abc       | Abc           |
| _abcField  | Abc           |
| abc        | Abc           |
| __abc      | _abc          |
| _abc_      | Abc_          |

# Attributes

Some attributes are placeable on either the class or field.
If the attribute is placed on the class, the attribute will be taken as a default for all fields.
This also implies that the field attributes always take precedence over the class attributes.

## `NotifyPropertyChangedAttribute`

This attribute will make the source generator add a `PropertyChanged` event call to the setter of the property.
If the attribute is placed on the class and the parameter is set to `true` (default: `false`),
the source generator will implement the `INotifyPropertyChanged` interface on the class.

### On the class with true

```csharp
// User-Code
[NotifyPropertyChanged(true)]
public partial class MyClass
{
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
    : System.ComponentModel.INotifyPropertyChanged
{
    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(MyProperty)));
        }
    }
}
```

### On the class with false

```csharp
// User-Code
[NotifyPropertyChanged(false)]
public partial class MyClass : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
        }
    }
}
```

### On the field with true

```csharp
// User-Code
public partial class MyClass : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    [NotifyPropertyChanged(true)]
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(MyProperty)));
        }
    }
}
```

### On the field with false

```csharp
// User-Code
public partial class MyClass : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    [NotifyPropertyChanged(false)]
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
        }
    }
}
```

## `NotifyPropertyChangingAttribute`

This attribute will make the source generator add a `PropertyChanging` event call to the setter of the property.
If the attribute is placed on the class and the parameter is set to `true` (default: `false`),
the source generator will implement the `INotifyPropertyChanging` interface on the class.

### On the class with true

```csharp
// User-Code
[NotifyPropertyChanging(true)]
public partial class MyClass
{
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
    : System.ComponentModel.INotifyPropertyChanging
{
    public event System.ComponentModel.PropertyChangingEventHandler? PropertyChanging;
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
            this.PropertyChanging?.Invoke(this, new System.ComponentModel.PropertyChangingEventArgs(nameof(MyProperty)));
        }
    }
}
```

### On the class with false

```csharp
// User-Code
[NotifyPropertyChanging(false)]
public partial class MyClass : INotifyPropertyChanging
{
    public event PropertyChangingEventHandler? PropertyChanging;
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
        }
    }
}
```

### On the field with true

```csharp
// User-Code
public partial class MyClass : INotifyPropertyChanging
{
    public event PropertyChangingEventHandler? PropertyChanging;
    [NotifyPropertyChanging(true)]
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(MyProperty)));
        }
    }
}
```

### On the field with false

```csharp
// User-Code
public partial class MyClass : INotifyPropertyChanging
{
    public event PropertyChangingEventHandler? PropertyChanging;
    [NotifyPropertyChanging(false)]
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
        }
    }
}
```

## `PropertyNameAttribute`
This attribute will make the source generator use the given name as the property name.
It cannot be placed on the class.

### On the field

```csharp
// User-Code
public partial class MyClass
{
    [PropertyName("MyProperty")]
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
        }
    }
}
```

## `ValidationStrategyAttribute`
The validation strategy attribute is used to define how additional, validating properties should be handled, when
the validation fails.

### Supported Validations
- `System.ComponentModel.DataAnnotations.RangeAttribute`
- `System.ComponentModel.DataAnnotations.MaxLengthAttribute`
- [`GuardAttribute`](#GuardAttribute)

### Available Strategies
- `EValidationStrategy.Exception`
  This strategy will throw an `ArgumentException` when the validation fails.
  *This is the default strategy.*
- `EValidationStrategy.Rollback`
  This strategy will behave like the `Ignore` strategy, unless `NotifyPropertyChanged` is also present.
  If the `NotifyPropertyChanged` attribute is present, the event will be raised with no value change.
- `EValidationStrategy.Ignore`
  This strategy will ignore the validation and exit the setter without changing the value.


### On the class  (`EValidationStrategy.Exception`)

```csharp
// User-Code
[ValidationStrategy(EValidationStrategy.Exception)]
public partial class MyClass
{
    [Range(0, 10)]
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            if (value < 0 || value > 10)
                throw new System.ArgumentException("Validation of Field failed: Value must be between 0 and 10", nameof(value));
            _myProperty = value;
        }
    }
}
```

### On the field (`EValidationStrategy.Exception`)

```csharp
// User-Code
public partial class MyClass
{
    [ValidationStrategy(EValidationStrategy.Exception)]
    [Range(0, 10)]
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            if (value < 0 || value > 10)
                throw new System.ArgumentException("Validation of Field failed: Value must be between 0 and 10", nameof(value));
            _myProperty = value;
        }
    }
}
```

### On the class  (`EValidationStrategy.Rollback`)

```csharp
// User-Code
[ValidationStrategy(EValidationStrategy.Rollback)]
[NofityPropertyChanged(true)]
public partial class MyClass
{
    [Range(0, 10)]
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            if (value < 0 || value > 10)
            {
                this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(MyProperty)));
                return;
            }
            _myProperty = value;
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(MyProperty)));
        }
    }
}
```

### On the class  (`EValidationStrategy.Ignore`)

```csharp
// User-Code
[ValidationStrategy(EValidationStrategy.Ignore)]
[NofityPropertyChanged(true)]
public partial class MyClass
{
    [Range(0, 10)]
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            if (value < 0 || value > 10)
                return;
            _myProperty = value;
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(MyProperty)));
        }
    }
}
```

## `PropertyEncapsulationAttribute`
This attribute will make the source generator encapsulate the property with the given access modifier.
It cannot be placed on the class.

Possible values are:
- `EPropertyEncapsulation.Public`
- `EPropertyEncapsulation.Protected`
- `EPropertyEncapsulation.Internal`
- `EPropertyEncapsulation.Private`
- `EPropertyEncapsulation.ProtectedInternal`

### On the field

```csharp
// User-Code
public partial class MyClass
{
    [PropertyEncapsulation(EPropertyEncapsulation.Protected)]
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    protected int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
        }
    }
}
```

## `VirtualPropertyAttribute`
This attribute will make the source generator generate a virtual property.
It cannot be placed on the class.

### On the field

```csharp
// User-Code
public partial class MyClass
{
    [VirtualProperty]
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public virtual int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
        }
    }
}
```

## `EqualityCheckAttribute`
This attribute will change how or if the equality check is performed.

### Supported Equality Modes
- `EEqualityCheckMode.Default`
  This mode will use the default equality check, using `==` for primitive types and `object.Equals` for non-primitive
  types.
  *This is the default mode.*
- `EEqualityCheckMode.Custom`
  This mode will use the given method to check for equality.
  The method must be a method with the signature `bool MethodName(T oldValue, T newValue)`.
  The method must be accessible from the generated code.
  If custom equality check is used, the `Custom` argument must be set to the name of the method or the generated code
  will fall back to the default equality check.
- `EEqualityCheckMode.None`
  This mode will not generate an equality check.

### On the class  (`EEqualityCheckMode.Default`)

```csharp
// User-Code
[EqualityCheck(EEqualityCheckMode.Default)]
public partial class MyClass
{
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
        }
    }
}
```

### On the class  (`EEqualityCheckMode.Custom`)

```csharp
// User-Code
[EqualityCheck(EEqualityCheckMode.Custom, "MyEqualityCheck")]
public partial class MyClass
{
    private static bool MyEqualityCheck(int left, int right) => left == right;
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (MyEqualityCheck(_myProperty, value))
                return;
            _myProperty = value;
        }
    }
}
```

### On the class  (`EEqualityCheckMode.None`)

```csharp
// User-Code
[EqualityCheck(EEqualityCheckMode.None)]
public partial class MyClass
{
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            _myProperty = value;
        }
    }
}
```

### On the field  (`EEqualityCheckMode.Default`)

```csharp
// User-Code
public partial class MyClass
{
    [EqualityCheck(EEqualityCheckMode.Default)]
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (_myProperty == value)
                return;
            _myProperty = value;
        }
    }
}
```

## `GuardAttribute`
The guard attribute allows to make the source generator use custom validation methods to validate the property.
See the [`ValidationStrategyAttribute`](#ValidationStrategyAttribute) for more information on how to change validation
handling.
It cannot be placed on the class.

The guard method must be a method with the signature `bool MethodName(T oldValue, T newValue)` and has to be accessible
from the generated code.
Multiple guards may be used with all of them being checked in the order they are placed on the field.

### On the field

```csharp
// User-Code
namespace SomethingFancy
{
    public static GuardMethods
    {
        public static bool MyGuard(int oldValue, int newValue) => newValue > 0;
    }
}
public partial class MyClass
{
    [Guard("MyGuard", className: "SomethingFancy.GuardMethods")]
    private int _myProperty;
}

// Generated-Code
public partial class MyClass
{
    public int MyProperty
    {
        get => _myProperty;
        set
        {
            if (!SomethingFancy.GuardMethods.MyGuard(_myProperty, value))
                throw new System.ArgumentException("Validation of Field failed: Guard method SomethingFancy.GuardMethods.MyGuard failed", nameof(value));
            _myProperty = value;
        }
    }
}
```