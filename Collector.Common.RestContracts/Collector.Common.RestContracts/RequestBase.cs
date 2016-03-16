// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestBase.cs" company="Collector AB">
//   Copyright © Collector AB. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Collector.Common.RestContracts
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using Contracts;
    using Interfaces;
    using Newtonsoft.Json;

    /// <summary>
    /// Base request without response
    /// </summary>
    /// <typeparam name="TResourceIdentifier">The type of the resource identifier.</typeparam>
    public abstract class RequestBase<TResourceIdentifier> : IRequest
           where TResourceIdentifier : class, IResourceIdentifier
    {
        /// <summary>
        /// The temporary long max value.
        /// </summary>
        protected const long TEMPORARY_LONG_MAXVALUE = long.MaxValue - 512; // https://github.com/AutoFixture/AutoFixture/issues/453

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBase{TResourceIdentifier}"/> class.
        /// </summary>
        protected RequestBase(TResourceIdentifier resourceIdentifier)
        {
            ResourceIdentifier = resourceIdentifier ?? (TResourceIdentifier)FormatterServices.GetUninitializedObject(typeof(TResourceIdentifier));
        }

        /// <summary>
        /// Gets or sets the user performing the request.
        /// </summary>
        [Required(ErrorMessage = RestErrorCodes.PERFORMED_BY_MISSING)]
        [StringLength(50, ErrorMessage = RestErrorCodes.PERFORMED_BY_LENGTH)]
        public string PerformedBy { get; set; }

        /// <summary>
        /// Gets the resource identifier.
        /// </summary>
        [JsonIgnore]
        public TResourceIdentifier ResourceIdentifier { get; private set; }

        internal abstract HttpMethod HttpMethod { get; }
    }

    /// <summary>
    /// Base request with response
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
