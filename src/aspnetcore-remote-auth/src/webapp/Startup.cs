using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AspNetCore.LoremIpsum
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddEbay(options =>
                {
                    IConfigurationSection ebayAuthNSection = this.Configuration.GetSection("Authentication:Ebay");

                    options.ClientId = ebayAuthNSection["ClientId"];
                    options.ClientSecret = ebayAuthNSection["ClientSecret"];
                    options.RuName = ebayAuthNSection["RuName"];
                })
                .AddGitHub(options =>
                {
                    IConfigurationSection githubAuthNSection = this.Configuration.GetSection("Authentication:GitHub");

                    options.ClientId = githubAuthNSection["ClientId"];
                    options.ClientSecret = githubAuthNSection["ClientSecret"];
                })
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection = this.Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/SignIn";
                    options.LogoutPath = "/SignOut";
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseCookiePolicy()
                .UseStaticFiles()
                .UseAuthentication()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapRazorPages();
                });
        }
    }
}
