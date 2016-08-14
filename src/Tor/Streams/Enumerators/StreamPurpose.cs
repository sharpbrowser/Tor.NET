using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// An enumerator containing the different purposes for a stream, which is provided when extended events are enabled.
    /// </summary>
    public enum StreamPurpose
    {
        /// <summary>
        /// A purpose was not provided.
        /// </summary>
        [Description(null)]
        None,

        /// <summary>
        /// The stream was generated internally for fetching directory information.
        /// </summary>
        [Description("DIR_FETCH")]
        DirectoryFetch,

        /// <summary>
        /// The stream was generated internally for uploading information to a directory authority.
        /// </summary>
        [Description("DIR_READ")]
        DirectoryRead,

        /// <summary>
        /// The stream is a user-initiated DNS request.
        /// </summary>
        [Description("DNS_REQUEST")]
        DNSRequest,

        /// <summary>
        /// The stream is used explicitly for testing whether our hosted directory port is accessible.
        /// </summary>
        [Description("DIRPORT_TEST")]
        DirectoryPortTest,

        /// <summary>
        /// The stream is likely handling a user request. This could also be an internal stream, and none of the other
        /// purposes match the real purpose of the stream.
        /// </summary>
        [Description("USER")]
        User,
    }
}
