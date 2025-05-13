namespace Movva.Configuration.DTO;

public class ConfigurationdDto
{
    public int Id { get; set; }
    public required string ConfigurationKey { get; set; }
    public required string ConfigurationValue { get; set; }
}