using PCTTools.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PCTTools.Extensions
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Returns the type name. If this is a generic type, appends
        /// the list of generic type arguments between angle brackets.
        /// (Does not account for embedded / inner generic arguments.)
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        public static string GetFormattedName(this Type type, bool useOeTypes = false)
        {
            if (type.IsGenericType)
            {
                string genericArguments = type.GetGenericArguments()
                                    .Select(x => x.GetFormattedName())
                                    .Aggregate((x1, x2) => $"{x1}, {x2}");
                return $"{type.Name.Substring(0, type.Name.IndexOf("`"))}"
                     + $"<{genericArguments}>";
            }

            if (useOeTypes)
            {
                return OeTypeUtil.ToOeFormatedType(type) ?? type.Name;
            }

            return type.Name;
        }

        /// <summary>
        /// Returns the type name. If this is a generic type, appends
        /// the list of generic type arguments between angle brackets.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="useOeTypes">use openedge type instead of native dotnet types</param>
        /// <returns>System.String.</returns>
        public static string GetFormattedFullName(this Type type, bool useOeTypes = false)
        {
            var name = type.FullName ?? (type.IsGenericParameter ? type.Name : $"{type.Namespace}.{type.Name}");
            if (type.IsGenericType && name != null)
            {
                string genericArguments = type.GetGenericArguments()
                                    .Select(x => x.GetFormattedFullName())
                                    .Aggregate((x1, x2) => $"{x1}, {x2}");

                return $"{name.Substring(0, name.IndexOf("`"))}"
                     + $"<{genericArguments}>";
            }

            if (useOeTypes)
            {
                return OeTypeUtil.ToOeFormatedType(type) ?? name;
            }

            return name;
        }
    }
}
