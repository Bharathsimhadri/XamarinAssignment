using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XFTest.Models;
using Task = System.Threading.Tasks.Task;

namespace XFTest.Services
{
    public class CarFitsServices: ICarFitServices
    {
        public async Task<ServiceResut> GetAllCarServices()
        {
            var serviceResult = new ServiceResut();
            await Task.Delay(3000);
            var data = DataBaseService.CarWashData;
            if(data!=null)
            {
                serviceResult.Data = data;
                serviceResult.Success = true;
            }
            else
            {
                serviceResult.Success = false;
                serviceResult.Message = "No data available";
            }
            return serviceResult;
        }
    }
}
