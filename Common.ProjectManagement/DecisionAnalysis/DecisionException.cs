using System;
using System.Runtime.Serialization;

namespace Common.ProjectManagement.DecisionAnalysis
{
    /// <summary>
    /// Represents an exception from within the <see cref="DecisionAnalysis"/> namespace.
    /// </summary>
    public class DecisionException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DecisionException"/> class.
        /// </summary>
        public DecisionException()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DecisionException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public DecisionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DecisionException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DecisionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DecisionException"/> class.
        /// </summary>
        /// <param name="info">The exception's serialization info.</param>
        /// <param name="context">The exception's streaming context.</param>
        protected DecisionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
