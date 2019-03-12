using InfoTrackSEOCore.Configuration;
using System;
using System.IO;

namespace InfoTrackSEOCore.Services
{
    public class TextLogger : ILogger
    {
        private InfoTrackSeoCoreConfiguration _config;

        public TextLogger(InfoTrackSeoCoreConfiguration config)
        {
            _config = config;
        }

		/// <summary>
		/// Records the provided data to the error log.
		/// </summary>
		/// <param name="logData">A string representing the data to log.</param>
		public void Log(string logData)
        {
            var path = Directory.GetCurrentDirectory() + _config.LogPath;

            var fs = File.Exists(path) == false ? File.Create(path) : File.Open(path, FileMode.Append);

            using (var sw = new StreamWriter(fs))
            {
                var logLine = $"{DateTime.Now:G}: {logData}.";
                sw.WriteLine(logLine);
            }
        }
    }
}
