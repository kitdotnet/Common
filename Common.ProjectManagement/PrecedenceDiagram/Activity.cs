using System;
using System.Collections.Generic;

namespace Common.ProjectManagement.PrecedenceDiagram
{
    /// <summary>
    /// Represents a project activity.
    /// </summary>
    public class Activity : IEquatable<Activity>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Activity"/> class.
        /// </summary>
        /// <param name="name">The name of the activity.</param>
        /// <param name="duration">The expected duration to complete the activity.</param>
        public Activity(string name, int duration)
        {
            GlobalId = Guid.NewGuid();
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
            Duration = duration > -1 ? duration : throw new ArgumentException("Duration must be positive.");
        }

        /// <summary>
        /// Gets the activity's unique identifier.
        /// </summary>
        public Guid GlobalId { get; }

        /// <summary>
        /// Gets the activity's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the activity's duration.
        /// </summary>
        public int Duration { get; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Activity);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(Activity other)
        {
            return other != null &&
                   GlobalId.Equals(other.GlobalId) &&
                   Name == other.Name &&
                   Duration == other.Duration;
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = -1429336570;
            hashCode = hashCode * -1521134295 + GlobalId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Duration.GetHashCode();
            return hashCode;
        }
    }
}
