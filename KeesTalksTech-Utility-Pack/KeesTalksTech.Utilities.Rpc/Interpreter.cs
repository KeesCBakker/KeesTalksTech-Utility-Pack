using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace KeesTalksTech.Utilities.Rpc
{
    /// <summary>
    /// Implements an RPC interpreter.
    /// </summary>
    /// <seealso cref="KeesTalksTech.Utilities.Rpc.IInterpreter" />
    internal class Interpreter : IInterpreter
    {
        private List<TryConverter> _converters = new List<TryConverter>();
        private MethodInfo[] _methods;
        private object _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="Interpreter" /> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="methods">The methods.</param>
        public Interpreter(object instance, MethodInfo[] methods)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            _instance = instance;
            _methods = methods;
        }

        /// <summary>
        /// Executes the methods defined by the JSON string.
        /// This can be an object that contains a 'method-name' property to identify the method.
        /// This can be an array that contains object that contain a 'method-name' property to identify the method.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>
        /// The result of the method. When multiple methods are executed, an array of results is returned.
        /// </returns>
        public object Execute(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            var obj = JToken.Parse(json);

            if (obj is JArray)
            {
                Execute(obj as JArray);
                return null;
            }

            if (obj is JObject)
            {
                return Execute(obj as JObject);
            }

            throw new ArgumentException("Bad format.", nameof(json));
        }

        /// <summary>
        /// Executes the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>A list with results.</returns>
        public object[] Execute(JArray array)
        {
            var result = new ArrayList();

            foreach (var obj in array)
            {
                if (obj is JObject)
                {
                    var r = Execute(obj as JObject);
                    result.Add(r);
                    continue;
                }
                
                throw new ArgumentException("Bad format.", nameof(array));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Executes the method specified by the JSON object. Must contain a 'method-name' property to identity the method.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// The result of the method.
        /// </returns>
        public object Execute(JObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var name = obj["method-name"]?.ToString();
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("No name specified.", nameof(obj));
            }

            var methods = _methods.Where(m => m.Name == name).OrderByDescending(m => m.GetParameters().Length).ToArray();

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                var count = parameters.Length;

                var values = new ArrayList();
                if (method.IsStatic)
                {
                    values.Add(_instance);
                    parameters = parameters.Skip(1).ToArray();
                }

                //try to fill each parameter
                foreach (var parameter in parameters)
                {
                    var value = obj[parameter.Name];

                    //check if value is not present
                    if (value == null)
                    {
                        //default value?
                        if (parameter.HasDefaultValue)
                        {
                            values.Add(parameter.RawDefaultValue);
                            continue;
                        }

                        //nothing here? Break the loop.
                        break;
                    }

                    try
                    {
                        bool found = false;
                        string stringValue = value.ToString();

                        //try value conversion by converters
                        foreach (var converter in _converters)
                        {
                            object convertedValue = null;

                            if (converter.Invoke(parameter, stringValue, out convertedValue))
                            {
                                values.Add(convertedValue);
                                found = true;
                                break;
                            }
                        }

                        if (found)
                        {
                            continue;
                        }

                        //try generic change type
                        object newValue = Convert.ChangeType(value, parameter.ParameterType);
                        values.Add(newValue);
                    }
                    catch
                    {
                        break;
                    }
                }

                if (count == values.Count)
                {
                    try
                    {
                        return method.Invoke(_instance, values.ToArray());
                    }
                    catch (Exception ex) { }
                }
            }

            throw new Exception($"Method '{name}' not found or could not be executed.");
        }

        /// <summary>
        /// Registers the converter.
        /// </summary>
        /// <param name="converter">The converter.</param>
        /// <returns>
        /// The interpreter (for chaining).
        /// </returns>
        public IInterpreter RegisterConverter(TryConverter converter)
        {
            _converters.Add(converter);

            return this;
        }
    }
}