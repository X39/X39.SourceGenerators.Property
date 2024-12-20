using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace X39.SourceGenerators.Property.Generator;

public static class TypedConstantExtensions
{
    public static object? GetValue(this TypedConstant self)
    {
        var value = self.Kind switch
        {
            TypedConstantKind.Error => self.Value,
            TypedConstantKind.Primitive => self.Value,
            TypedConstantKind.Enum => self.Value,
            TypedConstantKind.Type => self.Value,
            TypedConstantKind.Array => self.Values.Select(v => v.GetValue()).ToArray(),
            _ => throw new InvalidEnumArgumentException(nameof(self), (int) self.Kind, typeof(TypedConstantKind))
        };
        return value;
    }

    public static string ToCSharp(this TypedConstant self)
    {
        return self.Kind switch
        {
            TypedConstantKind.Error     => Microsoft.CodeAnalysis.CSharp.TypedConstantExtensions.ToCSharpString(self),
            TypedConstantKind.Primitive => Microsoft.CodeAnalysis.CSharp.TypedConstantExtensions.ToCSharpString(self),
            TypedConstantKind.Enum      => Microsoft.CodeAnalysis.CSharp.TypedConstantExtensions.ToCSharpString(self),
            TypedConstantKind.Type      => Microsoft.CodeAnalysis.CSharp.TypedConstantExtensions.ToCSharpString(self),
            TypedConstantKind.Array => string.Concat(
                "new[] { ",
                string.Join(", ", self.Values.First().Values.Select(ToCSharp)),
                " }"
            ),
            _ => throw new InvalidEnumArgumentException(nameof(self), (int) self.Kind, typeof(TypedConstantKind))
        };
    }
}