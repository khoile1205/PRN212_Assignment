using AutoMapper;
using DataAccess.Models;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Constants;
using Service.Mapper;
using Service.Services;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DI
{
    public static class DependencyInjection
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            // Register DB Context with connection string from configuration
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(AppConstants.CONNECTION_STRING_NAME))
            );

            services.AddAutoMapper(typeof(UserAutoMapper));
            // Register DAL (Repositories)
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register BLL (Services)
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IOrderService, OrderService>();

        }
    }
}
