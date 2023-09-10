using System;
using System.Collections.Generic;
using System.Text;

namespace PCTTools.Util
{
    internal class OeTypeUtil
    {
        private static Dictionary<Type, string> OeTypes = new Dictionary<Type, string>();
        static OeTypeUtil()
        {
            // https://docs.progress.com/fr-FR/bundle/openedge-gui-for-dotnet-reference-117/page/Explicit-data-type-mapping-from-ABL-primitive-to-.NET-types.html
            OeTypes.Add(typeof(bool), "LOGICAL");
            OeTypes.Add(typeof(Byte), "INTEGER");
            OeTypes.Add(typeof(SByte), "INTEGER");
            OeTypes.Add(typeof(DateTime), "DATETIME");
            OeTypes.Add(typeof(decimal), "DECIMAL");
            OeTypes.Add(typeof(Int16), "INTEGER");
            OeTypes.Add(typeof(UInt16), "INTEGER");
            OeTypes.Add(typeof(Int32), "INTEGER");
            OeTypes.Add(typeof(UInt32), "INTEGER");
            OeTypes.Add(typeof(Int64), "INT64");
            OeTypes.Add(typeof(UInt64), "INT64");
            OeTypes.Add(typeof(double), "DECIMAL");
            OeTypes.Add(typeof(Single), "DECIMAL");
            OeTypes.Add(typeof(string), "CHARACTER");
            OeTypes.Add(typeof(char), "CHARACTER");
            OeTypes.Add(typeof(void), "VOID");
        }

        /// <summary>
        /// Format Type to OeFormatedType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToOeFormatedType(Type type)
        {
            if (OeTypes.ContainsKey(type))
            {
                return OeTypes[type];
            }

            return null; // null if not Openedge Type
        }
    }
}
