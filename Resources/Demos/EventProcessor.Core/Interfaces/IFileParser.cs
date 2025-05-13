namespace EventProcessor.Core.Interfaces
{
    public interface IFileParser
    {
        Task<IEnumerable<object>> ParseEventFileAsync(Stream fileStream);
    }
}
