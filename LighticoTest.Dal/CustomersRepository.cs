using LighticoTest.Models;
using LighticoTest.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace LighticoTest.Dal
{
    public class CustomersRepository : ICustomersRepository
    {
         ConcurrentDictionary<Guid, ReaderWriterLockSlim> _locks = new ConcurrentDictionary<Guid, ReaderWriterLockSlim>();
        readonly string FILES_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LighticoApp");
        public CustomersRepository()
        {
            Directory.CreateDirectory(FILES_PATH);
        }
        public void AddCustomer(Customer customer)
        {
            var locker = _locks.GetOrAdd(customer.Id, new ReaderWriterLockSlim());
            locker.EnterWriteLock();
            try
            {
                string filePath = Path.Combine(FILES_PATH, customer.Id.ToString() + ".txt");
                string contents = JsonSerializer.Serialize(customer);
                File.WriteAllText(filePath, contents);
                _locks.TryRemove(customer.Id, out ReaderWriterLockSlim d);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public void UpdateCustomer(Customer customer)=> AddCustomer(customer);
    }
}
