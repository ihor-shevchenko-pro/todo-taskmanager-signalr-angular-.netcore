using signalr_best_practice_api_models.Models.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace signalr_best_practice_core.Interfaces.Managers
{
    public interface ISignalRNotificationManager
    {
        Task SendAsync(NotificationGetFullApiModel model);
        Task SendAsync(IEnumerable<NotificationGetFullApiModel> models);

        Task SendAllAsync(NotificationGetFullApiModel model);
        Task SendAllAsync(IEnumerable<NotificationGetFullApiModel> models);
    }
}
