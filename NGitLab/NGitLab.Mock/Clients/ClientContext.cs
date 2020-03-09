namespace NGitLab.Mock.Clients
{
    internal sealed class ClientContext
    {
        public ClientContext(GitLabServer server, User user)
        {
            Server = server;
            User = user;
        }

        public GitLabServer Server { get; }

        public User User { get; }

        public bool IsAuthenticated => User != null;
    }
}
