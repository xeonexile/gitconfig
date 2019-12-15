using Newtonsoft.Json;

namespace ExileLab.Extensions.Configuration.GitLab
{
    public class GitLabEntry
    {
        [JsonProperty("file_name")] public string FileName { get; set; }

        [JsonProperty("file_path")] public string FilePath { get; set; }

        [JsonProperty("size")] public int Size { get; set; }

        [JsonProperty("encoding")] public string Encoding { get; set; }

        [JsonProperty("content_sha256")] public string ContentSha256 { get; set; }

        [JsonProperty("ref")] public string Ref { get; set; }

        [JsonProperty("blob_id")] public string BlobId { get; set; }

        [JsonProperty("commit_id")] public string CommitId { get; set; }

        [JsonProperty("last_commit_id")] public string LastCommitId { get; set; }

        [JsonProperty("content")] public string Content { get; set; }
    }
}