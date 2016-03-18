// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Error.cs" company="Collector AB">
//   Copyright © Collector AB. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Collector.Common.RestContracts
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an error in the response.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="errors">Additional information about the errors.</param>
        public Error(string code, string message = null, IEnumerable<ErrorInfo> errors = null)
        {
            Code = code;
            Message = message;
            Errors = errors;
        }

        /// <summary>
        /// Gets the code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the additional information about the errors.
        /// </summary>
        public IEnumerable<ErrorInfo> Errors { get; set; }
    }
}