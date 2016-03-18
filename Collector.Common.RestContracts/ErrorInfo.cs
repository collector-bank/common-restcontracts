// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorInfo.cs" company="Collector AB">
//   Copyright © Collector AB. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Collector.Common.RestContracts
{
    /// <summary>
    /// Additional error information.
    /// </summary>
    public class ErrorInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorInfo"/> class.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="message">The message.</param>
        public ErrorInfo(string reason, string message)
        {
            Reason = reason;
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorInfo"/> class.
        /// </summary>
        public ErrorInfo()
        {
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the error reason. 
        /// </summary>
        public string Reason { get; set; }
    }
}