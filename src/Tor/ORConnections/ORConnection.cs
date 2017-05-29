using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Tor.Helpers;
using System.ComponentModel;

namespace Tor
{
    /// <summary>
    /// A class containing information regarding an OR connection within the tor service.
    /// </summary>
    [Serializable]
    public sealed class ORConnection
    {
        private int circuitCount;
        private int id;
        private ORReason reason;
        private ORStatus status;
        private string target;

        /// <summary>
        /// Initializes a new instance of the <see cref="ORConnection"/> class.
        /// </summary>
        internal ORConnection()
        {
            this.circuitCount = 0;
            this.id = 0;
            this.reason = ORReason.None;
            this.status = ORStatus.None;
            this.target = "";
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ORConnection"/> class.
        /// </summary>
        /// <param name="line">The line which was received from the control connection.</param>
        /// <returns>
        ///   <c>ORConnection</c> the new instance.
        /// </returns>
        public static ORConnection FromLine(string line)
        {
            string target;
            ORStatus status;
            string[] parts = StringHelper.GetAll(line, ' ');

            if (parts.Length < 2)
                return null;

            target = parts[0];
            status = ReflectionHelper.GetEnumerator<ORStatus, DescriptionAttribute>(attr => parts[1].Equals(attr.Description, StringComparison.CurrentCultureIgnoreCase));

            ORConnection connection = new ORConnection();
            connection.Status = status;
            connection.Target = target;

            for (int i = 2; i < parts.Length; i++)
            {
                string data = parts[i].Trim();

                if (!data.Contains("="))
                    continue;

                string[] values = data.Split(new[] { '=' }, 2);

                if (values.Length < 2)
                    continue;

                string name = values[0].Trim();
                string value = values[1].Trim();

                if ("REASON".Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    connection.Reason = ReflectionHelper.GetEnumerator<ORReason, DescriptionAttribute>(attr => value.Equals(attr.Description, StringComparison.CurrentCultureIgnoreCase));
                    continue;
                }

                if ("NCIRCS".Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    int circuits;

                    if (int.TryParse(value, out circuits))
                        connection.CircuitCount = circuits;

                    continue;
                }

                if ("ID".Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    int id;

                    if (int.TryParse(value, out id))
                        connection.ID = id;

                    continue;
                }
            }
            return connection;
        }

        #region Properties

        /// <summary>
        /// Gets the number of established and pending circuits.
        /// </summary>
        public int CircuitCount
        {
            get { return circuitCount; }
            internal set { circuitCount = value; }
        }

        /// <summary>
        /// Gets the unique identifier of the connection. This value is only provided in version 0.2.5.2-alpha.
        /// </summary>
        public int ID
        {
            get { return id; }
            internal set { id = value; }
        }

        /// <summary>
        /// Gets the reason for an <c>ORStatus.Closed</c> or <c>ORStatus.Failed</c> state.
        /// </summary>
        public ORReason Reason
        {
            get { return reason; }
            internal set { reason = value; }
        }

        /// <summary>
        /// Gets the status of the connection.
        /// </summary>
        public ORStatus Status
        {
            get { return status; }
            internal set { status = value; }
        }

        /// <summary>
        /// Gets the target of the connection.
        /// </summary>
        public string Target
        {
            get { return target; }
            internal set { target = value; }
        }

        #endregion

    }

    /// <summary>
    /// A class containing a read-only collection of <see cref="ORConnection"/> objects.
    /// </summary>
    public sealed class ORConnectionCollection : ReadOnlyCollection<ORConnection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ORConnectionCollection"/> class.
        /// </summary>
        /// <param name="list">The list of OR connections.</param>
        internal ORConnectionCollection(IList<ORConnection> list) : base(list)
        {
        }
    }
}
