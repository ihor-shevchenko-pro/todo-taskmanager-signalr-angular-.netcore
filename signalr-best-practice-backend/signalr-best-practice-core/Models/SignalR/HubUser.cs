using System;
using System.Collections.Generic;
using System.Text;

namespace signalr_best_practice_core.Models.SignalR
{
    public class HubUser
    {
        public string UserId { get; set; }

        public List<string> ConnectionIds { get; set; } = new List<string>();
    }
}
