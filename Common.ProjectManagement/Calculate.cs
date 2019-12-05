using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ProjectManagement
{
    public static class Calculate
    {
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
