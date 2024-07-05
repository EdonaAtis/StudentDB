using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.DataModels.CustomModels
{


    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }

    public class ServiceResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}