using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Letterbook.Generators;

[Generator]
public class TypedIdGenerator : ISourceGenerator
{
	// private static readonly Lazy<SyntaxReceiver> LazyReceiver = new();
	// private SyntaxReceiver Receiver => LazyReceiver.Value;

	// Adjust the namespace for your project
	private const string TypedIdName = "Letterbook.Generators.ITypedId<T>";
	private const string UuidName = "Medo.Uuid7";

	private static string GeneratorName =
		typeof(TypedIdGenerator).FullName ?? throw new InvalidOperationException("Error loading generator type");

	private static readonly ConcurrentDictionary<Type, string> IdTypeConverters = new();


	public void Initialize(GeneratorInitializationContext context)
	{
		// Uncomment the next line to debug the source generator
		System.Diagnostics.Debugger.Launch();
		context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
	}

	public void Execute(GeneratorExecutionContext context)
	{
		if (context.SyntaxReceiver is not SyntaxReceiver receiver)
			return;

		foreach (var declaration in receiver.CandidateDeclarations)
		{
			var model = context.Compilation.GetSemanticModel(declaration.SyntaxTree);
			var type = model.GetDeclaredSymbol(declaration);
			if (type is null)
				continue;

			if (type.AllInterfaces.FirstOrDefault(t => t.ConstructedFrom.ToString() == TypedIdName) is not { } symbol)
				continue;

			AddIdPartial(context, type, symbol);
			AddIdTypeConverter(context, type, symbol);
		}
	}

	private void AddIdTypeConverter(GeneratorExecutionContext context, INamedTypeSymbol type, INamedTypeSymbol symbol)
	{
		var fullNamespace = type.ContainingNamespace.ToDisplayString();
		var typeName = type.Name;
		var isUuid7 = symbol.TypeArguments.SingleOrDefault(arg => arg.ToString() == UuidName) is not null;
		var usingLine = isUuid7 ? "using Medo;" : "";
		var switchLine = isUuid7 ? $"Uuid7 u => new {typeName}(u)," : "";

		// throw new NotImplementedException();
		var source =
			$$"""
			  // <auto-generated from {{GeneratorName}}/>

			  using System;
			  using System.ComponentModel;
			  using System.Globalization;
			  {{usingLine}}

			  namespace {{fullNamespace}};

			  public class {{typeName}}Converter : TypeConverter
			  {
			  	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
			  		sourceType == typeof(string);

			  	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
			  		destinationType == typeof(string);

			  	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			  	{
			  		return value switch
			  		{
			  			string s => new {{typeName}}(Medo.Uuid7.FromId25String(s)),
			  			{{switchLine}}
			  			null => null,
			  			_ => throw new ArgumentException($"Cannot convert from {value} to {{typeName}}", nameof(value))
			  		};
			  	}

			  	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			  	{
			  		if (destinationType == typeof(string))
			  		{
			  			return value switch
			  			{
			  				{{typeName}} id => id.ToString(),
			  				null => null,
			  				_ => throw new ArgumentException($"Cannot convert {value} to string", nameof(value))
			  			};
			  		}

			  		throw new ArgumentException($"Cannot convert {value ?? "(null)"} to {destinationType}", nameof(destinationType));
			  	}
			  }
			  """;
		context.AddSource($"{typeName}Converter.g.cs", source);
	}

	private static void AddIdPartial(GeneratorExecutionContext context, INamedTypeSymbol type, INamedTypeSymbol symbol)
	{
		var fullNamespace = type.ContainingNamespace.ToDisplayString();
		var typeName = type.Name;
		var isUuid7 = symbol.TypeArguments.SingleOrDefault(arg => arg.ToString() == UuidName) is not null;
		var toStringFn = isUuid7
			? "ToId25String()"
			: "ToString()";
		var typeDef = type.IsValueType
			? "record struct"
			: "record";

		var source =
			$$"""
			  // <auto-generated from {{GeneratorName}}/>

			  using System.ComponentModel;

			  namespace {{fullNamespace}}
			  {
			      [TypeConverter(typeof({{typeName}}Converter))]
			      partial {{typeDef}} {{typeName}}
			      {
			          public override string ToString() => Id.{{toStringFn}};
			      }
			  }
			  """;
		context.AddSource($"{typeName}.g.cs", source);
	}

	private class SyntaxReceiver : ISyntaxReceiver
	{
		public List<RecordDeclarationSyntax> CandidateDeclarations { get; } = new();

		public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{
			if (syntaxNode is not RecordDeclarationSyntax declaration) return;
			if (declaration.HasInterface("ITypedId"))
			{
				CandidateDeclarations.Add(declaration);
			}
		}
	}
}