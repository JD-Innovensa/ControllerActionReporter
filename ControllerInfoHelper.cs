using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ControllerActionReporter
{
    public static class ControllerInfoHelper
    {
        public static List<ControllerInfo> GetControllerInfoFromAssembly(Assembly asm)
        {            
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(Microsoft.AspNetCore.Mvc.Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new ControllerInfo { ControllerName = x.DeclaringType.Name, ActionName = x.Name, ReturnTypeName = x.ReturnType.Name, AttributeList = String.Join("|", x.GetCustomAttributes().OrderBy(a => a.GetType().Name).Select(a => a.GetType().Name.Replace("Attribute", ""))) })
                    .OrderBy(x => x.ControllerName).ThenBy(x => x.ActionName).ToList();

            return controlleractionlist;
        }

        public static void ExportControllerInfo(List<ControllerInfo> controllerInfo, String exportPath)
        {
            using var file = File.CreateText(exportPath);

            file.WriteLine($"Controller,Action,ReturnType,Attributes");

            foreach (var item in controllerInfo)
            {
                file.WriteLine($"{item.ControllerName},{item.ActionName},{item.ReturnTypeName},{item.AttributeList}");
            }

            file.Flush();
        }
    }
}
