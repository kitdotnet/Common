using System;
using System.Collections.Generic;

namespace Common.ProjectManagement.PrecedenceDiagram
{
    /// <summary>
    /// Represents an <see cref="Activity"/> within a precedence diagram.
    /// </summary>
    public class ActivityNode : IEquatable<ActivityNode>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ActivityNode"/> class.
        /// </summary>
        /// <param name="activity">The underlying <see cref="ActivityNode"/>.</param>
        public ActivityNode(Activity activity)
        {
            Activity = activity ?? throw new ArgumentNullException(nameof(activity));
        }

        /// <summary>
        /// Gets the underlying <see cref="Activity"/>.
        /// </summary>
        public Activity Activity { get; }

        /// <summary>
        /// Gets the duration of the underlying <see cref="Activity"/>.
        /// </summary>
        public int Duration => Activity.Duration;

        /// <summary>
        /// Gets the early start (ES) value for this node.
        /// </summary>
        public int EarlyStart { get; internal set; }

        /// <summary>
        /// Gets the early finish (EF) value for this node.
        /// </summary>
        public int EarlyFinish => EarlyStart + Duration;

        /// <summary>
        /// Gets the late start (LS) value for this node.
        /// </summary>
        public int LateStart => LateFinish - Duration;

        /// <summary>
        /// Gets the late finish (LF) value for this node.
        /// </summary>
        public int LateFinish { get; internal set; }

        /// <summary>
        /// Gets the total float for this node.
        /// </summary>
        public int TotalFloat => LateStart - EarlyStart;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Activity.Name;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ActivityNode);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(ActivityNode other)
        {
            return other != null &&
                   EqualityComparer<Activity>.Default.Equals(Activity, other.Activity) &&
                   Duration == other.Duration &&
                   EarlyStart == other.EarlyStart &&
                   EarlyFinish == other.EarlyFinish &&
                   LateStart == other.LateStart &&
                   LateFinish == other.LateFinish &&
                   TotalFloat == other.TotalFloat;
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 786472466;
            hashCode = hashCode * -1521134295 + EqualityComparer<Activity>.Default.GetHashCode(Activity);
            hashCode = hashCode * -1521134295 + Duration.GetHashCode();
            hashCode = hashCode * -1521134295 + EarlyStart.GetHashCode();
            hashCode = hashCode * -1521134295 + EarlyFinish.GetHashCode();
            hashCode = hashCode * -1521134295 + LateStart.GetHashCode();
            hashCode = hashCode * -1521134295 + LateFinish.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalFloat.GetHashCode();
            return hashCode;
        }
    }
}
