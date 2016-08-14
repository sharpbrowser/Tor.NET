using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Tor.Controller
{
    /// <summary>
    /// A class containing information regarding a response received back from a control connection.
    /// </summary>
    internal sealed class ConnectionResponse
    {
        private readonly StatusCode code;
        private readonly ReadOnlyCollection<string> responses;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionResponse"/> class.
        /// </summary>
        /// <param name="code">The status code returned by the control connection.</param>
        public ConnectionResponse(StatusCode code)
        {
            this.code = code;
            this.responses = new List<string>().AsReadOnly();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionResponse"/> class.
        /// </summary>
        /// <param name="code">The status code returned by the control connection.</param>
        /// <param name="responses">The responses received back from the control connection.</param>
        public ConnectionResponse(StatusCode code, IList<string> responses)
        {
            this.code = code;
            this.responses = new ReadOnlyCollection<string>(responses);
        }

        #region Properties

        /// <summary>
        /// Gets a read-only collection of responses received from the control connection.
        /// </summary>
        public ReadOnlyCollection<string> Responses
        {
            get { return responses; }
        }

        /// <summary>
        /// Gets the status code returned with the response.
        /// </summary>
        public StatusCode StatusCode
        {
            get { return code; }
        }

        /// <summary>
        /// Gets a value indicating whether the response was successful feedback.
        /// </summary>
        public bool Success
        {
            get { return code == StatusCode.OK; }
        }

        #endregion
    }
}
