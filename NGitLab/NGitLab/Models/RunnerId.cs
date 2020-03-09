using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class RunnerId
    {
        public RunnerId()
        {
        }

        public RunnerId(int id)
        {
            Id = id;
        }

        [DataMember(Name = "runner_id")]
        public int Id;
    }
}
