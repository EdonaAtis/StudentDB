﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.DataModels.CustomModels
{
          public class ResponseModel
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public string AdminName { get; set; }
            public string AdminEmail { get; set; }
            public string AdminRoles { get; set; }
        }

    
}
