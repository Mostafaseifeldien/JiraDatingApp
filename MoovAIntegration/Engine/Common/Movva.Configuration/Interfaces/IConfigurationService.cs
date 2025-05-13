using Movva.Configuration.DTO;

namespace Movva.Configuration.Interfaces;

public interface IConfigurationService
{
    IEnumerable<ConfigurationdDto> GetConfigurations(List<string> keys);
}