using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDevTest.Dal.SqlServerNotifier;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SignalRDevTest.Web.Hubs
{
    public class MessagesHub : Hub
    {
        internal NotifierEntity NotifierEntity { get; private set; }

        public void DispatchToClient()
        {
            Clients.All.broadcastMessage("Refresh");
        }

        public void Initialize(String value)
        {
            NotifierEntity = NotifierEntity.FromJson(value);
            if (NotifierEntity == null)
                return;
            Action<String> dispatcher = (t) => { DispatchToClient(); };
            PushSqlDependency.Instance(NotifierEntity, dispatcher);
        }
    }
}