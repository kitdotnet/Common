using Common.ProjectManagement.DecisionAnalysis;
using Xunit;
using Xunit.Abstractions;
using System.Linq;

namespace Common.ProjectManagement.UnitTests
{
    public class DecisionAnalysisTests
    {
        private readonly ITestOutputHelper testOutputHelper;
        public DecisionAnalysisTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// This test comes directly from the PMBOK 6th edition, pg. 435
        /// </summary>
        [Fact]
        public void CorrectDecision()
        {
            Decision decision = new Decision("Build or Upgrade?", "Should we build a new plant or upgrade the existing plant?");

            DecisionNode buildNewPlant = new DecisionNode("Build new plant", 120000000M);
            buildNewPlant.ChanceNodes.Add(new ChanceNode("Strong Demand", buildNewPlant.Cost, .6M, 200000000M));
            buildNewPlant.ChanceNodes.Add(new ChanceNode("Weak Demand", buildNewPlant.Cost, .4M, 90000000M));

            DecisionNode upgradePlant = new DecisionNode("Upgrade plant", 50000000M);
            upgradePlant.ChanceNodes.Add(new ChanceNode("Strong Demand", upgradePlant.Cost, .6M, 120000000M));
            upgradePlant.ChanceNodes.Add(new ChanceNode("Weak Demand", upgradePlant.Cost, .4M, 60000000M));

            decision.DecisionNodes.Add(buildNewPlant);
            decision.DecisionNodes.Add(upgradePlant);

            decision.Evaluate();

            Assert.NotEmpty(decision.SelectedNodes);
            Assert.Single(decision.SelectedNodes);
            Assert.Equal(decision.SelectedNodes[0], upgradePlant);

            Assert.Equal(36000000M, buildNewPlant.ComputedValue);
            Assert.Equal(46000000M, upgradePlant.ComputedValue);

            var chanceNode = decision.DecisionNodes.SelectMany(n => n.ChanceNodes.Where(c => c.ParentCost == 120000000M && c.Title == "Strong Demand")).First();
            Assert.Equal(80000000M, chanceNode.ComputedValue);
            chanceNode = decision.DecisionNodes.SelectMany(n => n.ChanceNodes.Where(c => c.ParentCost == 120000000M && c.Title == "Weak Demand")).First();
            Assert.Equal(-30000000M, chanceNode.ComputedValue);

            chanceNode = decision.DecisionNodes.SelectMany(n => n.ChanceNodes.Where(c => c.ParentCost == 50000000M && c.Title == "Strong Demand")).First();
            Assert.Equal(70000000M, chanceNode.ComputedValue);

            chanceNode = decision.DecisionNodes.SelectMany(n => n.ChanceNodes.Where(c => c.ParentCost == 50000000M && c.Title == "Weak Demand")).First();
            Assert.Equal(10000000M, chanceNode.ComputedValue);
        }

        [Fact]
        public void TwoCorrectDecisions()
        {
            Decision decision = new Decision("Build or Upgrade?", "From PMBOK 6th Edition pg 435");

            DecisionNode buildNewPlant = new DecisionNode("Build new plant", 120000000M);
            buildNewPlant.ChanceNodes.Add(new ChanceNode("Strong Demand", buildNewPlant.Cost, .6M, 200000000M));
            buildNewPlant.ChanceNodes.Add(new ChanceNode("Weak Demand", buildNewPlant.Cost, .4M, 90000000M));

            DecisionNode upgradePlant = new DecisionNode("Upgrade plant", 50000000M);
            upgradePlant.ChanceNodes.Add(new ChanceNode("Strong Demand", buildNewPlant.Cost, .6M, 200000000M));
            upgradePlant.ChanceNodes.Add(new ChanceNode("Weak Demand", buildNewPlant.Cost, .4M, 90000000M));

            decision.DecisionNodes.Add(buildNewPlant);
            decision.DecisionNodes.Add(upgradePlant);

            decision.Evaluate();

            Assert.NotEmpty(decision.SelectedNodes);
            Assert.Equal(2, decision.SelectedNodes.Count);
            Assert.Equal(decision.SelectedNodes[0], buildNewPlant);
            Assert.Equal(decision.SelectedNodes[1], upgradePlant);
        }

        [Fact]
        public void ChancesDontAddUp_Throws()
        {
            Decision decision = new Decision("Build or Upgrade?", "From PMBOK 6th Edition pg 435");

            DecisionNode buildNewPlant = new DecisionNode("Build new plant", 120000000M);
            buildNewPlant.ChanceNodes.Add(new ChanceNode("Strong Demand", buildNewPlant.Cost, .6M, 200000000M));
            buildNewPlant.ChanceNodes.Add(new ChanceNode("Weak Demand", buildNewPlant.Cost, .4M, 90000000M));

            DecisionNode upgradePlant = new DecisionNode("Upgrade plant", 50000000M);
            upgradePlant.ChanceNodes.Add(new ChanceNode("Strong Demand", upgradePlant.Cost, .5M, 120000000M));
            upgradePlant.ChanceNodes.Add(new ChanceNode("Weak Demand", upgradePlant.Cost, .4M, 60000000M));

            decision.DecisionNodes.Add(buildNewPlant);
            decision.DecisionNodes.Add(upgradePlant);

            Assert.Throws<DecisionException>(() => decision.Evaluate());
        }
    }
}
