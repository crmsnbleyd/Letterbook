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
			AddIdTypeJsonConverter(context, type, symbol);
		}
	}

	private void AddIdTypeJsonConverter(GeneratorExecutionContext context, INamedTypeSymbol type, INamedTypeSymbol symbol)
	{
		var fullNamespace = type.ContainingNamespace.ToDisplayString();
		var idType = symbol.TypeArguments.Single();
		var typeName = type.Name;
		var source =
			$$"""
			  // <auto-generated from {{GeneratorName}}/>

			  using System.Text.Json;
			  using System.Text.Json.Serialization;

			  namespace {{fullNamespace}};

			  public class {{typeName}}JsonConverter : JsonConverter<{{typeName}}>
			  {
			      /// <inheritdoc />
			      public override {{typeName}} Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			      {
			     		try
			     		{
			     			var value = JsonSerializer.Deserialize<{{idType}}>(ref reader, options);
			     			return new {{typeName}}(value);
			     		}
			     		catch (Exception e)
			     		{
			     			throw new JsonException("Unable to convert to {{typeName}}", e);
			     		}
			      }

			      /// <inheritdoc />
			      public override void Write(Utf8JsonWriter writer, {{typeName}} value, JsonSerializerOptions options)
			      {
			            JsonSerializer.Serialize(writer, value.Id, options);
			      }
			  }
			  """;
		context.AddSource($"{typeName}JsonConverter.g.cs", source);
	}

	private void AddIdTypeConverter(GeneratorExecutionContext context, INamedTypeSymbol type, INamedTypeSymbol symbol)
	{
		var fullNamespace = type.ContainingNamespace.ToDisplayString();
		var typeName = type.Name;
		var idType = symbol.TypeArguments.Single();
		var switchLine = idType.ToString() == "string" ? "" : $"{idType} t => new {typeName}(t),";

		var source =
			$$"""
			  // <auto-generated from {{GeneratorName}}/>

			  using System;
			  using System.ComponentModel;
			  using System.Globalization;


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
			  			string s => new {{typeName}}({{typeName}}.FromString(s)),
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
			  		else if (destinationType == typeof({{idType}}))
			  		{
			  			return value switch
			  			{
			  				{{typeName}} id => id.Id,
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
		var idType = symbol.TypeArguments.Single();
		var isUuid7 = idType.ToString() == UuidName;
		var toStringFn = isUuid7
			? "ToId25String()"
			: "ToString()";
		var fromStringFn = isUuid7
			? "Medo.Uuid7.FromId25String(s)"
			: $"idType.Parse(s)";
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
			          public static {{idType}} FromString(string s) => {{fromStringFn}};
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