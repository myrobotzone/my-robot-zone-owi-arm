﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robot_arm_server
{
    internal interface ISystemUtils
    {
        bool FileExists(string file);

        string GetTempPath();
    }
}
