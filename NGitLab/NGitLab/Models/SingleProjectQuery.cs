using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    /// <summary>
    /// Allows to use more advanced gitlab queries for getting projects.
    /// </summary>
    [DataContract]
    public class SingleProjectQuery
    {
        /// <summary>
        /// Include project statistics
        /// </summary>
        public bool? Statistics;
    }
}
