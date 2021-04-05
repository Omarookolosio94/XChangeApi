using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using XChange.Api.DTO;
using XChange.Api.Extensions;
using XChange.Api.Services.Concretes;
using XChange.Api.Services.Interfaces;
using XChange.Data.Services.Concretes;

namespace XChange.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IRegistrationLogService, RegistrationLogService>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "XChange.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookLibrary.WebApi v1"));
            }
            else
            {
                app.UseHsts();
            }

            //Error Handler Exception
            app.ConfigureExceptionHandler(env);

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
