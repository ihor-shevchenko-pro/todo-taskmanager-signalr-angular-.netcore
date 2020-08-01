using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using signalr_best_practice_api_models.Models.Notifications;
using signalr_best_practice_core.Configuration;
using signalr_best_practice_core.Interfaces.Cache;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Models.SignalR;
using signalr_best_practice_signalr.Hubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace signalr_best_practice_signalr.Managers
{
    public class SignalRNotificationManager : ISignalRNotificationManager
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IHubConnectionCache _cache;

        public SignalRNotificationManager(IHubConnectionCache cache, IHubContext<NotificationHub> hubContext)
        {
            _cache = cache;
            _hubContext = hubContext;
            Log.Current.Message($"Initialize push notification manager cache: {_cache != null}");
        }


        public Task SendAsync(NotificationGetFullApiModel model)
        {
            HubUser user;

            if (model == null) throw new ArgumentNullException(nameof(model));

            try
            {
                user = _cache.GetUserByUserId(model.ToUserId);
                if (user == null) return Task.FromResult(1);

                foreach (string connection in user.ConnectionIds)
                {
                    try
                    {
                        _hubContext.Clients.Client(connection).SendAsync("HandleNotification", model);
                    }
                    catch (NullReferenceException ex)
                    {
                        Log.Current.Error(ex);
                    }
                }
            }
            catch (Exception er)
            {
                Log.Current.Error(er);
            }

            return Task.FromResult("Success");
        }

        public Task SendAsync(IEnumerable<NotificationGetFullApiModel> models)
        {
            try
            {
                if (models == null) throw new ArgumentNullException(nameof(models));

                foreach (var notification in models)
                {
                    var user = _cache.GetUserByUserId(notification.ToUserId);

                    #if DEV
                    LogPush(notification, user);
                    #endif

                    if (user == null) continue;
                    foreach (var connection in user.ConnectionIds)
                    {
                        try
                        {
                            _hubContext.Clients.Client(connection).SendAsync("HandleNotification", notification);
                        }
                        catch (NullReferenceException ex)
                        {
                            Log.Current.Error(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Current.Error(ex);
            }

            return Task.FromResult("Success");
        }

        public Task SendAllAsync(NotificationGetFullApiModel model)
        {
            try
            {
                _hubContext.Clients.All.SendAsync("HandleNotification", model);
            }
            catch (Exception ex)
            {
                Log.Current.Error(ex);
            }

            return Task.FromResult("Success");
        }

        public Task SendAllAsync(IEnumerable<NotificationGetFullApiModel> models)
        {
            try
            {
                foreach (var notification in models)
                {
                    _hubContext.Clients.All.SendAsync("HandleNotification", notification);
                }
            }
            catch (Exception ex)
            {
                Log.Current.Error(ex);
            }
            return Task.FromResult("Success");
        }


        private void LogNotification(NotificationGetFullApiModel notification, HubUser user = null)
        {
            try
            {
                Log.Current.Message($@"
                ================ Push Notification log ================
                User: {notification.ToUserId}
                Model type: {notification.NotificationDataType}
                Notification type: {notification.NotificationType}
                Connectioin ids: {JsonConvert.SerializeObject(user?.ConnectionIds)}
                ================ End of log ================
                ");
            }
            catch (Exception) { }
        }
    }
}
