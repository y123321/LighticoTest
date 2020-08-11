using System;
using System.Collections.Generic;
using System.Text;

namespace LighticoTest.Models
{
    public class CustomerOperation
    {
        public Customer Customer { get; set; }
        public OperationType Type { get; set; }
        public bool IsWorking { get; set; }
    }
}
