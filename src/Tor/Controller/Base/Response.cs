using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Controller
{
    /// <summary>
    /// A class containing information regarding the response received back through the control connection after receiving a command from a client.
    /// </summary>
    internal class Response
    {
        private readonly bool success;

        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class.
        /// </summary>
        /// <param name="success">A value indicating whether the command was received and processed successfully.</param>
        public Response(bool success)
        {
            this.success = success;
        }

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the command was received and processed successfully.
        /// </summary>
        public bool Success
        {
            get { return success; }
        }

        #endregion
    }

    /// <summary>
    /// A class containing a collection of mapped, <see cref="System.String"/> to <see cref="System.String"/> key-value pairs, as
    /// expected from certain commands dispatched to a control connection.
    /// </summary>
    internal sealed class ResponsePairs : Dictionary<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsePairs"/> class.
        /// </summary>
        public ResponsePairs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsePairs"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        public ResponsePairs(int capacity) : base(capacity)
        {
        }
    }
}
