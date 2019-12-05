using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common.ProjectManagement.DecisionAnalysis
{
    /// <summary>
    /// Represents a decision node.
    /// </summary>
    public class DecisionNode
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DecisionNode"/> class.
        /// </summary>
        /// <param name="title">The title of this code.</param>
        /// <param name="cost">The dollar cost of this decision.</param>
        public DecisionNode(string title,
            decimal cost)
        {
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentNullException(nameof(title)) : title;
            Cost = cost;
            ChanceNodes = new Collection<ChanceNode>();
        }

        /// <summary>
        /// Gets the title of this decision.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the dollar cost of this decision.
        /// </summary>
        public decimal Cost { get; }

        /// <summary>
        /// Gets the computed value of this decision. This value is set by the <see cref="Evaluate"/> function.
        /// </summary>
        public decimal ComputedValue { get; private set; }

        /// <summary>
        /// Gets the collection of chance nodes used to populate <see cref="ComputedValue"/> in the 
        /// <see cref="Evaluate"/> function.
        /// The <see cref="ChanceNode.Likelihood"/> values must equal 1.0 (100%) across this collection.
        /// </summary>
        public Collection<ChanceNode> ChanceNodes { get; }

        /// <summary>
        /// Evaluate the <see cref="ChanceNode"/> objects and set the <see cref="ComputedValue"/> for this
        /// decision node.
        /// </summary>
        /// <exception cref="DecisionException">Thrown if the <see cref="ChanceNodes"/> property is empty
        /// or if the sum of <see cref="ChanceNode.Likelihood"/> values does not equal 1.0 (100%).</exception>
        public void Evaluate()
        {
            if (!ChanceNodes.Any())
            {
                throw new DecisionException($"No chance nodes for {Title} are available to evaluate.");
            }

            foreach (ChanceNode chanceNode in ChanceNodes)
            {
                chanceNode.Evaluate();
            }

            decimal totalProbability = ChanceNodes.Sum(n => n.Likelihood);

            if (totalProbability != 1M)
            {
                throw new DecisionException($"The chance nodes for {Title} equal {(totalProbability * 100)}%, but must equal 100%.");
            }

            ComputedValue = 0M;
            for (int n = 0; n < ChanceNodes.Count; n++)
            {
                ComputedValue += (ChanceNodes[n].ComputedValue * ChanceNodes[n].Likelihood);
            }
        }
    }
}
