using System.Collections.Generic;
using FormulaOne;

namespace GrpcServer.Services
{
    public static class DriversData
    {
        public static readonly List<Driver> Drivers = new List<Driver>();
        
        public static void AddDriver(Driver driver)
        {
            Drivers.Add(driver);
        }
    }
}
