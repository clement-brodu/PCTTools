using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace PCTTools.Util
{
    internal static class EmbeddedAssemblyHelper
    {

        private static void CopyTo(Stream source, Stream destination)
        {
            var array = new byte[81920];
            int count;
            while ((count = source.Read(array, 0, array.Length)) != 0)
            {
                destination.Write(array, 0, count);
            }
        }
        public static Stream LoadStream(Assembly assembly, string fullName)
        {

            if (fullName.EndsWith(".compressed"))
            {
                using var stream = assembly.GetManifestResourceStream(fullName);
                using var compressStream = new DeflateStream(stream, CompressionMode.Decompress);
                var memStream = new MemoryStream();
                CopyTo(compressStream, memStream);
                memStream.Position = 0;
                return memStream;
            }

            return assembly.GetManifestResourceStream(fullName);
        }
        public static byte[] ReadStream(Stream stream)
        {
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
