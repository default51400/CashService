using System.Threading;
using Infrastructure.Models;
using Infrastructure.Models.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CashServiceHandler
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = configuration.GetConnectionString("SQLServer");

            services.AddTransient<ICashOrderRepository, CashOrderRepository>(provider =>
                new CashOrderRepository(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICashOrderRepository cor)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Thread receiverThread = new Thread(() => StartReceiver(cor));
            receiverThread.Start();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Cash service handler started!");
                });
            });
        }

        public void StartReceiver(ICashOrderRepository repository)
        {
            Receiver rec = new Receiver(configuration, repository);
            rec.Start();
        }
    }
}