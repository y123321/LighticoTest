using LighticoTest.Interfaces.Services;
using LighticoTest.Models;
using LighticoTest.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LighticoTest.Services
{
    public class CustomersService : ICustomerService
    {
        ConcurrentDictionary<Guid, QueueWithLocks<CustomerOperation>> _operations;

        public CustomersService(ConcurrentDictionary<Guid, QueueWithLocks<CustomerOperation>> operations)
        {
            _operations = operations;
        }
        public void AddCustomer(Customer customer)
        {
                customer.Id = Guid.NewGuid();

            while (_operations.ContainsKey(customer.Id))
                customer.Id = Guid.NewGuid();
            _operations[customer.Id] = new QueueWithLocks<CustomerOperation>();
            var addOperation = new CustomerOperation
            {
                Customer = customer,
                Type = OperationType.Add
            };
            AddOperation(customer, addOperation);
        }
        public void UpdateCustomer(Customer customer)
        {
            var updateOperation = new CustomerOperation
            {
                Customer = customer,
                Type = OperationType.Update
            };
            AddOperation(customer, updateOperation);
        }

        private void AddOperation(Customer customer, CustomerOperation updateOperation)
        {
            _operations.AddOrUpdate(customer.Id, id =>
            {
                var queue = new QueueWithLocks<CustomerOperation>();
                queue.Queue.Enqueue(updateOperation);
                return queue;
            }, (id, queue) =>
            {
                queue.Queue.Enqueue(updateOperation);
                return queue;
            });
        }
    }
}
