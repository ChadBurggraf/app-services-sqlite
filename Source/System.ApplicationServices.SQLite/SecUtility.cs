// ----------------------------------------------------------------------------------------------
// <copyright file="SecUtility.cs" company="N/A">
//      Copyright (c) 2008 Roger Martin.
//      http://www.codeproject.com/Articles/29199/SQLite-Membership-Role-and-Profile-Providers
// </copyright>
// ----------------------------------------------------------------------------------------------

namespace System.ApplicationServices.SQLite
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Provides general purpose validation functionality.
    /// </summary>
    internal class SecUtility
    {
        /// <summary>
        /// Checks the parameter and throws an exception if one or more rules are violated.
        /// </summary>
        /// <param name="param">The parameter to check.</param>
        /// <param name="checkForNull">When <c>true</c>, verify <paramref name="param"/> is not null.</param>
        /// <param name="checkIfEmpty">When <c>true</c> verify <paramref name="param"/> is not an empty string.</param>
        /// <param name="checkForCommas">When <c>true</c> verify <paramref name="param"/> does not contain a comma.</param>
        /// <param name="maxSize">The maximum allowed length of <paramref name="param"/>.</param>
        /// <param name="paramName">Name of the parameter to check. This is passed to the exception if one is thrown.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="param"/> is null and <paramref name="checkForNull"/> is true.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="param"/> does not satisfy one of the remaining requirements.</exception>
        /// <remarks>This method performs the same implementation as Microsoft's version at System.Web.Util.SecUtility.</remarks>
        internal static void CheckParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
        {
            if (param == null)
            {
                if (checkForNull)
                {
                    throw new ArgumentNullException(paramName);
                }
            }
            else
            {
                param = param.Trim();

                if (checkIfEmpty && (param.Length < 1))
                {
                    throw new ArgumentException(string.Format("The parameter '{0}' must not be empty.", paramName), paramName);
                }

                if ((maxSize > 0) && (param.Length > maxSize))
                {
                    throw new ArgumentException(string.Format("The parameter '{0}' is too long: it must not exceed {1} chars in length.", paramName, maxSize.ToString(CultureInfo.InvariantCulture)), paramName);
                }

                if (checkForCommas && param.Contains(","))
                {
                    throw new ArgumentException(string.Format("The parameter '{0}' must not contain commas.", paramName), paramName);
                }
            }
        }

        /// <summary>
        /// Verifies that <paramref name="param"/> conforms to all requirements.
        /// </summary>
        /// <param name="param">The parameter to check.</param>
        /// <param name="checkForNull">When <c>true</c>, verify <paramref name="param"/> is not null.</param>
        /// <param name="checkIfEmpty">When <c>true</c> verify <paramref name="param"/> is not an empty string.</param>
        /// <param name="checkForCommas">When <c>true</c> verify <paramref name="param"/> does not contain a comma.</param>
        /// <param name="maxSize">The maximum allowed length of <paramref name="param"/>.</param>
        /// <returns>Returns <c>true</c> if all requirements are met; otherwise returns <c>false</c>.</returns>
        internal static bool ValidateParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize)
        {
            if (param == null)
            {
                return !checkForNull;
            }

            param = param.Trim();

            return (!checkIfEmpty 
                || param.Length >= 1) 
                && (maxSize <= 0 
                || param.Length <= maxSize) 
                && (!checkForCommas 
                || !param.Contains(","));
        }
    }
}
