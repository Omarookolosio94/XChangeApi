using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;
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
            var jwtSection = Configuration.GetSection("JWTSettings");

            services.AddSingleton(emailConfig);
            services.Configure<JWTSettings>(jwtSection);
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IRegistrationLogService, RegistrationLogService>();
            services.AddScoped<IOtpLogService, OtpLogService>();

            services.AddMvc(options =>
                    {
                        // All endpoints need authorization using our custom authorization filter
                        options.Filters.Add(new CustomAuthorizationFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
                    }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Adding Authenticating
            var JWTSettings = jwtSection.Get<JWTSettings>();
            var key = Encoding.ASCII.GetBytes(JWTSettings.SecretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "XChange.Api",
                    Description = "ECommerce API for XChange.com",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Oghenemaro Okolosio",
                        Email = string.Empty,
                        Url = new Uri("https://oghenemaro.com"),
                    }
                    /*
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        //Url = new Uri("https://example.com/license"),
                    }
                    */
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "XChange.Api v1"));
            }
            else
            {
                app.UseHsts();
            }


            app.UseAuthentication();
            //Error Handler Exception
            app.ConfigureExceptionHandler(env);

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
