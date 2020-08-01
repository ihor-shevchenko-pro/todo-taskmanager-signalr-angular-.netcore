using signalr_best_practice_core.Models.SignalR;
using System.Collections.Generic;

namespace signalr_best_practice_core.Interfaces.Cache
{
    public interface IHubConnectionCache
    {
        HubUser GetUserByConnectionId(string connectionId);
        List<HubUser> GetAllUsers();
        HubUser GetUserByUserId(string userId);
        IEnumerable<HubUser> GetUsersByUserIds(IEnumerable<string> userIds);
        IEnumerable<string> GetConnectionIds(params string[] userIds);
        void AddConnectionId(string userId, string connectionId);
        void RemoveUsersByUserId(string userId);
        void RemoveConnectionId(string connectionId);
    }
}
