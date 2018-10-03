namespace Collector.Common.RestContracts.Interfaces
{
    /// <summary>
    /// Implement this interface on your request class if you want to control how successful data objects are parsed from the raw response of the server.
    /// </summary>
    public interface ISuccessfulResponseParser
    {
        TResponse ParseResponse<TResponse>(string content);
    }
}