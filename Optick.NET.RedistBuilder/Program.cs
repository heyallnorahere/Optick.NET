using System;
using System.Linq;
using System.Reflection;

namespace Optick.NET.RedistBuilder
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal sealed class RegisteredCommandAttribute : Attribute
    {
        public RegisteredCommandAttribute(string id)
        {
            ID = id;
        }

        public string ID { get; }
    }

    internal interface ICommand
    {
        public void Invoke(string[] args);
    }

    internal static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("No command ID was passed!");
            }

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            string commandId = args[0];
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<RegisteredCommandAttribute>();
                if (attribute?.ID != commandId)
                {
                    continue;
                }

                var interfaces = type.GetInterfaces();
                if (!interfaces.Contains(typeof(ICommand)))
                {
                    continue;
                }

                var constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, Array.Empty<Type>());
                if (constructor is null)
                {
                    continue;
                }

                var instance = (ICommand)constructor.Invoke(null);
                instance.Invoke(args[1..]);

                return;
            }

            throw new ArgumentException($"Invalid command ID: {commandId}");
        }
    }
}