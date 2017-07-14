namespace Collector.Common.RestContracts
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.Serialization;
    using Interfaces;

    /// <summary>
    /// Base request with response.
    /// </summary>
    /// <typeparam name="TResourceIdentifier">The type of the resource identifier.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    // ReSharper disable once UnusedTypeParameter
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

    /// <summary>
    /// Base request without response.
    /// </summary>
    /// <typeparam name="TResourceIdentifier">The type of the resource identifier.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class RequestBase<TResourceIdentifier> : IRequest<TResourceIdentifier>
           where TResourceIdentifier : class, IResourceIdentifier
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        // Needs to be set by reflection in the Request Binder
        private TResourceIdentifier _resourceIdentifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBase{TResourceIdentifier}"/> class.
        /// </summary>
        protected RequestBase(TResourceIdentifier resourceIdentifier)
        {
            _resourceIdentifier = resourceIdentifier ?? (TResourceIdentifier)FormatterServices.GetUninitializedObject(typeof(TResourceIdentifier));
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Gets the Http method.
        /// </summary>
        /// <returns>The Http method.</returns>
        public abstract HttpMethod GetHttpMethod();

        /// <summary>
        /// Gets the resource identifier.
        /// </summary>
        /// <returns></returns>
        public TResourceIdentifier GetResourceIdentifier() => _resourceIdentifier;

        /// <summary>
        /// Gets the key for loading base URL, authentication logic, logging etc..
        /// </summary>
        /// <returns>The configuration key name.</returns>
        public abstract string GetConfigurationKey();

        /// <summary>
        /// Validates the request.
        /// </summary>
        /// <returns>A list of validation error info's.</returns>
        public virtual IEnumerable<ErrorInfo> GetValidationErrors()
        {
            return ValidateRequest()?.Select(reason => new ErrorInfo(reason, "VALIDATION_ERROR"));
        }

        /// <summary>
        /// Validates the request.
        /// </summary>
        /// <returns>A list of validation errors</returns>
        protected virtual IEnumerable<string> ValidateRequest()
        {
            return Enumerable.Empty<string>();
        }
    }
}