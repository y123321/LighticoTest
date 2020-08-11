using LighticoTest.Models;
using LighticoTest.Repositories.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LighticoTest.Services
{
    public class CustomersOperationsManager : BackgroundService
    {
        ICustomersRepository _customersRepository;
        private readonly ConcurrentDictionary<Guid, Queue<CustomerOperation>> _operations;
        private readonly ConcurrentDictionary<Guid, object> _currenltlyRunning = new ConcurrentDictionary<Guid, object>();
        Thread t;
        Random _random = new Random();
        public CustomersOperationsManager(ConcurrentDictionary<Guid, Queue<CustomerOperation>> operations, ICustomersRepository customersRepository)
        {
            _operations = operations;
            _customersRepository = customersRepository;
        }
        void Run()
        {
            Parallel.For(0, Environment.ProcessorCount - 1, i =>
            {
                while (true)
                {
                    try
                    {
                        if (!_operations.Any())
                            continue;
                        var index = _random.Next(0, _operations.Count - 1);
                        var opData = _operations.ToArray()[index];
                        if (_currenltlyRunning.ContainsKey(opData.Key))
                            continue;
                        var locker = _currenltlyRunning.GetOrAdd(opData.Key, new { });
                        lock (locker)
                        {
                            if (opData.Value.Any() && opData.Value.Peek()?.IsWorking == true)
                                continue;
                            lock (opData.Value)
                                if (opData.Value.Any())
                                {
                                    var op = opData.Value.Dequeue();
                                    RunSingleOperation(op);
                                }
                                else _operations.Remove(opData.Key, out Queue<CustomerOperation> dd);
                            _currenltlyRunning.Remove(opData.Key, out object d);
                        }
                    }
                    catch (Exception e)
                    {
                        LogException(e);
                    }
                }
            });
            t.Start();


        }

        private void LogException(Exception e)
        {
        }

        void RunSingleOperation(CustomerOperation op)
        {
            switch (op.Type)
            {
                case OperationType.Add:
                    _customersRepository.AddCustomer(op.Customer);
                    break;
                case OperationType.Update:
                    _customersRepository.UpdateCustomer(op.Customer);
                    break;
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            t = new Thread(Run);
            t.Start();
        }
    }
}
