using SignalRDevTest.Dal.Entity;
using SignalRDevTest.Dal.SqlServerNotifier;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRDevTest.Dal.Model
{
    public static class DbContextExtensions
    {
        public static ObjectContext UnderlyingContext(this DbContext context)
        {
            return ((IObjectContextAdapter)context).ObjectContext;
        }

        public static NotifierEntity GetNotifierEntity<TEntity>
        (this DbContext dbContext, IQueryable iQueryable) where TEntity : EntityBase
        {
            var objectQuery = dbContext.GetObjectQuery<TEntity>(iQueryable);
            return new NotifierEntity()
            {
                SqlQuery = objectQuery.ToTraceString(),
                SqlConnectionString = objectQuery.SqlConnectionString(),
                SqlParameters = objectQuery.SqlParameters()
            };
        }

        public static ObjectQuery GetObjectQuery<TEntity>
        (this DbContext dbContext, IQueryable query) where TEntity : EntityBase
        {
            if (query is ObjectQuery)
                return query as ObjectQuery;

            if (dbContext == null)
                throw new ArgumentException("dbContext cannot be null");

            var objectSet = dbContext.UnderlyingContext().CreateObjectSet<TEntity>();
            var iQueryProvider = ((IQueryable)objectSet).Provider;

            // Use the provider and expression to create the ObjectQuery.
            return (ObjectQuery)iQueryProvider.CreateQuery(query.Expression);
        }
    }
    public static class ObjectQueryExtensions
    {
        public static String SqlString(this ObjectQuery objectQuery)
        {
            if (objectQuery == null)
                throw new ArgumentException("objectQuery cannot be null");

            return objectQuery.ToTraceString();
        }

        public static String SqlConnectionString(this ObjectQuery objectQuery)
        {
            if (objectQuery == null)
                throw new ArgumentException("objectQuery cannot be null");

            var dbConnection = objectQuery.Context.Connection;
            return ((System.Data.Entity.Core.EntityClient.EntityConnection)dbConnection).StoreConnection.ConnectionString;
        }

        public static ICollection<SqlParameter> SqlParameters(this ObjectQuery objectQuery)
        {
            if (objectQuery == null)
                throw new ArgumentException("objectQuery cannot be null");

            var collection = new List<SqlParameter>();
            foreach (ObjectParameter parameter in objectQuery.Parameters)
                collection.Add(new SqlParameter(parameter.Name, parameter.Value));
            return collection;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2100:Review SQL queries for security vulnerabilities")]
        public static SqlCommand SqlCommand(this ObjectQuery objectQuery)
        {
            if (objectQuery == null)
                throw new ArgumentException("objectQuery cannot be null");

            var sqlCommand = new SqlCommand(objectQuery.SqlConnectionString(),
            new SqlConnection(objectQuery.SqlConnectionString()));
            foreach (ObjectParameter parameter in objectQuery.Parameters)
                sqlCommand.Parameters.AddWithValue(parameter.Name, parameter.Value);

            return sqlCommand;
        }
    }
}
