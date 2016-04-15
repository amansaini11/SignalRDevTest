using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SignalRDevTest.Dal.SqlServerNotifier
{
    public class NotifierEntity
    {
        ICollection<SqlParameter> sqlParameters = new List<SqlParameter>();

        public String SqlQuery { get; set; }

        public String SqlConnectionString { get; set; }

        public ICollection<SqlParameter> SqlParameters
        {
            get
            {
                return sqlParameters;
            }
            set
            {
                sqlParameters = value;
            }
        }

        public static NotifierEntity FromJson(String value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException("NotifierEntity Value can not be null!");
            return new JavaScriptSerializer().Deserialize<NotifierEntity>(value);
        }
    }

    public static class NotifierEntityExtentions
    {
        public static String ToJson(this NotifierEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("NotifierEntity can not be null!");
            return new JavaScriptSerializer().Serialize(entity);
        }
    }

    public class PushSqlDependency
    {
        static PushSqlDependency instance = null;
        readonly SqlDependencyRegister sqlDependencyNotifier;
        readonly Action<String> dispatcher;

        public static PushSqlDependency Instance(NotifierEntity notifierEntity, Action<String> dispatcher)
        {
            if (instance == null)
                instance = new PushSqlDependency(notifierEntity, dispatcher);
            return instance;
        }

        private PushSqlDependency(NotifierEntity notifierEntity, Action<String> dispatcher)
        {
            this.dispatcher = dispatcher;
            sqlDependencyNotifier = new SqlDependencyRegister(notifierEntity);
            sqlDependencyNotifier.SqlNotification += OnSqlNotification;
        }

        internal void OnSqlNotification(object sender, SqlNotificationEventArgs e)
        {
            dispatcher("Refresh");
        }
    }

    public class SqlDependencyRegister
    {
        public event SqlNotificationEventHandler SqlNotification;

        readonly NotifierEntity notificationEntity;

        internal SqlDependencyRegister(NotifierEntity notificationEntity)
        {
            this.notificationEntity = notificationEntity;
            RegisterForNotifications();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2100:Review SQL queries for security vulnerabilities")]
        void RegisterForNotifications()
        {
            using (var sqlConnection = new SqlConnection(notificationEntity.SqlConnectionString))
            using (var sqlCommand = new SqlCommand(notificationEntity.SqlQuery, sqlConnection))
            {
                foreach (var sqlParameter in notificationEntity.SqlParameters)
                    sqlCommand.Parameters.Add(sqlParameter);

                sqlCommand.Notification = null;
                var sqlDependency = new SqlDependency(sqlCommand);
                sqlDependency.OnChange += OnSqlDependencyChange;
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
        }

        void OnSqlDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            if (SqlNotification != null)
                SqlNotification(sender, e);
            RegisterForNotifications();
        }
    }

    public delegate void SqlNotificationEventHandler(object sender, SqlNotificationEventArgs e);
}
