using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using signalr_best_practice_core.Configuration;
using signalr_best_practice_core.Interfaces.Cache;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace signalr_best_practice_signalr.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub : Hub
    {
        protected IHubConnectionCache Cache { get; set; }

        public NotificationHub(IHubConnectionCache cache)
        {
            Cache = cache;
            Log.Current.Message("Notification hub created");
        }

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var userId = GetUserId();
            Cache.AddConnectionId(userId, connectionId);

            Log.Current.Message($"Connect userId: {userId} - ConnectionId: {connectionId}");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            var userId = GetUserId();
            if (connectionId != null)
            {
                Cache.RemoveConnectionId(connectionId);
            }

            Log.Current.Message($"Disconnect user: {userId} - ConnectionId {connectionId}");

            return base.OnDisconnectedAsync(exception);
        }

        protected string GetUserId()
        {
            //return this.Context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return this.Context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
