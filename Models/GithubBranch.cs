namespace IntegrationsApi.Models
{
    public class GitHubBranch
    {
        public string Name { get; set; }
        public CommitData Commit { get; set; }
        public bool Protected { get; set; }
    }

    public class CommitData
    {
        public string Sha { get; set; }
        public string Url { get; set; }
    }
}
