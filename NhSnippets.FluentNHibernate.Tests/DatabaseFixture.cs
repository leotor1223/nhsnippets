using System;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Xunit;

namespace NhSnippets.FluentNHibernate.Tests
{
    public class DatabaseFixture : IDisposable
    {
        private bool disposed;
        private static ISessionFactory Factory;
        protected ISession Session;
        private Configuration Configuration;

        static DatabaseFixture()
        {
            log4net.Config.BasicConfigurator.Configure();
        }

        public DatabaseFixture(Assembly mappingAssembly)
        {
            Assert.NotNull(typeof(System.Data.SQLite.SQLiteConnection));

            Factory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
                .Diagnostics(d => d.OutputToConsole())
                .Mappings(m =>
                {
                    m.FluentMappings
                        .AddFromAssembly(mappingAssembly)
                        .Conventions.AddAssembly(mappingAssembly);
                })
                .ExposeConfiguration(c => Configuration = c)
                .BuildSessionFactory();

            Session = Factory.OpenSession();

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
