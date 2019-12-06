using System;

namespace Common.ProjectManagement
{
    /// <summary>
    /// Represents the mathematics for project analysis.
    /// </summary>
    public class ProjectAnalysis : IEquatable<ProjectAnalysis>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ProjectAnalysis"/> class.
        /// </summary>
        /// <param name="plannedValue">The dollar amount representing Planned Value.</param>
        /// <param name="earnedValue">The dollar amount representing Earned Value.</param>
        /// <param name="actualCost">The dollar amount representing Actual Cost.</param>
        /// <param name="budgetAtCompletion">The dollar amount representing Budget At Completion.</param>
        /// <param name="estimateModel">The <see cref="ProjectManagement.EstimateModel"/> used for estimating.</param>
        /// <param name="newEstimate">The dollar amount representing the new bottom-up estimate to complete the project.</param>
        /// <param name="dateOfReport">The date of the analysis.</param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="estimateModel"/> is 
        /// <see cref="ProjectManagement.EstimateModel.Reestimate"/> and <paramref name="newEstimate"/> is null.</exception>
        /// <remarks>When <paramref name="dateOfReport"/> is null, <see cref="DateTime.Now"/> is used.</remarks>
        public ProjectAnalysis(decimal plannedValue,
            decimal earnedValue,
            decimal actualCost,
            decimal budgetAtCompletion,
            EstimateModel estimateModel = EstimateModel.CpiRemainsConstant,
            decimal? newEstimate = default,
            DateTime? dateOfReport = default)
        {
            DateOfReport = dateOfReport ?? DateTime.Now;
            PlannedValue = plannedValue;
            EarnedValue = earnedValue;
            ActualCost = actualCost;
            BudgetAtCompletion = budgetAtCompletion;
            EstimateModel = estimateModel;
            NewEstimate = newEstimate;

            if (EstimateModel == EstimateModel.Reestimate && NewEstimate == default)
            {
                throw new ArgumentNullException($"When the estimate model is {EstimateModel.Reestimate.ToString()}, {nameof(NewEstimate)} is required.");
            }
        }

        /// <summary>
        /// Gets the date of this analysis.
        /// </summary>
        public DateTime DateOfReport { get; }

        /// <summary>
        /// Gets the <see cref="ProjectManagement.EstimateModel"/> used for estimating.
        /// </summary>
        public EstimateModel EstimateModel { get; }

        /// <summary>
        /// Gets the new, bottoms-up estimate to complete the project.
        /// </summary>
        public decimal? NewEstimate { get; }

        /// <summary>
        /// Gets the authorized budget assigned to scheduled work.
        /// </summary>
        /// <remarks>This is the value of the work planned to be completed to a point in time,
        /// usually <see cref="DateOfReport"/>, or project completion.</remarks>
        public decimal PlannedValue { get; }

        /// <summary>
        /// Gets the measure of work performed expressed in terms of the budget authorized for that work.
        /// </summary>
        /// <remarks>The planned value of all the work completed (earned) to a point in time,
        /// usually <see cref="DateOfReport"/>, without reference to actual costs.</remarks>
        public decimal EarnedValue { get; }

        /// <summary>
        /// Gets the realized cost incurred for the work performed on an activity during a specific time period.
        /// </summary>
        /// <remarks>This is the actual cost of all work completed to a point in time, usually <see cref="DateOfReport"/>.</remarks>
        public decimal ActualCost { get; }

        /// <summary>
        /// Gets the sum of all budgets established for the work to be performed.
        /// </summary>
        /// <remarks>This is value of total planned work, the project cost baseline.</remarks>
        public decimal BudgetAtCompletion { get; }

        /// <summary>
        /// Gets the amount of budget deficit or surplus at a given point in time, expressed as the
        /// difference between the earned value and the actual cost.
        /// </summary>
        /// <remarks>This is the difference the value of work completed to a point in time, 
        /// usually <see cref="DateOfReport"/>, and the actual costs to the same point in time.
        /// A positive value means that the project's cost is under planned cost.
        /// A negative value means that the project's cost is over planned cost.
        /// A neutral value means that the project's cost is on track.
        /// </remarks>
        public decimal CostVariance => EarnedValue - ActualCost;

        /// <summary>
        /// Gets the amount by which the project is ahead or behind the planned delivery date, at a given point in time,
        /// expressed as the difference between the earned value and the planned value.
        /// </summary>
        /// <remarks>This is the difference between the work completed to a point in time, usually <see cref="DateOfReport"/>,
        /// and the work planned to be completed to the same point in time.
        /// A positive value means that the project is ahead of schedule.
        /// A negative value means that the project is behind schedule.
        /// A neutral value means that the project's schedule is on track.
        /// </remarks>
        public decimal ScheduleVariance => EarnedValue - PlannedValue;

        /// <summary>
        /// Gets a projection of the amount of budget deficit or surplus, expressed as the differnce between
        /// the budget at completion and the estimate at completion.
        /// </summary>
        /// <remarks>This is the estimated difference in cost at the completion of the project.
        /// A positive value means that the project's estimated cost is under planned cost.
        /// A negative value means that the project's estimated cost is above planned cost.
        /// A neutral value means that the project's estimated cost is on track.</remarks>
        public decimal VarianceAtCompletion => BudgetAtCompletion - EstimateAtCompletion;

        /// <summary>
        /// Gets a measure of the cost efficiency of budgeted resources expressed as the ratio of earned
        /// value to actual cost.
        /// </summary>
        /// <remarks>A CPI of 1.0 means the project is exactly on budget, that the work actually done so far
        /// is exactly the same as the cost so far. Other values show the percentage of how mucch costs are over
        /// or under the budgeted amount for work accomplished.
        /// A value greater than 1.0 means the project's cost is under planned cost.
        /// A value less than 1.0 means the project's cost is over planned cost.
        /// A value of exactly 1.0 means the project's cost is exactly on track.</remarks>
        public decimal CostPerformanceIndex => EarnedValue / ActualCost;

        /// <summary>
        /// Gets a measure of schedule efficiency expressed as the ratio of earned value to planned value.
        /// </summary>
        /// <remarks>An SPI of 1.0 means that the project is exactly on schedule, that the work done so far
        /// is exactly the same as the work planned to be done so far. Other values show the percentage of how
        /// much costs are over or under the budgeted amount for work planned.
        /// A value greater than 1.0 means the project is ahead of schedule.
        /// A value less than 1.0 means the project's is behind schedule.
        /// A value of exactly 1.0 means the project's is exactly on schedule.</remarks>
        public decimal SchedulePerformanceIndex => EarnedValue / PlannedValue;

        /// <summary>
        /// Gets the expected total cost of completing all work expressed as the sum of the actual cost to date
        /// and the estimate to complete.
        /// </summary>
        /// <remarks>This calculation of this value is based on <see cref="ProjectManagement.EstimateModel"/>.
        /// If the CPI is expected to be the same for the remainder of the project, EAC is calculated using
        /// <see cref="ProjectManagement.EstimateModel.CpiRemainsConstant"/>.
        /// If future work will be accomplished at the planned rate, EAC is calculated using
        /// <see cref="ProjectManagement.EstimateModel.PlannedRate"/>.
        /// If the initial plan is no longer value, a <see cref="NewEstimate"/> should be provided, and EAC
        /// is calculated using <see cref="ProjectManagement.EstimateModel.Reestimate"/>.
        /// If both the CPI and SPI influence the remaining work, EAC is calculated using
        /// <see cref="ProjectManagement.EstimateModel.CpiAndSpi"/>.
        /// </remarks>
        public decimal EstimateAtCompletion
        {
            get
            {
                switch (EstimateModel)
                {
                    case EstimateModel.CpiRemainsConstant:
                        return BudgetAtCompletion / CostPerformanceIndex;
                    case EstimateModel.PlannedRate:
                        return ActualCost + BudgetAtCompletion - EarnedValue;
                    case EstimateModel.Reestimate:
                        return ActualCost + NewEstimate.GetValueOrDefault();
                    case EstimateModel.CpiAndSpi:
                        return ActualCost + ((BudgetAtCompletion - EarnedValue) / (CostPerformanceIndex * SchedulePerformanceIndex));
                    default:
                        throw new Exception($"Unknown estimate model: {this.EstimateModel.ToString()}");
                }
            }
        }

        /// <summary>
        /// Gets the expected cost to finish all the remaining project work.
        /// </summary>
        /// <remarks>If the <see cref="EstimateModel"/> is <see cref="ProjectManagement.EstimateModel.Reestimate"/>
        /// and a <see cref="NewEstimate"/> has been provided, the ETC wil be the <see cref="NewEstimate"/>.
        /// Otherwise, ETC assumes work is proceeding on plan.</remarks>
        public decimal EstimateToComplete
        {
            get
            {
                if (EstimateModel == EstimateModel.Reestimate && NewEstimate != default)
                {
                    return NewEstimate.GetValueOrDefault();
                }
                else
                {
                    return EstimateAtCompletion - ActualCost;
                }
            }
        }

        /// <summary>
        /// Gets a measure of the cost performance that must be achieved with the remaining resources in order to meet the goal defined by 
        /// <see cref="BudgetAtCompletion"/>, expressed as the ratio of the cost to finish the outstanding work to the budget available.
        /// </summary>
        /// <remarks>This is the efficiency that must be maintained in order to complete on plan.
        /// A value of 1.0 means that the cost performance must remain the same to complete.
        /// A value greater than 1.0 means that the project is harder to complete.
        /// A value less than 1.0 means that the project is easier to complete.</remarks>
        public decimal ToCompletePerformanceIndexBac => (BudgetAtCompletion - EarnedValue) / (BudgetAtCompletion - ActualCost);

        /// <summary>
        /// Gets a measure of the cost performance that must be achieved with the remaining resources in order to meet the goal defined by
        /// <see cref="EstimateAtCompletion"/>, expressed as the ratio of the cost to finish the outstanding work to the budget available.
        /// </summary>
        /// <remarks>This is the efficiency that must be maintained in order to complete the current <see cref="EstimateAtCompletion"/>.
        /// A value of 1.0 means that the cost performance must remain the same to complete.
        /// A value greater than 1.0 means that the project is harder to complete.
        /// A value less than 1.0 means that the project is easier to complete.</remarks>
        public decimal ToCompletePerformanceIndexEac => (BudgetAtCompletion - EarnedValue) / (EstimateAtCompletion - ActualCost);

        /// <summary>
        /// Estimate the cost of a task or project.
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

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Project analysis as of {DateOfReport.ToString("yyyy-MM-dd")}";
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ProjectAnalysis);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(ProjectAnalysis other)
        {
            return other != null &&
                   DateOfReport == other.DateOfReport &&
                   EstimateModel == other.EstimateModel &&
                   NewEstimate == other.NewEstimate &&
                   PlannedValue == other.PlannedValue &&
                   EarnedValue == other.EarnedValue &&
                   ActualCost == other.ActualCost &&
                   BudgetAtCompletion == other.BudgetAtCompletion;
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 849256091;
            hashCode = hashCode * -1521134295 + DateOfReport.GetHashCode();
            hashCode = hashCode * -1521134295 + EstimateModel.GetHashCode();
            hashCode = hashCode * -1521134295 + NewEstimate.GetHashCode();
            hashCode = hashCode * -1521134295 + PlannedValue.GetHashCode();
            hashCode = hashCode * -1521134295 + EarnedValue.GetHashCode();
            hashCode = hashCode * -1521134295 + ActualCost.GetHashCode();
            hashCode = hashCode * -1521134295 + BudgetAtCompletion.GetHashCode();
            return hashCode;
        }
    }
}
