using System;

namespace Common.ProjectManagement.DecisionAnalysis
{
    /// <summary>
    /// Reprensents a chance node in the decision tree.
    /// </summary>
    public class ChanceNode
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ChanceNode"/> class.
        /// </summary>
        /// <param name="title">The title for this event.</param>
        /// <param name="parentCost">The cost of the parent node, a <see cref="DecisionNode"/>.</param>
        /// <param name="likelihood">The probability that this event will occur (a positive value less than or equal to 1.0.</param>
        /// <param name="expectedMonetaryValue">The expected monetary value of this event (the expected
        /// financial gain if this event occurs).</param>
        public ChanceNode(string title,
            decimal parentCost,
            decimal likelihood,
            decimal expectedMonetaryValue)
        {
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentNullException(nameof(title)) : title;
            ParentCost = parentCost;
            Likelihood = likelihood > 0M && likelihood <= 1M ? likelihood : throw new ArgumentException("Likelihood must be a positive decimal less than or equal to 1.");
            ExpectedMonetaryValue = expectedMonetaryValue;
        }

        /// <summary>
        /// Gets the title of this event.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the probability that this event will occur.
        /// </summary>
        public decimal Likelihood { get; }

        /// <summary>
        /// Gets the expected monetary value (financial gain) if this event occurs.
        /// </summary>
        public decimal ExpectedMonetaryValue { get; }

        /// <summary>
        /// Gets the cost of the <see cref="DecisionNode"/> that is the parent of this object.
        /// </summary>
        public decimal ParentCost { get; }

        /// <summary>
        /// Gets the computed value that is calculated in the <see cref="Evaluate"/> function.
        /// </summary>
        public decimal ComputedValue { get; private set; }

        /// <summary>
        /// Calculate the <see cref="ComputedValue"/> for this event.
        /// </summary>
        public void Evaluate()
        {
            ComputedValue = ExpectedMonetaryValue - ParentCost;
        }
    }
}
