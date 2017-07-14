namespace Collector.Common.RestContracts.Interfaces
{
    /// <summary>
    /// The interface used for identifying a resource
    /// </summary>
    public interface IResourceIdentifier
    {
        /// <summary>
        /// The Uri for the request
        /// </summary>
        string Uri { get; }
    }
}