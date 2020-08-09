using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.Debugger
{
    public static class DebugHelper
    {
#if DEBUG
        public static bool IsDebug => true;
#endif

#if !DEBUG
        public static bool IsDebug => false;
#endif
    }
}
