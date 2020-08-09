using LighticoTest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LighticoTest.Interfaces.Services
{
    public interface ICustomerService
    {
        void UpdateCustomer(Customer customer);
        void AddCustomer(Customer customer);

    }
}
