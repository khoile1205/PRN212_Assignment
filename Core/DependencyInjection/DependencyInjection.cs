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

            // Register DB Context
            string CONNECTION_STRING = configuration.GetConnectionString(AppConstants.CONNECTION_STRING_NAME);
            services.AddDbContext<Prn211AssignmentContext>(option => option.UseSqlServer(CONNECTION_STRING));
            services.AddAutoMapper(typeof(UserAutoMapper));
            // Register DAL (Repositories)
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register BLL (Services)
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
