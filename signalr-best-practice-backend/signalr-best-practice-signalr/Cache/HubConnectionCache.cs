using signalr_best_practice_core.Configuration;
using signalr_best_practice_core.Interfaces.Cache;
using signalr_best_practice_core.Models.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace signalr_best_practice_signalr.Cache
{
    public class HubConnectionCache : IHubConnectionCache
    {
        protected object UsersLocker { get; } = new object();
        protected List<HubUser> Users { get; }

        public HubConnectionCache()
        {
            Users = new List<HubUser>();
            Log.Current.Message(">>> Create Hub Connection Cache");
        }

        ~HubConnectionCache()
        {
            Log.Current.Message(">>> Destruct Hub Connection Cache");
        }

        public virtual HubUser GetUserByConnectionId(string connectionId)
        {
            lock (UsersLocker)
            {
                return Users.FirstOrDefault(x => x.ConnectionIds.Any(c => c == connectionId));
            }
        }

        public virtual List<HubUser> GetAllUsers()
        {
            lock (UsersLocker)
            {
                return Users.ToList();
            }
        }

        public virtual HubUser GetUserByUserId(string userId)
        {
            lock (UsersLocker)
            {
                return Users.FirstOrDefault(x => x.UserId == userId);
            }
        }

        public virtual IEnumerable<HubUser> GetUsersByUserIds(IEnumerable<string> userIds)
        {
            lock (UsersLocker)
            {
                return Users.Where(x => userIds.Contains(x.UserId));
            }
        }

        public virtual IEnumerable<string> GetConnectionIds(params string[] userIds)
        {
            lock (UsersLocker)
            {
                var temp = Users.Where(x => userIds.Contains(x.UserId));
                return temp.SelectMany(x => x.ConnectionIds);
            }
        }

        public virtual void AddConnectionId(string userId, string connectionId)
        {
            lock (UsersLocker)
            {
                var user = Users.FirstOrDefault(x => x.UserId == userId);

                if (user == null)
                {
                    user = new HubUser()
                    {
                        UserId = userId,
                    };
                    Users.Add(user);
                }

                if (user.ConnectionIds.Any(x => x == connectionId) == false)
                {
                    user.ConnectionIds.Add(connectionId);
                }
            }
        }

        public virtual void RemoveUsersByUserId(string userId)
        {
            lock (UsersLocker)
            {
                Users.RemoveAll(x => x.UserId == userId);
            }
        }

        public virtual void RemoveConnectionId(string connectionId)
        {
            lock (UsersLocker)
            {
                var user = Users.FirstOrDefault(x => x.ConnectionIds.Any(y => y == connectionId));

                if (user != null)
                {
                    user.ConnectionIds.RemoveAll(x => x == connectionId);
                }
            }
        }
    }
}
