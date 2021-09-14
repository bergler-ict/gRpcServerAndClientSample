using System.Collections.Generic;
using FormulaOne;

namespace GrpcServer.Services
{
    public class DriversData
    {
        private List<Driver> _drivers;

        public DriversData()
        {
            _drivers = new List<Driver>();
        }

        public void AddDriver(Driver driver)
        {
            _drivers.Add(driver);
        }

        public List<Driver> Drivers => _drivers;
    }
}
