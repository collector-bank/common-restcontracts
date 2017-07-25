namespace Collector.Common.RestContracts.Interfaces
{
    using Collector.Common.RestContracts;
    
    /// <summary>
    /// Implement this interface on your request class if you want to control how errors are parsed from the raw response of the server.
    /// </summary>
    public interface IErrorResponseParser
    {
        Error ParseError(string content);
    }
}