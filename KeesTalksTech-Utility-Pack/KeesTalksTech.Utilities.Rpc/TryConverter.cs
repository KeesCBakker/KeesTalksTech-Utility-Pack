using System;
using System.Reflection;

namespace KeesTalksTech.Utilities.Rpc
{
    /// <summary>
    /// Tries to convert the value.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    /// <param name="value">The value.</param>
    /// <param name="conversionResult">The conversion result.</param>
    /// <returns>
    ///   <c>true</c> if the conversion was succesful; otherwise <c>false</c>.
    /// </returns>
    public delegate bool TryConverter(ParameterInfo parameter, string value, out object conversionResult);
}
