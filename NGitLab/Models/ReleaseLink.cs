using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract(Name = "ReleaseLinkType")]
    public enum ReleaseLinkType
    {
        [EnumMember(Value = "other")]
        Other,
        [EnumMember(Value = "runbook")]
        Runbook,
        [EnumMember(Value = "image")]
        Image,
        [EnumMember(Value = "package")]
        Package,
    }

    [DataContract]
    public class ReleaseLink
    {
        [DataMember(Name = "id")]
        public int? Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "url")]
        public string Url;

        [DataMember(Name = "direct_asset_url")]
        public string DirectAssetUrl;

        [DataMember(Name = "external")]
        public bool External;

        [DataMember(Name = "link_type")]
        public ReleaseLinkType LinkType;
    }
}
