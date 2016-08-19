using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace KeesTalksTech.Utilities.Rpc
{
    /// <summary>
    /// Helps with interpretation of JSON calls to methods.
    /// </summary>
    public static class Interpretation
    {
        /// <summary>
        /// Creates the interpreter.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="errorHandling">The error handling for interpretation.</param>
        /// <param name="extensionTypes">The extension types.</param>
        /// <returns>An interpreter.</returns>
        public static IInterpreter Create<TInterface>(TInterface instance, params Type[] extensionTypes)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var type = typeof(TInterface);

            if (!type.IsInterface)
            {
                throw new Exception("Interface type must be an interface for security reasons.");
            }

            var list = new List<MethodInfo>();

            AddInterfaceMethods(type, list);
            AddExtensionMethods(extensionTypes, type, list);

            return new Interpreter(instance, list.ToArray());
        }

        /// <summary>
        /// Adds the interface methods.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="list">The list.</param>
        private static void AddInterfaceMethods(Type interfaceType, List<MethodInfo> list)
        {
            var methods = interfaceType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            list.AddRange(methods);

            foreach (var sub in interfaceType.GetInterfaces())
            {
                AddInterfaceMethods(sub, list);
            }
        }

        /// <summary>
        /// Adds the extension methods.
        /// </summary>
        /// <param name="extensionTypes">The extension types.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="list">The list.</param>
        private static void AddExtensionMethods(Type[] extensionTypes, Type interfaceType, List<MethodInfo> list)
        {
            foreach (var extensionType in extensionTypes)
            {
                if (extensionType.IsSealed && !extensionType.IsGenericType && !interfaceType.IsNested)
                {
                    var extMethods = extensionType
                        .GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                        .Where(m => m.IsDefined(typeof(ExtensionAttribute), false))
                        .Where(m =>
                        {
                            var t = m.GetParameters()[0].ParameterType;
                            return t.IsInterface && t.IsAssignableFrom(interfaceType);
                        });

                    list.AddRange(extMethods);

                    continue;
                }

                throw new Exception($"Type '{extensionType.FullName}' is not a class with extension methods.");
            }
        }
    }
}