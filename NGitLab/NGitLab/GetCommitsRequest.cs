namespace NGitLab
{
    public class GetCommitsRequest
    {
        public string RefName { get; set; }
        public string Path { get; set; }
        public bool? FirstParent { get; set; }
        public int MaxResults { get; set; }
    }
}
