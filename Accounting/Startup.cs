using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Accounting.Data;
using Accounting.Infrastructure.Core;
using Accounting.Infrastructure.Data;
using Accounting.Models;
using Accounting.Repositories;
using Accounting.Services;
using Accounting.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Accounting
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true);
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var conn = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AccountingContext>(options => options.UseSqlServer(conn));
            services.AddIdentity<User, IdentityRole>()
               .AddEntityFrameworkStores<AccountingContext>()
               .AddDefaultTokenProviders();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var issuer = appSettings.ValidIssuer;

            #region JWT Token Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
                options.RequireHttpsMetadata = false;
            });

            #endregion

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<ICustomerTypeRepository, CustomerTypeRepository>();
            services.AddScoped<IReceiptTypeRepository, ReceiptTypeRepository>();

            services.AddCors();
            services.AddMvc();
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(new DateTime(2016, 7, 1));
            });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Bank, BankViewModel>();
                cfg.CreateMap<CustomerType, CustomerTypeViewModel>();
                cfg.CreateMap<ReceiptType, ReceiptTypeViewModel>();
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSingleton(provider => Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AccountingContext context, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            app.UseAuthentication();
            app.UseMvc();

            CreateUserRoles(serviceProvider, context).Wait();
            DbInitializer.Initialize(context);
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider, AccountingContext context)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            // local variables
            DateTime createdDate = new DateTime(2016, 03, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;
            string roleAdministrators = "Administrators";
            string roleRegistered = "Registered";

            //Create Roles (if they doesn't exist yet)
            if (!await roleManager.RoleExistsAsync(roleAdministrators))
            {
                await roleManager.CreateAsync(new IdentityRole(roleAdministrators));
            }

            if (!await roleManager.RoleExistsAsync(roleRegistered))
            {
                await roleManager.CreateAsync(new IdentityRole(roleRegistered));
            }

            // Create the "Admin" ApplicationUser account (if it doesn't exist already)
            var userAdmin = new User
            {
                UserName = "admin",
                Email = "admin@gmail.com"
            };

            // Insert "Admin" into the Database and also assign the "Administrator"
            // role to him.
            if (await userManager.FindByNameAsync(userAdmin.UserName) == null)
            {
                await userManager.CreateAsync(userAdmin, "12345678x@X");
                await userManager.AddToRoleAsync(userAdmin, roleAdministrators);
                userAdmin.EmailConfirmed = true;
                userAdmin.LockoutEnabled = true;
            }

#if DEBUG

            var userMinh = new User
            {
                UserName = "minhlam",
                Email = "kelvincoder@gmail.com"
            };

            if (await userManager.FindByNameAsync(userMinh.UserName) == null)
            {
                await userManager.CreateAsync(userMinh, "12345678x@X");
                await userManager.AddToRoleAsync(userMinh, roleRegistered);
                userMinh.EmailConfirmed = true;
                userMinh.LockoutEnabled = true;
            }

#endif
            await context.SaveChangesAsync();
        }
    }
}
