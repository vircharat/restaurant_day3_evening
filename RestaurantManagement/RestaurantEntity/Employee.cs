using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestaurantEntity
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpId { get; set; }

        public string EmpName { get; set; }

        public string EmpEmail { get; set; }

        public string EmpPassword { get; set; }

        public string EmpDesignation { get; set; }

        public string EmpSpeciality { get; set; }

        public double EmpPhone { get; set; }
    }
}
