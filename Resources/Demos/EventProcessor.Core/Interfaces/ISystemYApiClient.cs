using EventProcessor.Core.Entities;
using System.Threading.Tasks;

namespace EventProcessor.Core.Interfaces
{
    public interface ISystemYApiClient
    {
        Task SendAccountingOperationAsync(AccountingOperation operation);
        Task SendPersonalizationEventAsync(Personalization personalization);
        Task SendUnitReconstructionEventAsync(UnitReconstruction reconstruction);
    }
}
