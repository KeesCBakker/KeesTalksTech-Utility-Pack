using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeesTalksTech.Utilities.IO
{
    /// <summary>
    /// Helper methods for streams.
    /// </summary>
    public class StreamUtility
    {
        /// <summary>
        /// Checks if both stream are equal.
        /// </summary>
        /// <param name="stream1">The stream1.</param>
        /// <param name="stream2">The stream2.</param>
        /// <returns><c>True</c> if the streams are equal; otherwise <c>false</c>.</returns>
        public static bool Equals(Stream stream1, Stream stream2)
        {
            if (stream1 == null)
            {
                return stream2 == null;
            }
            if (stream2 == null)
            {
                return false;
            }
            if (stream1.Length != stream2.Length)
            {
                return false;
            }

            long p1 = stream1.Position, p2 = stream2.Position;
            stream1.Position = stream2.Position = 0;
            int i1 = 0, i2 = 0;

            try
            {
                while ((i1 = stream1.ReadByte()) > -1)
                {
                    i2 = stream2.ReadByte();

                    if (i1 != i2)
                    {
                        return false;
                    }
                }
            }
            finally
            {
                stream1.Position = p1;
                stream2.Position = p2;
            }

            return true;
        }
    }
}
