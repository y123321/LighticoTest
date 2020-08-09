using LighticoTest.Models;
using System;
using System.Threading.Tasks;

namespace LighticoTest.Repositories.Interfaces
{
    public interface ICustomersRepository
    {
        public void AddCustomer(Customer customer);
        public void UpdateCustomer(Customer customer);
    }
}
