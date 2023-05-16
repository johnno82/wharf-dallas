using System;
using System.Data;
using System.Data.Common;

namespace Dallas.Core.Data.Entities
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        public required string Name { get; set; }

        public required string JobTitle { get; set; }
    }
}
