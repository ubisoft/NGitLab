using NGitLab.Impl;
using NGitLab.Models;
using System;
using System.IO;
using System.Collections.Generic;

namespace NGitLab.Tests
{
    public class MockHttpRequestor : HttpRequestor
    {
        public MockHttpRequestor(string hostUrl, string apiToken)
            : base(hostUrl, apiToken)
        {
        }

        public override void Stream(string tailUrl, Action<Stream> parser)
        {
            using (var stream = new FileStream(Path.Combine(Path.GetTempPath(), GetFileName(tailUrl)), FileMode.Open))
            {
                parser(stream);
            }
        }

        public override IEnumerable<T> GetAll<T>(string tailUrl)
        {
            using (var stream = new FileStream(Path.Combine(Path.GetTempPath(), GetFileName(tailUrl)), FileMode.Open))
            {
                return SimpleJson.DeserializeObject<T[]>(new StreamReader(stream).ReadToEnd());
            }
        }

        public override T To<T>(string tailUrl)
        {
            using (var stream = new FileStream(Path.Combine(Path.GetTempPath(), GetFileName(tailUrl)), FileMode.Open))
            {
                return SimpleJson.DeserializeObject<T>(new StreamReader(stream).ReadToEnd());
            }
        }

        private string GetFileName(string tailUrl)
        {
            string fileName = string.Empty;
            switch (tailUrl)
            {
                case "/user":
                    fileName = "user";
                    break;
                case User.Url:
                    fileName = "users";
                    break;
                case Project.Url:
                    fileName = "projects";
                    break;
                case Project.Url + "/owned":
                    fileName = "projects";
                    break;
                case Project.Url + "/all":
                    fileName = "projects";
                    break;
                case User.Url + "/1":
                    fileName = "user";
                    break;
                case Project.Url + "/4/hooks":
                    fileName = "hooks";
                    break;
                case Project.Url + "/4/repository/branches":
                    fileName = "branches";
                    break;
                case Project.Url + "/4/merge_request/5/comments":
                    fileName = "merge_requests_comments";
                    break;
                case Project.Url + "/4/merge_request/4/comments":
                    fileName = "merge_requests_comment";
                    break;
                case Project.Url + "/4/repository/commits":
                    fileName = "commits";
                    break;
                case Project.Url + "/4/merge_requests":
                    fileName = "merge_requests";
                    break;
                case Project.Url + "/4/merge_requests?state=opened":
                    fileName = "merge_requests_opened";
                    break;
                case Project.Url + "/4/repository/branches/master":
                    fileName = "branch_master";
                    break;
                case Project.Url + "/4/repository/commits/6104942438C14EC7BD21C6CD5BD995272B3FAFF6":
                    fileName = "commit_sha";
                    break;
                case Project.Url + "/4/repository/commits/ED899A2F4B50B4370FEEEA94676502B42383C746/diff":
                    fileName = "commit_diff";
                    break;
                case Project.Url + "/4/merge_request/1":
                    fileName = "merge_request";
                    break;
                case Project.Url + "/4/merge_request/4/merge":
                    fileName = "merge_request";
                    break;
                case Project.Url + "/4/merge_request/5":
                    fileName = "merge_request";
                    break;
                default:
                    throw new NotImplementedException(tailUrl);
            }

            return $"NGitLab.Tests.{fileName}.json";
        }
    }
}
