using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// SP2100 rule analyzer - validates that code lines do not exceed configured length.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SP2101Analyzer : StyleCopPlusAnalyzer
    {
        public const string DiagnosticId = "SP2101";

        public const string Category = "Maintainability";

        public const string Title = "Method is too long.";

        public const string MessageFormat = "Method body should not exceed {0} lines (now {1}).";

        public const string Description = "Method body should not exceed {0} lines.";

        private static DiagnosticDescriptor _rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: string.Format(Description, Settings.SP2101MaxMethodLength));

        /// <summary>
        /// Gets a set of descriptors for the diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(_rule); }
        }

        /// <summary>
        /// Called once at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context">Analysis context to register actions in.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCodeBlockAction(HandleCodeBlock);
        }

        private static void HandleCodeBlock(CodeBlockAnalysisContext context)
        {
            if (!IsInsideMethod(context))
                return;

            int lines = GetNumberOfLinesInCodeBlock(context.CodeBlock);
            if (lines <= Settings.SP2101MaxMethodLength)
                return;

            // Report diagnostic on the symbol definition that is in the same file as its long body.
            Location location =
                context.OwningSymbol.Locations.First(l => l.SourceTree == context.CodeBlock.SyntaxTree);

            Diagnostic result = Diagnostic.Create(
                _rule,
                location,
                Settings.SP2101MaxMethodLength,
                lines);

            context.ReportDiagnostic(result);
        }
    }
}