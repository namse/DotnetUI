using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DotnetUI.tests
{
    public static class RoslynTestHelper
    {
        public static string GenerateCodeForExpression(string expression)
        {
            return $@"
using System;
using System.Runtime;
using DotnetUI.Core;
using DotnetUI;

namespace Test
{{
    public static class TestClass
    {{
        public static object Func() {{
            return {expression};
        }}
    }}
}}";
        }

        public static T GetGeneratedExpressionCodeReturnValue<T>(
            string generatedCode
        )
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(generatedCode);
            var assemblyName = Path.GetRandomFileName();
            var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll")),
                MetadataReference.CreateFromFile(typeof(DivComponent).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                var errorString = failures.Aggregate("", (current, diagnostic) =>
                    current + $"{diagnostic.Id}: {diagnostic.GetMessage()}\n");
                throw new Exception(errorString);
            }

            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            var testClass = assembly.GetType("Test.TestClass");
            var method = testClass.GetMethod("Func");
            var returnValue = method.Invoke(null, null);
            return (T)returnValue;
        }
    }
}