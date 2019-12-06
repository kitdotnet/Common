namespace Common.ProjectManagement.PrecedenceDiagram
{
    /// <summary>
    /// Represents logical relationship types between activity nodes in a precedence diagram.
    /// </summary>
    public enum LogicalRelationshipType
    {
        /// <summary>
        /// The second activity node cannot start until the first one finishes.
        /// </summary>
        FinishToStart = 0,
        /// <summary>
        /// The second activity node cannot finish until the first one finishes.
        /// </summary>
        FinishToFinish,
        /// <summary>
        /// The second activity node cannot start until the first one starts.
        /// </summary>
        StartToStart,
        /// <summary>
        /// The second activity node cannot finish until the first one starts.
        /// </summary>
        StartToFinish
    }
}
