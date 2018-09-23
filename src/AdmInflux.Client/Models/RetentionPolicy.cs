namespace AdmInflux.Client.Models
{
    public class RetentionPolicy
    {
        public string Name { get; set; }
        public string Duration { get; set; }
        public string ShardGroupDuration { get; set; }
        public int ReplicaN { get; set; }
        public bool Default { get; set; }
    }
}