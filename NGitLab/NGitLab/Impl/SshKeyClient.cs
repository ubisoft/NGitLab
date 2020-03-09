using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class SshKeyClient : ISshKeyClient
    {
        private readonly API _api;
        private readonly string _url;

        public SshKeyClient(API api, int? userId)
        {
            _api = api;
            _url = userId != null ? $"users/{userId}/keys" : "user/keys";
        }

        public IEnumerable<SshKey> All => _api.Get().GetAll<SshKey>(_url);

        public SshKey this[int keyId] => _api.Get().To<SshKey>($"{_url}/{keyId}");

        public SshKey Add(SshKeyCreate key)
        {
            return _api.Post().With(key).To<SshKey>(_url);
        }

        public void Remove(int keyId)
        {
            _api.Delete().Execute($"{_url}/{keyId}");
        }
    }
}