// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRequest.cs" company="Collector AB">
//   Copyright © Collector AB. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Collector.Common.RestContracts.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Marker interface for all requests
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Gets the Http method.
        /// </summary>
        /// <returns>The Http method.</returns>
        HttpMethod GetHttpMethod();

        /// <summary>
        /// Gets the key for loading base URL, authentification logic, logging etc..
        /// </summary>
        /// <returns>The configuration key name.</returns>
        string GetConfigurationKey();

        /// <summary>
        /// Validates the request.
        /// </summary>
        /// <returns>A list of validation error infos.</returns>
        IEnumerable<ErrorInfo> GetValidationErrors();
    }
}