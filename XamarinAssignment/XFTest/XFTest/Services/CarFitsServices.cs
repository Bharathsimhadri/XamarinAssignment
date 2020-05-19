using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XFTest.Models;
using XFTest.Views;
using Task = System.Threading.Tasks.Task;

namespace XFTest.Services
{
    public class CarFitsServices: ICarFitServices
    {
        public async Task<ServiceResut> GetAllCarServices()
        {

            string jsonFileName = "XF_Test.Json";
            var serviceResult = new ServiceResut();
            await Task.Delay(3000);
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
            using (var reader = new System.IO.StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                if (jsonString != null)
                {
                  return  serviceResult = JsonConvert.DeserializeObject<ServiceResut>(jsonString);
                }
                else
                {
                    serviceResult.Success = false;
                    serviceResult.Message = "No data available";
                }
            }
            return serviceResult;
        }
    }
}
