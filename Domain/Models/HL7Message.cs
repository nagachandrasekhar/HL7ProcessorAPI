using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class HL7Message
    {
        public Guid Id { get; set; }
        //public string PatientName { get; set; }
        //public string RecordNumber { get; set; }
        public string JsonData { get; set; }
       
    }
}
