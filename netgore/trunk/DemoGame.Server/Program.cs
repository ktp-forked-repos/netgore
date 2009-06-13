using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;

namespace DemoGame.Server
{
    class Program
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            if (log.IsInfoEnabled)
                log.Info("Starting server...");

            using (new Server())
            {
            }
        }
    }
}