using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XFTest.Models;

namespace XFTest.Services
{
    public interface ICarFitServices
    {
        Task<ServiceResut> GetAllCarServices();
    }
}
