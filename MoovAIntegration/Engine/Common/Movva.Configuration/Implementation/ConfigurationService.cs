using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Movva.Configuration.DTO;
using Movva.Configuration.Interfaces;
using Movva.DAL;

namespace Movva.Configuration.Implementation;

public class ConfigurationService : IConfigurationService
{
    private static readonly object ConfigLock = new();
    private readonly IConfiguration _configuration;
    private readonly Dictionary<string, object> _configValues = new();

    public ConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
        LoadAllConfigurations();
    }

    public IEnumerable<ConfigurationdDto> GetConfigurations(List<string> keys)
    {
        foreach (var key in keys)
        {
            var propInfo = GetType().GetProperty(key, BindingFlags.Public | BindingFlags.Instance);
            if (propInfo != null)
            {
                var value = propInfo.GetValue(this)?.ToString();
                if (value != null)
                    yield return new ConfigurationdDto
                    {
                        Id = 1,
                        ConfigurationKey = key,
                        ConfigurationValue = value
                    };
            }
        }
    }

    private void LoadAllConfigurations()
    {
        using var context = CreateDbContext();

        lock (ConfigLock)
        {
            var configurations = context.Configuration
                .Where(c => !c.IsDeleted)
                .ToList();

            foreach (var config in configurations)
                if (!_configValues.ContainsKey(config.ConfigurationKey))
                {
                    _configValues[config.ConfigurationKey] = config.ConfigurationValue;
                    SetPropertyValue(config.ConfigurationKey, config.ConfigurationValue);
                }
        }
    }

    private MovvaIntegrationContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<MovvaIntegrationContext>()
            .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
            .Options;

        return new MovvaIntegrationContext(options);
    }

    private void SetPropertyValue(string propertyName, string value)
    {
        var propInfo = GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (propInfo == null || !propInfo.CanWrite)
            return;

        try
        {
            var convertedValue = Convert.ChangeType(value, propInfo.PropertyType);
            propInfo.SetValue(this, convertedValue);
        }
        catch
        {
            // Optionally: log error
        }
    }
}