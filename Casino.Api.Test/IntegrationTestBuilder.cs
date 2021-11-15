using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;

namespace CasinoApi.Test
{
    public abstract class IntegrationTestBuilder : IDisposable
    {
        protected HttpClient TestClient;
        private bool Disposed;

        protected IntegrationTestBuilder()
        {
            BootstrapTestingSuite();
        }

        protected void BootstrapTestingSuite()
        {
            Disposed = false;
            var appFactory = new WebApplicationFactory<Casino.Api.Startup>();
            TestClient = appFactory.CreateClient();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;
            if (disposing)
            {
                TestClient.Dispose();
            }
            Disposed = true;
        }
    }
}
