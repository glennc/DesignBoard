using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesignBoard.Services;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;

namespace DesignBoard
{
    public class Startup
    {
        public Startup(IHostingEnvironment hostingEnvironment, IApplicationEnvironment applicationEnvironment)
        {
            Configuration = new ConfigurationBuilder(applicationEnvironment.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{hostingEnvironment.EnvironmentName}.json", optional: true)
                .AddUserSecrets()
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddInstance(Configuration);

            services.AddSingleton<ISearchResultsProvider, SearchResultsProvider>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvcWithDefaultRoute();
        }
    }
}
