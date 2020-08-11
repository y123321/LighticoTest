using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LighticoTest.Models
{
    public class QueueWithLocks<T>
    {
        public Queue<T> Queue { get; set; } = new Queue<T>();
        public ReaderWriterLockSlim FirstLock { get; set; } = new ReaderWriterLockSlim();
        public ReaderWriterLockSlim SecondLock { get; set; } = new ReaderWriterLockSlim();
        public bool IsWorking { get; set; }
    }
}
