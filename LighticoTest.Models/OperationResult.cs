using System;
using System.Collections.Generic;
using System.Text;

namespace LighticoTest.Models
{
    public class OperationResult
    {
        public bool IsSuccessful { get; set; }
        public string Reason { get; set; }
        public object Data { get; set; }
    }
}
