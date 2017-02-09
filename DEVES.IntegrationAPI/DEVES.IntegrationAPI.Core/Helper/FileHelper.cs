using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Core.Helper
{
    public static class FileHelper
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(FileHelper));

        public static string ReadTextFile(string fileName)
        {
            var text = string.Empty;
            if (!string.IsNullOrWhiteSpace(fileName) && File.Exists(fileName))
            //if (File.Exists(fileName))
            {
                _log.InfoFormat("ReadTextFile: {0} is exists.", fileName);
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(fileName))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadToEnd();
                    text = line;
                }
            }
            return text;
        }
    }
}
