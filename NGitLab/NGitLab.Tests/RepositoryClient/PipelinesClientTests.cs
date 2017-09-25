using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests.RepositoryClient {
    public class PipelinesClientTests {
        readonly IRepositoryClient repo;

        public PipelinesClientTests() {
            var project = Config.Connect().Projects.Owned().FirstOrDefault(x => x.Name == "pipelines");
            repo = Config.Connect().GetRepository(project.Id);
        }

        [Test]
        [Category("Server_Required")]
        public void PipelinesDataGetAll() {
            var pipelines = repo.Pipelines.All();
            pipelines.ShouldNotBeEmpty();
        }
        [Test]
        [Category("Server_Required")]
        public void PipelinesDataCheckProperties() {
            var pipelines = repo.Pipelines.All();
            var data = pipelines.First();
            data.Id.ShouldBeGreaterThan(0);
            data.Ref.ShouldNotBeNull();
            data.Sha1.ShouldNotBeNull();
            data.Status.ShouldBe(PipelineStatus.success);
        }
        [Test]
        [Category("Server_Required")]
        public void PipelineGetSingle() {
            var pipelines = repo.Pipelines.All();
            var data = pipelines.First();
            var pipeline = repo.Pipelines.Get(data.Id);
            pipeline.Id.ShouldBeGreaterThan(0);
            pipeline.BeforeSha.ShouldNotBeNull();
            pipeline.Sha.ShouldNotBeNull();
            pipeline.Ref.ShouldBe("master");
            pipeline.Status.ShouldBe(PipelineStatus.success);
            pipeline.User.ShouldNotBeNull();
        }
        [Test]
        [Category("Server_Required")]
        public void PipelinesGetJobs() {
            var pipelines = repo.Pipelines.All();
            var data = pipelines.First();
            var jobs = repo.Pipelines.GetJobs(data.Id);
            jobs.ShouldNotBeEmpty();
        }
        [Test]
        [Category("Server_Required")]
        public void PipelinesJobCheckProperties() {
            var pipelines = repo.Pipelines.All();
            var data = pipelines.First();
            var job = repo.Pipelines.GetJobs(data.Id).First();
            job.Name.ShouldNotBeNull();
            job.Commit.ShouldNotBeNull();
            job.CreatedAt.ShouldNotBeNull();
            job.FinishedAt.ShouldNotBeNull();
            job.Id.ShouldBeGreaterThan(0);
            job.Pipeline.ShouldNotBeNull();
        }
    }
}
