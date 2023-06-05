using Reclone_BackEnd.Models;

namespace Reclone_BackEnd.ServiceBus
{
    public interface IServiceBus
    {
        Task SendMessageAsync(Image imageDetails );
    }
}
