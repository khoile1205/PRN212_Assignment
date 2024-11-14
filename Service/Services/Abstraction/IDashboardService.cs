using DataAccess.Models;
using Service.DTO.Admin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Abstraction
{
    public interface IDashboardService
    {
        public Task<StatisticDashboardResponseDto> GetStatisticDashboardResponse();
    }
}
