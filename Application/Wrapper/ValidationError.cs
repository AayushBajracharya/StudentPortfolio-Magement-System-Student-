﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrapper
{
    public class ValidationError
    {
        public string Name { get; }
        public string Reason { get; }
        public ValidationError(string name, string reason)
        {
            Name = name != string.Empty ? name : null;
            Reason = reason;
        }
    }
}
