using System;
using System.Reflection;
using System.Runtime.Loader;

namespace ControllerActionReporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Controller/Action exporter (Net Core 3.1).");

            Console.WriteLine("Enter the path to the assembly (DLL) to inspect:");

            var assemblyPath = Console.ReadLine();

            Console.WriteLine("Enter directory for CSV export:");

            var exportDirectory = Console.ReadLine();

            Assembly asm = Assembly.LoadFrom(assemblyPath);

            var info = ControllerInfoHelper.GetControllerInfoFromAssembly(asm);

            var filename = $"{exportDirectory}\\{asm.GetName().Name}_controllerinfo.csv";

            ControllerInfoHelper.ExportControllerInfo(info, filename);

            Console.WriteLine("Finished.");
        }
    }
}
