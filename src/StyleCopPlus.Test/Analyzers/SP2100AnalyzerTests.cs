using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Analyzers;
using StyleCopPlus.Test.Helpers;
using TestHelper;

namespace StyleCopPlus.Test.Analyzers
{
    [TestClass]
    public class SP2100AnalyzerTests : DiagnosticVerifier
    {
        [TestMethod]
        public void DoesNotReport_EmptyString()
        {
            var test = string.Empty;

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Reports_LongConstructorDefinition()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2100ConstructorDefinition);
            DiagnosticResult expected = CreateResult(132, 5);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_LongConstructorInvocation()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2100ConstructorInvocation);
            DiagnosticResult expected = CreateResult(173, 17);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_LongMethodDefinition()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2100MethodDefinition);
            DiagnosticResult expected = CreateResult(129, 16);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_LongMethodInvocation()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2100MethodInvocation);
            DiagnosticResult expected = CreateResult(121, 16);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_LongMethodInvocationWithAssignment()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2100MethodInvocationWithAssignment);
            DiagnosticResult expected = CreateResult(134, 16);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_LongFluentApiCalls()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2100FluentApi);
            DiagnosticResult expected = CreateResult(182, 36);

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP2100LineTooLongAnalyzer();
        }

        private DiagnosticResult CreateResult(int exceededLineLength, int lineNumber)
        {
            return new DiagnosticResult
            {
                Id = SP2100LineTooLongAnalyzer.DiagnosticId,
                Message = string.Format(
                    SP2100LineTooLongAnalyzer.MessageFormat,
                    Settings.SP2100MaxLineLengthDefault,
                    exceededLineLength),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[]
                {
                     new DiagnosticResultLocation(
                         "Test0.cs",
                         lineNumber,
                         Settings.SP2100MaxLineLengthDefault + 1)
                }
            };
        }
    }
}
