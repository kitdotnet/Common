using System;

namespace Common.ProjectManagement
{
    /// <summary>
    /// Utility class for calculating cost estimates.
    /// </summary>
    public static class Calculate
    {
        /// <summary>
        /// Estimate the code of a project task.
        /// </summary>
        /// <param name="optimistic">An optimistic estimate.</param>
        /// <param name="pessimistic">A pessimistic estimate.</param>
        /// <param name="mostLikely">The most likely estimate.</param>
        /// <param name="costEstimateDistribution">The <see cref="CostEstimateDistribution"/>
        /// used for the cost estimate calculation.</param>
        /// <returns></returns>
        public static decimal CostEstimate(decimal optimistic,
            decimal pessimistic,
            decimal mostLikely,
            CostEstimateDistribution costEstimateDistribution = CostEstimateDistribution.Triangular)
        {
            return costEstimateDistribution switch
            {
                CostEstimateDistribution.Beta => (optimistic + mostLikely + pessimistic) / 3,
                CostEstimateDistribution.Triangular => (optimistic + (4 * mostLikely) + pessimistic) / 6,
                _ => throw new ArgumentException($"Unknown cost estimate distribution: {costEstimateDistribution.ToString()}"),
            };
        }
    }
}
