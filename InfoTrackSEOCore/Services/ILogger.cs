
namespace InfoTrackSEOCore.Services
{
    public interface ILogger
    {
		/// <summary>
		/// Records the provided data to the error log.
		/// </summary>
		/// <param name="logData">A string representing the data to log.</param>
        void Log(string logData);
    }
}
