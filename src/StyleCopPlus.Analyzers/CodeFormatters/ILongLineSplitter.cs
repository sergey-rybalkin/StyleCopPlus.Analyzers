﻿using Microsoft.CodeAnalysis;

namespace StyleCopPlus.Analyzers.CodeFormatters
{
    /// <summary>
    /// Defines interface for splitting long code lines into several short lines and applying predefined
    /// formatting to match max line length requirement.
    /// </summary>
    internal interface ILongLineSplitter
    {
        /// <summary>
        /// Splits long code line into several smaller lines in order to match max line length requirement.
        /// </summary>
        void SplitCodeLine();
    }
}