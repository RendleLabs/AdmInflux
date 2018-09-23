namespace AdmInflux.Models
{
    public class NewRetentionPolicyViewModel
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Name { get; set; }
        public string Duration { get; set; }
        public string ShardDuration { get; set; }
        public bool MakeDefault { get; set; }
    }
}