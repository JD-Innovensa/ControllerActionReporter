using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ControllerActionReporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Controller/Action exporter (Net Core 3.1).");

            Console.WriteLine("Enter the path to the assembly (DLL) to inspect:");

            var assemblyPath = Console.ReadLine();

            Assembly asm = Assembly.LoadFrom(assemblyPath);
            
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(Microsoft.AspNetCore.Mvc.Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            Console.WriteLine($"Controller/Actions in {asm.GetName()}.");

            foreach (var item in controlleractionlist)
            {
                Console.WriteLine($"{item.Controller},{item.Action}");
            }

            Console.WriteLine("Finished.");
        }
    }
}
