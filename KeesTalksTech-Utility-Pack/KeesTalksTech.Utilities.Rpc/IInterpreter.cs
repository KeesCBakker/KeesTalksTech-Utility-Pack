using Newtonsoft.Json.Linq;
using System;

namespace KeesTalksTech.Utilities.Rpc
{
    /// <summary>
    /// Indicate the obect 
    /// </summary>
    public interface IInterpreter
    {
        /// <summary>
        /// Registers the converter.
        /// </summary>
        /// <param name="converter">The converter.</param>
        /// <returns>The interpreter (for chaining).</returns>
        IInterpreter RegisterConverter(TryConverter converter);

        /// <summary>
        /// Executes the methods defined by the JSON string.
        /// This can be an object that contains a 'method-name' property to identify the method.
        /// This can be an array that contains object that contain a 'method-name' property to identify the method.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>
        /// The result of the method. When multiple methods are executed, an array of results is returned.
        /// </returns>
        object Execute(string json);
    }
}