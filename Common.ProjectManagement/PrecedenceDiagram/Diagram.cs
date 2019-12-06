using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common.ProjectManagement.PrecedenceDiagram
{
    /// <summary>
    /// Represents a precedence diagram.
    /// </summary>
    public class Diagram
    {
        private readonly Activities activities;
        private readonly Activity start;
        private readonly Activity finish;
        private readonly List<ActivityRelationship> activityRelationships;

        /// <summary>
        /// Creates a new instance of the <see cref="Diagram"/> class.
        /// </summary>
        /// <param name="activities">A collection of activities to include in the diagram.</param>
        /// <param name="activityRelationships">A collection of activity relationships to include in the diagram.</param>
        public Diagram(IEnumerable<Activity> activities, IEnumerable<ActivityRelationship> activityRelationships)
        {
            this.activities = new Activities(activities);
            start = new Activity("Start", 0);
            finish = new Activity("Finish", 0);
            this.activities.Add(start);
            this.activities.Add(finish);

            this.activityRelationships = new List<ActivityRelationship>(activityRelationships);

            List<ActivityNode> nodes = new List<ActivityNode>();

            this.activities.ToList().ForEach(a => nodes.Add(new ActivityNode(a)));

            ActivityNodes = new ReadOnlyCollection<ActivityNode>(nodes);

            ConstructDiagram();
        }

        /// <summary>
        /// Gets a collection of the <see cref="ActivityRelationship"/> instances included in the diagram.
        /// </summary>
        public ReadOnlyCollection<ActivityRelationship> ActivityRelationships =>
            new ReadOnlyCollection<ActivityRelationship>(activityRelationships.ToList());

        /// <summary>
        /// Gets a collection of <see cref="ActivityNode"/> instances included in the diagram.
        /// </summary>
        public ReadOnlyCollection<ActivityNode> ActivityNodes { get; }

        private void ConstructDiagram()
        {
            IEnumerable<ActivityNode> startingNodes = ActivityNodes.Where(n =>
                n.Activity != start &&
                n.Activity != finish &&
                !ActivityRelationships.Select(r => r.Activity2).Contains(n.Activity));

            startingNodes.ToList().ForEach(n => MakeForwardPass(n, true));

            List<ActivityNode> finishingNodes = ActivityNodes.Where(n =>
                n.Activity != finish &&
                !ActivityRelationships.Select(r => r.Activity1).Contains(n.Activity)).ToList();

            finishingNodes.ToList().ForEach(n => activityRelationships.Add(
                new ActivityRelationship(n.Activity, finish, new ActivityRelationshipType())));


            // It's possible that some other node in the network has an early finish date greater than
            // the EF dates on the nodes directly connected to the finish node. In these cases, also tie these nodes
            // to the finish node.
            int maxKnownFinish = finishingNodes.Max(n => n.EarlyFinish);

            List<ActivityNode> otherNodes = ActivityNodes.Where(n => n.EarlyFinish > maxKnownFinish)
                .Except(finishingNodes).ToList();

            otherNodes.ToList().ForEach(n => activityRelationships.Add(
                new ActivityRelationship(n.Activity, finish, new ActivityRelationshipType())));

            List<Activity> finalActivities = activityRelationships.Where(r => r.Activity2 == finish).Select(r => r.Activity1).ToList();

            ActivityNodes.Where(n => finalActivities.Contains(n.Activity)).ToList()
                .ForEach(n => MakeBackwardPass(n, true));
        }

        private void MakeForwardPass(ActivityNode node, bool isStart = false)
        {
            if (isStart)
            {
                ActivityNode startNode = ActivityNodes.FirstOrDefault(n => n.Activity.Equals(start));
                activityRelationships.Add(new ActivityRelationship(start, node.Activity, new ActivityRelationshipType()));
                node.EarlyStart = startNode.EarlyFinish;
            }

            foreach (ActivityRelationship relationship in activityRelationships.Where(r => r.Activity1.Equals(node.Activity)))
            {
                int delay = relationship.ActivityRelationshipType.Lag - relationship.ActivityRelationshipType.Lead;

                ActivityNode nextNode = ActivityNodes.FirstOrDefault(n => n.Activity.Equals(relationship.Activity2));
                nextNode.EarlyStart = relationship.ActivityRelationshipType.LogicalRelationshipType switch
                {
                    LogicalRelationshipType.FinishToStart => Math.Max(nextNode.EarlyStart, node.EarlyFinish + delay),
                    LogicalRelationshipType.StartToStart => Math.Max(nextNode.EarlyStart, node.EarlyStart + delay),
                    LogicalRelationshipType.FinishToFinish => Math.Max(nextNode.EarlyStart, node.EarlyFinish - nextNode.Duration + delay),
                    LogicalRelationshipType.StartToFinish => Math.Max(nextNode.EarlyStart, node.EarlyStart - nextNode.Duration + delay),
                    _ => throw new Exception($"Unknown {nameof(LogicalRelationshipType)}: {relationship.ActivityRelationshipType.LogicalRelationshipType.ToString()}"),
                };
                MakeForwardPass(nextNode);
            }
        }

        private void MakeBackwardPass(ActivityNode node, bool isStart = false)
        {
            if (isStart)
            {
                ActivityNode finishNode = ActivityNodes.FirstOrDefault(n => n.Activity == finish);

                List<Activity> previousActivities = activityRelationships.Where(r => r.Activity2 == finish).Select(r => r.Activity1).ToList();

                finishNode.LateFinish = ActivityNodes.Where(n => previousActivities.Contains(n.Activity)).Max(m => m.EarlyFinish);
                finishNode.EarlyStart = finishNode.LateFinish;
                //activityRelationships.Add(new ActivityRelationship(node.Activity, finish, new ActivityRelationshipType()));
                node.LateFinish = finishNode.LateFinish;
            }

            foreach (ActivityRelationship relationship in activityRelationships.Where(r => r.Activity2.Equals(node.Activity)))
            {
                int delay = relationship.ActivityRelationshipType.Lag - relationship.ActivityRelationshipType.Lead;

                ActivityNode previousNode = ActivityNodes.FirstOrDefault(n => n.Activity.Equals(relationship.Activity1));

                if (previousNode.LateFinish == 0) { previousNode.LateFinish = int.MaxValue; }

                previousNode.LateFinish = relationship.ActivityRelationshipType.LogicalRelationshipType switch
                {
                    LogicalRelationshipType.FinishToStart => Math.Min(previousNode.LateFinish, node.LateStart - delay),
                    LogicalRelationshipType.StartToStart => Math.Min(previousNode.LateFinish, node.LateStart + previousNode.Duration - delay),
                    LogicalRelationshipType.FinishToFinish => Math.Min(previousNode.LateFinish, node.LateStart + node.Duration - delay),
                    LogicalRelationshipType.StartToFinish => Math.Min(previousNode.LateFinish, node.LateStart + previousNode.Duration + node.Duration - delay),
                    _ => throw new Exception($"Unknown {nameof(LogicalRelationshipType)}: {relationship.ActivityRelationshipType.LogicalRelationshipType.ToString()}"),
                };


                //previousNode.LateFinish = Math.Min(previousNode.LateFinish, node.LateStart - 1 - delay);

                MakeBackwardPass(previousNode);
            }
        }
    }
}
