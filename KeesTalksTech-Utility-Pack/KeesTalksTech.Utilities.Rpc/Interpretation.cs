using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace KeesTalksTech.Utilities.Rpc
{
    /// <summary>
    /// Helps with interpretation of JSON calls to methods.
    /// </summary>
    public static class Interpretation
    {
        /// <summary>
        /// Creates the interpreter..
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

            var methods = type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).ToList();

            foreach (var extensionType in extensionTypes)
            {
                if (extensionType.IsSealed && !extensionType.IsGenericType && !type.IsNested)
                {
                    var extMethods = extensionType
                        .GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                        .Where(m => m.IsDefined(typeof(ExtensionAttribute), false))
                        .Where(m =>
                        {
                            var t = m.GetParameters()[0].ParameterType;
                            return t.IsInterface && t.IsAssignableFrom(type);
                        });

                    methods.AddRange(extMethods);

                    continue;
                }

                throw new Exception($"Type '{extensionType.FullName}' is not a class with extension methods.");
            }

            return new Interpreter(instance, methods.ToArray());
        }
    }
}