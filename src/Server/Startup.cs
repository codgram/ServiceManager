using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using ServiceManager.Server.Data;
using ServiceManager.Server.Models;
using ServiceManager.Server.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Net.Http.Headers;

namespace ServiceManager.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ServiceManagerContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ServiceManagerUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ServiceManagerContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ServiceManagerUser, ServiceManagerContext>();

            services.AddAuthentication()
            .AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection =
                    Configuration.GetSection("Authentication:Google");
    
                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
            });


            // This is used to get user claims in the API
            services.Configure<IdentityOptions>(options => 
                    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddControllersWithViews();
            services.AddRazorPages();
            

            /*******************   CORS [Access-Control-Allow-Origin]  ********************/
            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:5001", "http://localhost:5000").AllowAnyHeader().AllowAnyMethod();
                    });

                // options.AddDefaultPolicy(
                //     builder =>
                //     {
                //         builder.WithOrigins("https://localhost:5001", "http://localhost:5000").AllowAnyHeader().WithMethods("GET");
                //     });
            });

            

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseCors();

            // app.UseCors(policy => 
            //     policy.WithOrigins("http://localhost:5000", "https://localhost:5001")
            //     .AllowAnyMethod()
            //     .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization)
            //     .AllowCredentials());

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
