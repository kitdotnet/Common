using System;
using System.Collections.Generic;

namespace Common.ProjectManagement.PrecedenceDiagram
{
    /// <summary>
    /// Represents the relationship between two <see cref="Activity"/> objects.
    /// </summary>
    public class ActivityRelationship : IEquatable<ActivityRelationship>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ActivityRelationship"/> class.
        /// </summary>
        /// <param name="activity1">The first <see cref="Activity"/>.</param>
        /// <param name="activity2">The second <see cref="Activity"/>.</param>
        /// <param name="activityRelationshipType">The type of relationship between the two activities.</param>
        public ActivityRelationship(Activity activity1, Activity activity2, ActivityRelationshipType activityRelationshipType)
        {
            Activity1 = activity1 ?? throw new ArgumentNullException(nameof(activity1));
            Activity2 = activity2 ?? throw new ArgumentNullException(nameof(activity2));
            ActivityRelationshipType = activityRelationshipType ?? throw new ArgumentNullException(nameof(activityRelationshipType));
        }

        /// <summary>
        /// Gets the first <see cref="Activity"/>.
        /// </summary>
        public Activity Activity1 { get; }

        /// <summary>
        /// Gets the second <see cref="Activity"/>.
        /// </summary>
        public Activity Activity2 { get; }

        /// <summary>
        /// Gets the relationship between the two activities.
        /// </summary>
        public ActivityRelationshipType ActivityRelationshipType { get; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{Activity1.Name} {Activity2.Name} {ActivityRelationshipType.ToString()}";
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ActivityRelationship);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(ActivityRelationship other)
        {
            return other != null &&
                   EqualityComparer<Activity>.Default.Equals(Activity1, other.Activity1) &&
                   EqualityComparer<Activity>.Default.Equals(Activity2, other.Activity2) &&
                   EqualityComparer<ActivityRelationshipType>.Default.Equals(ActivityRelationshipType, other.ActivityRelationshipType);
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = -111270445;
            hashCode = hashCode * -1521134295 + EqualityComparer<Activity>.Default.GetHashCode(Activity1);
            hashCode = hashCode * -1521134295 + EqualityComparer<Activity>.Default.GetHashCode(Activity2);
            hashCode = hashCode * -1521134295 + EqualityComparer<ActivityRelationshipType>.Default.GetHashCode(ActivityRelationshipType);
            return hashCode;
        }
    }
}
