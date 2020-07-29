using Microsoft.AspNetCore.SignalR;

namespace signalr_nest_practice_hub.Hubs.Base
{
    public abstract class BaseHub : Hub
    {
        protected HubConnectionCache Cache { get; set; }

        protected BaseHub(HubConnectionCache cache)
        {
            Cache = cache;
        }

        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;

            var userId = GetUserId();

            Log.Entity.Message($"Connect userId: {userId} ConnectionId: {connectionId}");

            Cache.AddConnectionId(userId, connectionId);

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            var connectionId = Context.ConnectionId;
            var userId = GetUserId();

            Log.Entity.Message($"Reconnect user userId: {userId} ConnectionId: {connectionId}");

            if (userId != null)
            {
                Cache.AddConnectionId(userId, connectionId);
            }

            return base.OnReconnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            var connectionId = Context.ConnectionId;

            Log.Entity.Message($"Disconnect user {connectionId}");

            if (connectionId != null)
            {
                Cache.RemoveConnectionId(connectionId);
            }


            return base.OnDisconnected(stopCalled);
        }


        protected string GetUserId()
        {
            return Context.User?.Identity?.GetUserId();
        }
    }
}
