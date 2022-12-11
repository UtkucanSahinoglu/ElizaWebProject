using ElizaWebProject.Model;
using ElizaWebProject.Services;
using ElizaWebProject.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElizaWebProject
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
            services.AddRazorPages();
            services.AddDbContext<ElizaDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ElizaConnectionString")));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ElizaDbContext>();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Login";
            });

            services.AddRouting(r => {
                r.ConstraintMap.Add("slugify", typeof(SlugifyParameterTransformer));
                r.LowercaseQueryStrings = true;
                r.LowercaseUrls = true;
            });

            services.AddRazorPages(c =>
            {
                c.Conventions.AddPageRouteModelConvention("/ViewEvent", mc =>
                {
                    var allSelectors = new List<SelectorModel>
                    {
                        mc.Selectors[0]
                    };
                    mc.Selectors.Clear();
                    mc.Selectors.Add(new SelectorModel(allSelectors[0])
                    {
                        AttributeRouteModel = { Template = AttributeRouteModel.CombineTemplates("/event", "{title:slugify}") }
                    });
                    mc.Selectors.Add(new SelectorModel(allSelectors[0])
                    {
                        AttributeRouteModel = { Template = AttributeRouteModel.CombineTemplates("/event/view", "{id}") }
                    });
                });
            });

            services.AddControllers();

            services.AddSingleton<IEventService, EventService>();
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

            app.UseHttpsRedirection();


            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
