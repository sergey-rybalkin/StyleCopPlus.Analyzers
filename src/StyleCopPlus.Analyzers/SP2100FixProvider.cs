﻿using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// Tries to fix SP2100 warning by splitting long code lines where possible.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SP2100FixProvider)), Shared]
    public class SP2100FixProvider : CodeFixProvider
    {
        private const string Title = "Format code";

        /// <summary>
        /// Gets a list of diagnostic IDs that this provider can provider fixes for.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(SP2100Analyzer.DiagnosticId); }
        }

        /// <summary>
        /// Gets an optional <see cref="T:Microsoft.CodeAnalysis.CodeFixes.FixAllProvider" /> that can fix
        /// all/multiple occurrences of diagnostics fixed by this code fix provider. Return null if the
        /// provider doesn't support fix all/multiple occurrences. Otherwise, you can return any of the well
        /// known fix all providers from
        /// <see cref="T:Microsoft.CodeAnalysis.CodeFixes.WellKnownFixAllProviders" /> or implement your own
        /// fix all provider.
        /// </summary>
        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        /// <summary>
        /// Computes one or more fixes for the specified
        /// <see cref="T:Microsoft.CodeAnalysis.CodeFixes.CodeFixContext" />.
        /// </summary>
        /// <param name="context">
        /// A <see cref="T:Microsoft.CodeAnalysis.CodeFixes.CodeFixContext" /> containing context
        /// information about the diagnostics to fix. The context must only contain diagnostics with an
        /// <see cref="P:Microsoft.CodeAnalysis.Diagnostic.Id" /> included in the
        /// <see cref="P:Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider.FixableDiagnosticIds" /> for the
        /// current provider.
        /// </param>
        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document
                                    .GetSyntaxRootAsync(context.CancellationToken)
                                    .ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedSolution: c => FormatCodeAsync(context.Document, c),
                    equivalenceKey: Title),
                diagnostic);
        }

        private async Task<Solution> FormatCodeAsync(Document document, CancellationToken c)
        {
            return document.Project.Solution;
        }
    }
}