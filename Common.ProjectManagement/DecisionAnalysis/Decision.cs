using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common.ProjectManagement.DecisionAnalysis
{
    /// <summary>
    /// Represents a decision that needs to be made.
    /// </summary>
    public class Decision 
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Decision"/> class.
        /// </summary>
        /// <param name="title">The title of the decision.</param>
        /// <param name="description">The decision's description.</param>
        public Decision(string title,
            string description)
        {
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentNullException(nameof(title)) : title;
            Description = description;
            DecisionNodes = new Collection<DecisionNode>();
            SelectedNodes = new Collection<DecisionNode>();
            Summary = new Collection<string>();
        }

        /// <summary>
        /// Gets a collection of <see cref="DecisionNode"/> objects.
        /// </summary>
        public Collection<DecisionNode> DecisionNodes { get; }

        /// <summary>
        /// Gets the title of this decision.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the description of this decision.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets a collection of summary information.
        /// </summary>
        public Collection<string> Summary { get; }

        /// <summary>
        /// Gets a collection of <see cref="DecisionNode"/> objects that were selected as 
        /// the best decision(s). This collection is populated by the <see cref="Evaluate"/> function.
        /// Usually, there will only be one item in this collection after the <see cref="Evaluate"/> function
        /// is executed, although it is possible for multiple <see cref="DecisionNode"/> objects to tie 
        /// for first place.
        /// </summary>
        public Collection<DecisionNode> SelectedNodes { get; }

        /// <summary>
        /// Evaluate the underlying <see cref="DecisionNode"/> objects and
        /// </summary>
        public void Evaluate()
        {
            if (!DecisionNodes.Any())
            {
                throw new DecisionException("No decision nodes are available to evaluate.");
            }

            foreach (DecisionNode node in DecisionNodes)
            {
                node.Evaluate();
            }

            Summary.Clear();

            decimal maxValue = DecisionNodes.Max(s => s.ComputedValue);

            foreach (var node in DecisionNodes.Where(m => m.ComputedValue == maxValue))
            {
                SelectedNodes.Add(node);
            }

            if (SelectedNodes.Count() < 1)
            {
                throw new DecisionException("Logic error: no node was selected.");
            }
            else if (SelectedNodes.Count() == 1)
            {
                DecisionNode winningNode = SelectedNodes.First();
                Summary.Add($"{winningNode.Title} is the best decision with a computed value of {winningNode.ComputedValue.ToString("C")}");
            }
            else
            {
                Summary.Add($"There were {SelectedNodes.Count()} nodes that tied for first place.");
                foreach (DecisionNode node in SelectedNodes)
                {
                    Summary.Add($"{node.Title} has a computed value of {node.ComputedValue.ToString("C")}");
                }
            }
        }
    }
}
