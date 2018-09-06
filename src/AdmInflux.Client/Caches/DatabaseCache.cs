using System.Collections.Generic;
using AdmInflux.Client.Models;

namespace AdmInflux.Client.Caches
{
    internal static class DatabaseCache
    {
        private static readonly IDictionary<string, List<Database>> Cache = new Dictionary<string, List<Database>>();

        public static bool TryGet(string server, out List<Database> databases) => Cache.TryGetValue(server, out databases);

        public static void Update(string server, List<Database> databases) => Cache[server] = databases;
    }

    internal static class MeasurementCache
    {
        private static readonly IDictionary<(string,string), List<Measurement>> Cache = new Dictionary<(string, string), List<Measurement>>();

        public static bool TryGet(string server, string database, out List<Measurement> measurements) =>
            Cache.TryGetValue((server, database), out measurements);
        
        public static void Update(string server, string database, List<Measurement> measurements) => Cache[(server, database)] = measurements;
    }

    internal static class SeriesCache
    {
        private static readonly IDictionary<(string,string,string), List<Series>> Cache = new Dictionary<(string, string, string), List<Series>>();

        public static bool TryGet(string server, string database, string measurement, out List<Series> series) =>
            Cache.TryGetValue((server, database, measurement), out series);

        public static void Update(string server, string database, string measurement, List<Series> series) =>
            Cache[(server, database, measurement)] = series;
    }
}