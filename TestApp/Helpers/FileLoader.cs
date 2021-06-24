using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace TestApp.Helpers
{
    public class FileLoader
    {
        private byte[] binaryData;
        public FileLoader(Stream stream, int lenght)
        {
            BinaryReader b = new BinaryReader(stream);
            binaryData = b.ReadBytes(lenght);
        }

        public string GetTextData(Encoding encoding = null)
        {
            if (encoding == null)
            { encoding = Encoding.UTF8; }

            return encoding.GetString(binaryData);
        }
    }
}