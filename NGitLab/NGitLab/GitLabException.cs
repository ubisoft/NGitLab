using System;
using System.Net;
using System.Runtime.Serialization;
using NGitLab.Impl;

namespace NGitLab
{
    /// <summary>
    /// Thrown when GitLab returns an error on a call.
    /// </summary>
    [Serializable]
    public class GitLabException : Exception
    {
        public GitLabException()
        {
        }

        public GitLabException(string message) 
            : base(message)
        {
        }

        public GitLabException(string message, Exception inner) 
            : base(message, inner)
        {
        }

        protected GitLabException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// The error code returned from the server.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        public JsonObject ErrorObject { get; set; }

        /// <summary>
        /// The extracted message from the error object.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The call that triggerred the error.
        /// </summary>
        public Uri OriginalCall { get; set; }
    }
}