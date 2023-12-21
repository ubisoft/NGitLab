using System.IO;

namespace NGitLab.Models
{
    public sealed class FileContent
    {
        public FileContent(Stream stream)
        {
            Stream = stream;
        }

        /// <summary>
        /// The stream to be uploaded.
        /// </summary>
        public Stream Stream { get; }
    }
}
