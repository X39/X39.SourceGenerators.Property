<!-- TOC -->
* [X39.SourceGenerators.Property](#x39sourcegeneratorsproperty)
* [Quick Start](#quick-start)
* [When and how are properties generated?](#when-and-how-are-properties-generated)
* [Attributes](#attributes)
  * [`NotifyPropertyChangedAttribute`](#notifypropertychangedattribute)
    * [On the class with true](#on-the-class-with-true)
    * [On the class with false](#on-the-class-with-false)
    * [On the field with true](#on-the-field-with-true)
    * [On the field with false](#on-the-field-with-false)
  * [`NotifyPropertyChangingAttribute`](#notifypropertychangingattribute)
    * [On the class with true](#on-the-class-with-true-1)
    * [On the class with false](#on-the-class-with-false-1)
    * [On the field with true](#on-the-field-with-true-1)
    * [On the field with false](#on-the-field-with-false-1)
  * [`PropertyNameAttribute`](#propertynameattribute)
    * [On the field](#on-the-field)
  * [`ValidationStrategyAttribute`](#validationstrategyattribute)
    * [Supported Validations](#supported-validations)
    * [Available Strategies](#available-strategies)
    * [On the class  (`EValidationStrategy.Exception`)](#on-the-class-evalidationstrategyexception)
    * [On the field (`EValidationStrategy.Exception`)](#on-the-field-evalidationstrategyexception)
    * [On the class  (`EValidationStrategy.Rollback`)](#on-the-class-evalidationstrategyrollback)
    * [On the class  (`EValidationStrategy.Ignore`)](#on-the-class-evalidationstrategyignore)
  * [`PropertyEncapsulationAttribute`](#propertyencapsulationattribute)
    * [On the field](#on-the-field-1)
  * [`VirtualPropertyAttribute`](#virtualpropertyattribute)
    * [On the field](#on-the-field-2)
  * [`EqualityCheckAttribute`](#equalitycheckattribute)
    * [Supported Equality Modes](#supported-equality-modes)
    * [On the class  (`EEqualityCheckMode.Default`)](#on-the-class-eequalitycheckmodedefault)
    * [On the class  (`EEqualityCheckMode.Custom`)](#on-the-class-eequalitycheckmodecustom)
    * [On the class  (`EEqualityCheckMode.None`)](#on-the-class-eequalitycheckmodenone)
    * [On the field  (`EEqualityCheckMode.Default`)](#on-the-field-eequalitycheckmodedefault)
  * [`GuardAttribute`](#guardattribute)
    * [On the field](#on-the-field-3)
* [Project Notes](#project-notes)
  * [Building](#building)
  * [Test coverage](#test-coverage)
  * [Contributing](#contributing)
    * [Code of Conduct](#code-of-conduct)
    * [Contributors Agreement](#contributors-agreement)
  * [License](#license)
<!-- TOC -->

# X39.SourceGenerators.Property

This library adds a source generator that generates properties for a given class.

# Quick Start

***!IMPORTANT!***: After installing the package, check whether the build output contains a `CS9057` warning.
If it does, your compiler is not updated enough to support this source generator.
Install the latest .NET SDK to fix this.

***!FOR RIDER USERS!*** You may have to restart Rider after installing the package to make the source generator work.

-----

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

# Project Notes

This project is part of my personal utility libraries i use in my projects.
The following few paragraphs are meant to give you an overview of the project and how you can contribute to it.

## Building

This project uses GitHub Actions for continuous integration. The workflow is defined in `.github/workflows/main.yml`. It
includes steps for restoring dependencies, building the project, and publishing a NuGet package.

## Test coverage

This project is covered by unit tests for the generator only.
This means that the generated code is not yet tested.

## Contributing

Contributions are welcome!
Please submit a pull request or create a discussion to discuss any changes you wish to make.

### Code of Conduct

Be excellent to each other.

### Contributors Agreement

First of all, thank you for your interest in contributing to this project!
Please add yourself to the list of contributors in the [CONTRIBUTORS](CONTRIBUTORS.md) file when submitting your
first pull request.
Also, please always add the following to your pull request:

```
By contributing to this project, you agree to the following terms:
- You grant me and any other person who receives a copy of this project the right to use your contribution under the
  terms of the GNU Lesser General Public License v3.0.
- You grant me and any other person who receives a copy of this project the right to relicense your contribution under
  any other license.
- You grant me and any other person who receives a copy of this project the right to change your contribution.
- You waive your right to your contribution and transfer all rights to me and every user of this project.
- You agree that your contribution is free of any third-party rights.
- You agree that your contribution is given without any compensation.
- You agree that I may remove your contribution at any time for any reason.
- You confirm that you have the right to grant the above rights and that you are not violating any third-party rights
  by granting these rights.
- You confirm that your contribution is not subject to any license agreement or other agreement or obligation, which
  conflicts with the above terms.
```

This is necessary to ensure that this project can be licensed under the GNU Lesser General Public License v3.0 and
that a license change is possible in the future if necessary (e.g., to a more permissive license).
It also ensures that I can remove your contribution if necessary (e.g., because it violates third-party rights) and
that I can change your contribution if necessary (e.g., to fix a typo, change implementation details, or improve
performance).
It also shields me and every user of this project from any liability regarding your contribution by deflecting any
potential liability caused by your contribution to you (e.g., if your contribution violates the rights of your
employer).
Feel free to discuss this agreement in the discussions section of this repository, i am open to changes here (as long as
they do not open me or any other user of this project to any liability due to a **malicious contribution**).

## License

This project is licensed under the GNU Lesser General Public License v3.0. See the [LICENSE](LICENSE) file for details.