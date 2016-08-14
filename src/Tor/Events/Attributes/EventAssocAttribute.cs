using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tor.Events
{
    /// <summary>
    /// Specifies that a dispatcher class is associated with an event.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class EventAssocAttribute : Attribute
    {
        private readonly Event evt;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAssocAttribute"/> class.
        /// </summary>
        /// <param name="evt">The event the class is associated with.</param>
        public EventAssocAttribute(Event evt)
        {
            this.evt = evt;
        }

        #region Properties

        /// <summary>
        /// Gets the event the class is associated with.
        /// </summary>
        public Event Event
        {
            get { return evt; }
        }

        #endregion
    }
}
