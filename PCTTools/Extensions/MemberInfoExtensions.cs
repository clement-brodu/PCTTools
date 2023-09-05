using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using PCTTools.Model;

namespace PCTTools.Extensions
{
    internal static class MemberInfoExtensions
    {
        /// <summary>
        /// Return ObsoleteDocumentation from MemberInfo
        /// </summary>
        /// <param name="member">member</param>
        /// <returns></returns>
        public static ObsoleteDocumentation GetObsolete(this MemberInfo member)
        {
            var obsolete = member.GetCustomAttribute<ObsoleteAttribute>();
            if (obsolete is null)
            {
                return null;
            }
            return new ObsoleteDocumentation()
            {
                Message = obsolete.Message ?? "Obsolete",
                IsError = obsolete.IsError
            };
        }

        /// <summary>
        /// Find if method is public or protected. ignore internal or private
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="publicOnly">ignore protected, public only</param>
        /// <returns></returns>
        public static bool IsMethodPublicOrProtected(this MethodBase method, bool publicOnly)
        {
            return method.IsPublic || (!publicOnly && method.IsFamily && !method.IsFamilyAndAssembly && !method.IsFamilyOrAssembly);
        }

        /// <summary>
        /// Find if field is public or protected. ignore internal or private
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="publicOnly">ignore protected, public only</param>
        /// <returns></returns>
        public static bool IsFieldPublicOrProtected(this FieldInfo field, bool publicOnly)
        {
            return field.IsPublic || (!publicOnly && field.IsFamily && !field.IsFamilyAndAssembly && !field.IsFamilyOrAssembly);
        }
    }
}
