namespace Collector.Common.RestContracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Interfaces;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Base request with response.
    /// </summary>
    /// <typeparam name="TResourceIdentifier">The type of the resource identifier.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    public abstract class RequestBase<TResourceIdentifier, TResponse> : RequestBase<TResourceIdentifier>
        where TResourceIdentifier : class, IResourceIdentifier
        where TResponse : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBase{TResourceIdentifier, TResponse}"/> class.
        /// </summary>
        /// <param name="resourceIdentifier"></param>
        protected RequestBase(TResourceIdentifier resourceIdentifier)
            : base(resourceIdentifier)
        {
        }

        public override string GetRawResponseContentForLogging(string rawContent, string contentType)
        {
            if (!IsJsonResponse(contentType))
                return "Response not in json format";

            var sensitiveProperties = SensitiveAttribute.GetSensitiveProperties(typeof(TResponse));
            return sensitiveProperties.Any() 
                ? "Response contains sensitive information" 
                : rawContent;
        }

        public override string GetResponseContentForLogging(string rawContent, string contentType)
        {
            if (!IsJsonResponse(contentType))
                return "Response not in json format";

            var sensitiveProperties = SensitiveAttribute.GetSensitiveProperties(typeof(TResponse));
            return FormatContent(rawContent, ResponseDataRootSelector, sensitiveProperties);
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
            _resourceIdentifier = resourceIdentifier ?? throw new ArgumentNullException(nameof(resourceIdentifier));

            Headers = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        public virtual string Context { get; set; }

        /// <summary>
        /// Get or set request headers
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

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

        public virtual string GetRequestContentForLogging(string rawContent)
        {
            if (GetHttpMethod() == HttpMethod.GET || GetHttpMethod() == HttpMethod.DELETE)
                return string.Empty;

            var sensitiveProperties = SensitiveAttribute.GetSensitiveProperties(GetType());
            return FormatContent(rawContent, RequestDataRootSelector, sensitiveProperties);
        }

        public virtual string GetRawRequestContentForLogging(string rawContent)
        {
            if (GetHttpMethod() == HttpMethod.GET || GetHttpMethod() == HttpMethod.DELETE)
                return string.Empty;

            return SensitiveAttribute.GetSensitiveProperties(GetType()).Any() 
                ? "Request contains sensitive information" 
                : rawContent;
        }

        public virtual string GetRawResponseContentForLogging(string rawContent, string contentType)
        {
            return IsJsonResponse(contentType)
                ? rawContent
                : "Response not in json format";
        }

        public virtual string GetResponseContentForLogging(string rawContent, string contentType)
        {
            return IsJsonResponse(contentType)
                ? FormatContent(rawContent, ResponseDataRootSelector)
                : "Response not in json format";
        }

        /// <summary>
        /// Validates the request.
        /// </summary>
        /// <returns>A list of validation errors</returns>
        protected virtual IEnumerable<string> ValidateRequest()
        {
            return Enumerable.Empty<string>();
        }

        protected static bool IsJsonResponse(string contentType)
        {
            return contentType?.ToLower().Contains("application/json") ?? false;
        }

        protected string FormatContent(string rawContent, Func<JObject, JObject> dataRootSelector, IReadOnlyDictionary<string, SensitiveAttribute> sensitiveProperties = null)
        {
            try
            {
                var jObject = (JObject)JsonConvert.DeserializeObject(rawContent);
                if (sensitiveProperties != null)
                {
                    var dataRoot = dataRootSelector(jObject);

                    foreach (var jProperty in dataRoot.Properties())
                    {
                        if (sensitiveProperties.ContainsKey(jProperty.Name))
                            dataRoot[jProperty.Name] = sensitiveProperties[jProperty.Name].FormatMaskedValue(dataRoot[jProperty.Name]);
                    }
                }

                return JsonConvert.SerializeObject(jObject, Formatting.Indented);
            }
            catch
            {
                return "Could not format the raw content";
            }
        }

        protected virtual JObject ResponseDataRootSelector(JObject jObject) => jObject.Properties().SingleOrDefault(p => p.Name == "Data")?.Value as JObject ?? jObject;

        protected virtual JObject RequestDataRootSelector(JObject jObject) => jObject;
    }
}