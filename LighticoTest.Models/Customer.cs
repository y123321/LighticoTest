using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LighticoTest.Models
{
    public class Customer
    {
        public Guid Id { get; set; } = default(Guid);
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public IEnumerable<Address> Address { get; set; }

    }
}
