using System.Collections.Generic;

namespace X39.SourceGenerators.Property;

internal sealed class GenInfo
{
    public bool?                                                            NotifyPropertyChanged { get; set; }
    public bool?                                                            NotifyPropertyChanging { get; set; }
    public string?                                                          ValidationStrategy { get; set; }
    public string?                                                          PropertyName { get; set; }
    public string?                                                          PropertyEncapsulation { get; set; }
    public bool?                                                            VirtualProperty { get; set; }
    public (string type, string from, string to)?                           Range { get; set; }
    public int?                                                             MaxLength { get; set; }
    public (string mode, string epsilonF, string epsilonD, string? custom)? EqualityCheck { get; set; }
    public List<(string methodName, string? className)>                     GuardMethods { get; set; } = new();

    public GenInfo WithDefaults(GenInfo defaultGenInfo)
    {
        return new GenInfo
        {
            NotifyPropertyChanged  = NotifyPropertyChanged ?? defaultGenInfo.NotifyPropertyChanged,
            NotifyPropertyChanging = NotifyPropertyChanging ?? defaultGenInfo.NotifyPropertyChanging,
            ValidationStrategy     = ValidationStrategy ?? defaultGenInfo.ValidationStrategy,
            PropertyName           = PropertyName ?? defaultGenInfo.PropertyName,
            PropertyEncapsulation  = PropertyEncapsulation ?? defaultGenInfo.PropertyEncapsulation,
            VirtualProperty        = VirtualProperty ?? defaultGenInfo.VirtualProperty,
            Range                  = Range ?? defaultGenInfo.Range,
            MaxLength              = MaxLength ?? defaultGenInfo.MaxLength,
            EqualityCheck          = EqualityCheck ?? defaultGenInfo.EqualityCheck,
            GuardMethods           = GuardMethods.Count > 0 ? GuardMethods : defaultGenInfo.GuardMethods
        };
    }

    public bool GenerateProperty()
    {
        return NotifyPropertyChanged is not null
               || NotifyPropertyChanging is not null
               || ValidationStrategy is not null
               || PropertyName is not null
               || PropertyEncapsulation is not null
               || VirtualProperty is not null
               // || Range is not null
               // || MaxLength is not null
               || EqualityCheck is not null
               || GuardMethods.Count > 0
            ;
    }
}
