using System.Collections.Generic;

namespace InfoTrackSEOCore.Models
{
    public class InfoTrackSEOModel
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public List<int> IndexPositions { get; set; }

        public InfoTrackSEOModel()
        {
            // Init collection
            IndexPositions = new List<int>();
        }
    }
}
