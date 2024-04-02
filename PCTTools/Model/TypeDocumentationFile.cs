using System.Collections.Generic;

namespace PCTTools.Model
{
    public class TypeDocumentationFile
    {
        /// <summary>
        /// File Model version - must be increment in case of changes in the model
        /// </summary>
        public int SchemaVersion { get; set; } = 1;

        public List<TypeDocumentation> Classes { get; set; }
    }
}
