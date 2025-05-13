using Movva.Data;
using Movva.Data.Interfaces;

namespace Movva.DAL.Entities.Shared;

public class Configuration : BaseAuditableEntity, IAuditable
{
    public required string ConfigurationKey { get; set; }
    public required string ConfigurationValue { get; set; }
}