using System;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using Environment = NHibernate.Cfg.Environment;

namespace NhSnippets.NhXml.Tests
{
    public class DatabaseFixture : IDisposable
    {
        private bool disposed;
        private static Configuration Configuration;
        private static ISessionFactory SessionFactory;
        protected ISession Session;

        static DatabaseFixture()
        {
            log4net.Config.BasicConfigurator.Configure();
        }

        public DatabaseFixture(Configuration config)
        {
            Configuration = config;
            SessionFactory = Configuration.BuildSessionFactory();
            Session = SessionFactory.OpenSession();

            new SchemaExport(Configuration).Execute(true, true, false, Session.Connection, null);
        }

        public DatabaseFixture(Assembly assemblyContainingMapping)
        {
            Configuration = new Configuration()
                .SetProperty(Environment.ReleaseConnections, "on_close")
                .SetProperty(Environment.Dialect, typeof (SQLiteDialect).AssemblyQualifiedName)
                .SetProperty(Environment.ConnectionDriver, typeof (SQLite20Driver).AssemblyQualifiedName)
                .SetProperty(Environment.ConnectionString, "data source=:memory:")
                .SetProperty(Environment.ShowSql, "true")
                .SetProperty(Environment.FormatSql, "true")
                .SetProperty(Environment.GenerateStatistics, "true")
                .AddAssembly(assemblyContainingMapping);

            SessionFactory = Configuration.BuildSessionFactory();
            Session = SessionFactory.OpenSession();

            new SchemaExport(Configuration).Execute(true, true, false, Session.Connection, null);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DatabaseFixture()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (Session.Transaction.IsActive)
                {
                    Session.Transaction.Rollback();
                }
                Session.Dispose();
            }


            disposed = true;
        }
    }
}
