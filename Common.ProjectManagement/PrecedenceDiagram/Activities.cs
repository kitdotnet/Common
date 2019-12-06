using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common.ProjectManagement.PrecedenceDiagram
{
    /// <summary>
    /// Represents a keyed collection of <see cref="Activity"/> objects.
    /// </summary>
    public class Activities : KeyedCollection<Guid, Activity>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Activities"/> class.
        /// </summary>
        public Activities() { }

        /// <summary>
        /// Creates a new instance of the <see cref="Activities"/> class.
        /// </summary>
        /// <param name="activities">A collection of <see cref="Activity"/> objects.</param>
        public Activities(IEnumerable<Activity> activities)
        {
            if (activities != null)
            {
                activities.ToList().ForEach(a => Add(a));
            }
        }

        /// <summary>
        /// Add a range of <see cref="Activity"/> objects to the keyed collection.
        /// </summary>
        /// <param name="activities"></param>
        public void AddRange(IEnumerable<Activity> activities)
        {
            if (activities != null)
            {
                activities.ToList().ForEach(a => Add(a));
            }
        }

        protected override Guid GetKeyForItem(Activity item)
        {
            return item.GlobalId;
        }
    }
}
