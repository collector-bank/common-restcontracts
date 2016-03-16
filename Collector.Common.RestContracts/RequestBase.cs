// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestBase.cs" company="Collector AB">
//   Copyright © Collector AB. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Collector.Common.RestContracts
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using Interfaces;

    /// <summary>
    /// Base request without response.
    /// </summary>
    /// <typeparam name="TResourceIdentifier">The type of the resource identifier.</typeparam>
    public abstract class RequestBase<TResourceIdentifier> : IRequest
           where TResourceIdentifier : class, IResourceIdentifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBase{TResourceIdentifier}"/> class.
        /// </summary>
        protected RequestBase(TResourceIdentifier resourceIdentifier)
        {
            ResourceIdentifier = resourceIdentifier ?? (TResourceIdentifier)FormatterServices.GetUninitializedObject(typeof(TResourceIdentifier));
        }

        /// <summary>
        /// Gets the resource identifier.
        /// </summary>
        public TResourceIdentifier ResourceIdentifier { get; private set; }

        internal abstract HttpMethod HttpMethod { get; }
    }

    /// <summary>
    /// Base request with response.
    /// </summary>
    /// <typeparam name="TResourceIdentifier">The type of the resource identifier.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class RequestBase<TResourceIdentifier, TResponse> : RequestBase<TResourceIdentifier>
        where TResourceIdentifier : class, IResourceIdentifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBase{TResourceIdentifier, TResponse}"/> class.
        /// </summary>
        /// <param name="resourceIdentifier"></param>
        protected RequestBase(TResourceIdentifier resourceIdentifier) : base(resourceIdentifier)
        {            
        }
    }
}