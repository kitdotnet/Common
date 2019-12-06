using System;

namespace Common.ProjectManagement.PrecedenceDiagram
{
    /// <summary>
    /// Represents the type of relationship between two activities.
    /// </summary>
    public class ActivityRelationshipType : IEquatable<ActivityRelationshipType>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ActivityRelationshipType"/> class.
        /// </summary>
        /// <param name="logicalRelationshipType">The logical relationship between the two activities.</param>
        /// <param name="lead">The lead time.</param>
        /// <param name="lag">The lag time.</param>
        public ActivityRelationshipType(LogicalRelationshipType logicalRelationshipType = LogicalRelationshipType.FinishToStart,
            int lead = 0,
            int lag = 0)
        {
            LogicalRelationshipType = logicalRelationshipType;
            Lead = lead;
            Lag = lag;
        }

        /// <summary>
        /// Gets the logical relationship between two activities.
        /// </summary>
        public LogicalRelationshipType LogicalRelationshipType { get; }

        /// <summary>
        /// Gets the lead for this relationship.  A lead is the amount of time a successor activity
        /// can be advanced with respect to a predecessor activity. 
        /// </summary>
        /// <remarks>This is sometimes known as negative lag.</remarks>
        /// <example>On a construction project, the landscaping could be scheduled to begin 2 weeks
        /// prior to the to the scheduled punch list completion. This would be a finish-to-start with
        /// a 2 week lead.</example>
        public int Lead { get; }

        /// <summary>
        /// Gets the lag for this relationship. A lag is the amount of time a successor activity will
        /// be delayed with respect to a predecessor activity.
        /// </summary>
        /// <example>A technical writing team may begin editing the draft of a large document 15 days
        /// after they begin writing it. This could be a start-to-start relationship with a 15 day lag.
        /// </example>
        public int Lag { get; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return LogicalRelationshipType.ToString();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ActivityRelationshipType);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(ActivityRelationshipType other)
        {
            return other != null &&
                   LogicalRelationshipType == other.LogicalRelationshipType &&
                   Lead == other.Lead &&
                   Lag == other.Lag;
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = -1685309748;
            hashCode = hashCode * -1521134295 + LogicalRelationshipType.GetHashCode();
            hashCode = hashCode * -1521134295 + Lead.GetHashCode();
            hashCode = hashCode * -1521134295 + Lag.GetHashCode();
            return hashCode;
        }
    }
}
