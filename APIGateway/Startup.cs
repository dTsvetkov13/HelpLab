using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Logging;
using APIGateway.Services;

namespace APIGateway
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
            services.AddOptions();

            IdentityModelEventSource.ShowPII = true;

            services.AddSingleton<RabbitMqService>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
                    builder.WithOrigins("https://localhost:44341", "example.com").AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer",
                                  options =>
                                  {
                                      options.Authority = "https://localhost:44337"; //Url where the IdentityServer is hosted

                                      options.Audience = "Microservices.Users";

                                      //options.RequireHttpsMetadata = false;

                                      options.TokenValidationParameters.ValidateIssuer = false;
                                  });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                                       .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                       .RequireAuthenticatedUser()
                                       .Build();
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIGateway", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIGateway v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
