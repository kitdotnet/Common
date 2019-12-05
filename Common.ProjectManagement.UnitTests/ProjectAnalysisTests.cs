using Xunit;
using Xunit.Abstractions;

namespace Common.ProjectManagement.UnitTests
{
    public class ProjectAnalysisTests
    {
        private readonly ITestOutputHelper testOutputHelper;
        public ProjectAnalysisTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ProjectOverBudget()
        {
            var analysis = new ProjectAnalysis(
                plannedValue: 900M,
                earnedValue: 500M,
                actualCost: 1100M,
                budgetAtCompletion: 2000M);

            WriteAnalysis(analysis);

            Assert.True(analysis.CostVariance < 0);
            Assert.True(analysis.ScheduleVariance < 0);
            Assert.True(analysis.VarianceAtCompletion < 0);
            Assert.True(analysis.CostPerformanceIndex < 1);
            Assert.True(analysis.ScheduleVariance < 1);
            Assert.True(analysis.EstimateAtCompletion > analysis.BudgetAtCompletion);
            Assert.True(analysis.ToCompletePerformanceIndexEac < 1);
        }

        [Fact]
        public void ProjectUnderBudget()
        {
            var analysis = new ProjectAnalysis(
                plannedValue: 900M,
                earnedValue: 1000M,
                actualCost: 900M,
                budgetAtCompletion: 2000M);

            WriteAnalysis(analysis);

            Assert.True(analysis.CostVariance > 0);
            Assert.True(analysis.ScheduleVariance > 0);
            Assert.True(analysis.VarianceAtCompletion > 0);
            Assert.True(analysis.CostPerformanceIndex > 1);
            Assert.True(analysis.ScheduleVariance > 1);
            Assert.True(analysis.EstimateAtCompletion < analysis.BudgetAtCompletion);
            Assert.True(analysis.ToCompletePerformanceIndexEac > 1);
        }

        [Fact]
        public void ProjectOnTrack()
        {
            var analysis = new ProjectAnalysis(
                plannedValue: 900M,
                earnedValue: 1100M,
                actualCost: 1100M,
                budgetAtCompletion: 2000M);

            WriteAnalysis(analysis);

            Assert.True(analysis.CostVariance == 0);
            Assert.True(analysis.ScheduleVariance > 0);
            Assert.True(analysis.VarianceAtCompletion == 0);
            Assert.True(analysis.CostPerformanceIndex == 1);
            Assert.True(analysis.ScheduleVariance > 1);
            Assert.True(analysis.EstimateAtCompletion == analysis.BudgetAtCompletion);
            Assert.True(analysis.ToCompletePerformanceIndexEac == 1);
        }

        private void WriteAnalysis(ProjectAnalysis projectAnalysis)
        {
            string moneyFormat = "#,##0.00";
            string percentageFormat = "##0.00";

            testOutputHelper.WriteLine($"Date                                :  {projectAnalysis.DateOfReport.ToString("yyyy-MM-dd")}");
            testOutputHelper.WriteLine($"Estimate Model                      :  {projectAnalysis.EstimateModel.ToString()}");
            testOutputHelper.WriteLine($"Planned Value                       :  {projectAnalysis.PlannedValue.ToString(moneyFormat)}");
            testOutputHelper.WriteLine($"Earned Value                        :  {projectAnalysis.EarnedValue.ToString(moneyFormat)}");
            testOutputHelper.WriteLine($"Actual Cost                         :  {projectAnalysis.ActualCost.ToString(moneyFormat)}");
            testOutputHelper.WriteLine($"Budget at Completion                :  {projectAnalysis.BudgetAtCompletion.ToString(moneyFormat)}");
            testOutputHelper.WriteLine($"Cost Variance                       :  {projectAnalysis.CostVariance.ToString(moneyFormat)}");
            testOutputHelper.WriteLine($"Schedule Variance                   :  {projectAnalysis.ScheduleVariance.ToString(moneyFormat)}");
            testOutputHelper.WriteLine($"Variance at Completion              :  {projectAnalysis.VarianceAtCompletion.ToString(moneyFormat)}");
            testOutputHelper.WriteLine($"Cost Performance Index              :  {projectAnalysis.CostPerformanceIndex.ToString(percentageFormat)}");
            testOutputHelper.WriteLine($"Schedule Performance Index          :  {projectAnalysis.SchedulePerformanceIndex.ToString(percentageFormat)}");
            testOutputHelper.WriteLine($"Estimate at Completion              :  {projectAnalysis.EstimateAtCompletion.ToString(moneyFormat)}");
            testOutputHelper.WriteLine($"Estimate to Complete                :  {projectAnalysis.EstimateToComplete.ToString(moneyFormat)}");  
            testOutputHelper.WriteLine($"To Complete Performance Index (BAC) :  {projectAnalysis.ToCompletePerformanceIndexBac.ToString(percentageFormat)}");
            testOutputHelper.WriteLine($"To Complete Performance Index (EAC) :  {projectAnalysis.ToCompletePerformanceIndexEac.ToString(percentageFormat)}");
        }
    }
}