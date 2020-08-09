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
         ConcurrentDictionary<Guid,ReaderWriterLock> _locks = new ConcurrentDictionary<Guid, ReaderWriterLock>();
        readonly string FILES_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LighticoApp");
        public CustomersRepository()
        {
            Directory.CreateDirectory(FILES_PATH);
        }
        public void AddCustomer(Customer customer)
        {
            var locker = _locks.GetOrAdd(customer.Id, new ReaderWriterLock());
            locker.AcquireWriterLock(TimeSpan.FromSeconds(60));
            try
            {
                string filePath = Path.Combine(FILES_PATH, customer.Id.ToString() + ".txt");
                string contents = JsonSerializer.Serialize(customer);
                File.WriteAllText(filePath, contents);
                _locks.TryRemove(customer.Id, out ReaderWriterLock d);
            }
            finally
            {
                locker.ReleaseWriterLock();
            }
        }

        public void UpdateCustomer(Customer customer)=> AddCustomer(customer);
    }
}
