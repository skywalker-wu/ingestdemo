using System;
using System.Collections.Generic;
using System.Text;

namespace DemoFunction
{
    public static class ConfigurationUtils
    {
        public static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
