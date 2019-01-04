using System;
using System.Collections.Generic;
using System.IO;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class JobClient : IJobClient
    {
        private readonly API _api;
        private readonly string _jobsPath;

        public JobClient(API api, int projectId)
        {
            _api = api;
            _jobsPath = $"{Project.Url}/{projectId}/jobs";
        }

        public IEnumerable<Job> GetJobs(JobScopeMask scope)
        {
            var url = _jobsPath;

            if (scope != JobScopeMask.All)
            {
                foreach (Enum value in Enum.GetValues(scope.GetType()))
                {
                    if (scope.HasFlag(value))
                    {
                        url = Utils.AddParameter(url, "scope", value.ToString().ToLowerInvariant());
                    }
                }
            }

            return _api.Get().GetAll<Job>(url);
        }

        public Job RunAction(int jobId, JobAction action) => _api.Post().To<Job>($"{_jobsPath}/{jobId}/{action.ToString().ToLowerInvariant()}");
        public Job Get(int jobId) => _api.Get().To<Job>($"{_jobsPath}/{jobId}");

        public byte[] GetJobArtifacts(int jobId)
        {
            byte[] result = null;
            _api.Get().Stream($"{_jobsPath}/{jobId}/artifacts", s =>
            {
                using (var ms = new MemoryStream())
                {
                    s.CopyTo(ms);
                    result = ms.ToArray();
                }
            });
            return result;
        }

        public string GetTrace(int jobId)
        {
            var result = "";
            _api.Get().Stream($"{_jobsPath}/{jobId}/trace", s =>
            {
                result = new StreamReader(s).ReadToEnd();
            });
            return result;
        }
    }
}
