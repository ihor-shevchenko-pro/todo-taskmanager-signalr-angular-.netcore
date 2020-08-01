using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using signalr_best_practice_api_models;
using signalr_best_practice_api_models.Models.Notifications;
using signalr_best_practice_core.Configuration;
using signalr_best_practice_core.Interfaces.Managers;

namespace signalr_best_practice_api.Controllers.Base
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected ActionResult<string> SuccessResult()
        {
            return Ok("success");
        }

        protected ActionResult<T> SuccessResult<T>(T model)
        {
            return Ok(model);
        }

        protected virtual Task BroadcastMessageSignalR<T>(NotificationTypeEnum notificationType, T model, bool sendToAll,
            params string[] users)
        {
            string jsonString;
            ModelTypeEnum? modelType;
            IModelTypeManager modelTypeManager;
            IEnumerable<NotificationGetFullApiModel> notificationModels;
            ISignalRNotificationManager notificationManager;

            string userId = GetUserId();

            return Task.Run(() =>
            {
                try
                {
                    notificationManager = (ISignalRNotificationManager)HttpContext.RequestServices.GetService(typeof(ISignalRNotificationManager));
                    modelTypeManager = (IModelTypeManager)HttpContext.RequestServices.GetService(typeof(IModelTypeManager));

                    modelType = modelTypeManager.GetModelType(model);
                    if (modelType == null) return;

                    jsonString = JsonConvert.SerializeObject(model);
                    notificationModels = users.Select(user =>
                    new NotificationGetFullApiModel()
                    {
                        Created = DateTime.UtcNow,
                        Data = jsonString,
                        FromUserId = userId,
                        ToUserId = user,
                        NotificationType = notificationType,
                        NotificationDataType = modelType.Value,
                        NotificationStatus = NotificationStatusEnum.New,
                        Title = "notification",
                        Description = $"from {DateTime.UtcNow}",
                    });

                    if (sendToAll) notificationManager.SendAllAsync(notificationModels);
                    else notificationManager.SendAsync(notificationModels);
                }
                catch (Exception ex)
                {
                    Log.Current.Error(ex);
                }
            });
        }

        protected virtual async Task BroadcastMessageSignalR<T>(NotificationTypeEnum notificationType, IEnumerable<T> models,
            bool sendToAll, Func<T, string[]> recipientPredicate)
        {
            if (recipientPredicate == null) return;

            foreach (T model in models)
            {
                await BroadcastMessageSignalR(notificationType, model, sendToAll, recipientPredicate(model));
            }
        }

        protected virtual string GetUserId()
        {
            return this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}