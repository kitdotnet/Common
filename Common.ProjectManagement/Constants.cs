using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ProjectManagement
{
    public enum CostEstimateDistribution
    {
        Triangular = 0,
        Beta = 1
    }

    public enum EstimateModel
    {
        CpiRemainsConstant = 0,
        PlannedRate = 1,
        Reestimate = 2,
        CpiAndSpi = 3
    }
}
