﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    class EmptyStringException: Exception
    {
        public EmptyStringException()
            :base("Empty string given")
        { }
    }
}
