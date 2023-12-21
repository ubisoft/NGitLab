using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class PackageClient : IPackageClient
    {
        private const string PublishPackageUrl = "/projects/{0}/packages/generic/{1}/{2}/{3}?status={4}&select=package_file";
        private const string GetPackagesUrl = "/projects/{0}/packages";
        private const string GetPackageUrl = "/projects/{0}/packages/{1}";

        private readonly API _api;

        public PackageClient(API api)
        {
            _api = api;
        }

        public Task<Package> PublishAsync(int projectId, PackagePublish packagePublish, CancellationToken cancellationToken = default)
        {
            var formData = new FileContent(packagePublish.PackageStream);

            return _api.Put().With(formData).ToAsync<Package>(string.Format(CultureInfo.InvariantCulture,
                PublishPackageUrl, projectId, Uri.EscapeDataString(packagePublish.PackageName),
                Uri.EscapeDataString(packagePublish.PackageVersion), Uri.EscapeDataString(packagePublish.FileName),
                Uri.EscapeDataString(packagePublish.Status)), cancellationToken);
        }

        public IEnumerable<PackageSearchResult> Get(int projectId, PackageQuery packageQuery)
        {
            var url = CreateGetUrl(projectId, packageQuery);
            return _api.Get().GetAllAsync<PackageSearchResult>(url);
        }

        public Task<PackageSearchResult> GetByIdAsync(int projectId, int packageId, CancellationToken cancellationToken = default)
        {
            return _api.Get().ToAsync<PackageSearchResult>(string.Format(CultureInfo.InvariantCulture, GetPackageUrl, projectId, packageId), cancellationToken);
        }

        private static string CreateGetUrl(int projectId, PackageQuery query)
        {
            var url = string.Format(CultureInfo.InvariantCulture, GetPackagesUrl, projectId);

            url = Utils.AddParameter(url, "order_by", query.OrderBy);
            url = Utils.AddParameter(url, "sort", query.Sort);
            url = Utils.AddParameter(url, "status", query.Status);
            url = Utils.AddParameter(url, "page", query.Page);
            url = Utils.AddParameter(url, "per_page", query.PerPage);

            if (query.PackageType != PackageType.all)
            {
                url = Utils.AddParameter(url, "package_type", query.PackageType);
            }

            if (!string.IsNullOrWhiteSpace(query.PackageName))
            {
                url = Utils.AddParameter(url, "package_name", query.PackageName);
            }

            if (query.IncludeVersionless)
            {
                url = Utils.AddParameter(url, "include_versionless", true);
            }

            return url;
        }
    }
}
