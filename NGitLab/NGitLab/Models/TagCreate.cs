using System.Runtime.Serialization;

namespace NGitLab.Models
{
	[DataContract]
	public class TagCreate
	{
		[DataMember(Name = "tag_name")]
		public string TagName;

		[DataMember(Name = "ref")]
		public string Ref;

		[DataMember(Name = "message")]
		public string Message;

		[DataMember(Name = "release_description")]
		public string ReleaseDescription;
	}
}