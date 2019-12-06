using Common.ProjectManagement.PrecedenceDiagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Common.ProjectManagement.UnitTests
{
    public class PrecedenceDiagramTests
    {
        private readonly ITestOutputHelper testOutputHelper;
        public PrecedenceDiagramTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        #region Finish-to-Start
        [Fact]
        public void FinishToStart_Simple()
        {
            Activity a = new Activity("A", 5);
            Activity b = new Activity("B", 6);
            Activity c = new Activity("C", 3);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType());
            ActivityRelationship b2c = new ActivityRelationship(b, c, new ActivityRelationshipType());

            Activities activities = new Activities(new Activity[] { a, b, c });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, b2c
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(5, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(14, finish.EarlyFinish);
            Assert.Equal(14, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(5, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(5, aNode.LateFinish);

            Assert.Equal(5, bNode.EarlyStart);
            Assert.Equal(11, bNode.EarlyFinish);
            Assert.Equal(5, bNode.LateStart);
            Assert.Equal(11, bNode.LateFinish);

            Assert.Equal(11, cNode.EarlyStart);
            Assert.Equal(14, cNode.EarlyFinish);
            Assert.Equal(11, cNode.LateStart);
            Assert.Equal(14, cNode.LateFinish);
        }

        [Fact]
        public void FinishToStart_LagAndLead()
        {
            Activity a = new Activity("A", 5);
            Activity b = new Activity("B", 6);
            Activity c = new Activity("C", 3);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType(lag: 1));
            ActivityRelationship b2c = new ActivityRelationship(b, c, new ActivityRelationshipType(lead: 1));

            Activities activities = new Activities(new Activity[] { a, b, c });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, b2c
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(5, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(14, finish.EarlyFinish);
            Assert.Equal(14, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(5, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(5, aNode.LateFinish);

            Assert.Equal(6, bNode.EarlyStart);
            Assert.Equal(12, bNode.EarlyFinish);
            Assert.Equal(6, bNode.LateStart);
            Assert.Equal(12, bNode.LateFinish);

            Assert.Equal(11, cNode.EarlyStart);
            Assert.Equal(14, cNode.EarlyFinish);
            Assert.Equal(11, cNode.LateStart);
            Assert.Equal(14, cNode.LateFinish);
        }

        [Fact]
        public void FinishToStart_Parallel()
        {
            Activity a = new Activity("A", 5);
            Activity b = new Activity("B", 6);
            Activity c = new Activity("C", 3);
            Activity d = new Activity("D", 7);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType());
            ActivityRelationship a2c = new ActivityRelationship(a, c, new ActivityRelationshipType());
            ActivityRelationship b2d = new ActivityRelationship(b, d, new ActivityRelationshipType());
            ActivityRelationship c2d = new ActivityRelationship(c, d, new ActivityRelationshipType());

            Activities activities = new Activities(new Activity[] { a, b, c, d });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, a2c, b2d, c2d
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(6, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");
            ActivityNode dNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "D");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(18, finish.EarlyFinish);
            Assert.Equal(18, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(5, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(5, aNode.LateFinish);

            Assert.Equal(5, bNode.EarlyStart);
            Assert.Equal(11, bNode.EarlyFinish);
            Assert.Equal(5, bNode.LateStart);
            Assert.Equal(11, bNode.LateFinish);

            Assert.Equal(5, cNode.EarlyStart);
            Assert.Equal(8, cNode.EarlyFinish);
            Assert.Equal(8, cNode.LateStart);
            Assert.Equal(11, cNode.LateFinish);

            Assert.Equal(11, dNode.EarlyStart);
            Assert.Equal(18, dNode.EarlyFinish);
            Assert.Equal(11, dNode.LateStart);
            Assert.Equal(18, dNode.LateFinish);
        }

        [Fact]
        public void FinishToStart_Parallel_LagAndLead()
        {
            Activity a = new Activity("A", 5);
            Activity b = new Activity("B", 6);
            Activity c = new Activity("C", 3);
            Activity d = new Activity("D", 7);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType(lag: 1));
            ActivityRelationship a2c = new ActivityRelationship(a, c, new ActivityRelationshipType(lead: 1));
            ActivityRelationship b2d = new ActivityRelationship(b, d, new ActivityRelationshipType(lead: 2));
            ActivityRelationship c2d = new ActivityRelationship(c, d, new ActivityRelationshipType(lag: 2));

            Activities activities = new Activities(new Activity[] { a, b, c, d });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, a2c, b2d, c2d
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(6, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");
            ActivityNode dNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "D");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(17, finish.EarlyFinish);
            Assert.Equal(17, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(5, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(5, aNode.LateFinish);

            Assert.Equal(6, bNode.EarlyStart);
            Assert.Equal(12, bNode.EarlyFinish);
            Assert.Equal(6, bNode.LateStart);
            Assert.Equal(12, bNode.LateFinish);

            Assert.Equal(4, cNode.EarlyStart);
            Assert.Equal(7, cNode.EarlyFinish);
            Assert.Equal(5, cNode.LateStart);
            Assert.Equal(8, cNode.LateFinish);

            Assert.Equal(10, dNode.EarlyStart);
            Assert.Equal(17, dNode.EarlyFinish);
            Assert.Equal(10, dNode.LateStart);
            Assert.Equal(17, dNode.LateFinish);
        }
        #endregion

        #region Finish-to-Finish
        [Fact]
        public void FinishToFinish_Simple()
        {
            Activity a = new Activity("A", 5);
            Activity b = new Activity("B", 3);
            Activity c = new Activity("C", 4);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType(LogicalRelationshipType.FinishToFinish));
            ActivityRelationship b2c = new ActivityRelationship(b, c, new ActivityRelationshipType(LogicalRelationshipType.FinishToFinish));

            Activities activities = new Activities(new Activity[] { a, b, c });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, b2c
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(5, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(5, finish.EarlyFinish);
            Assert.Equal(5, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(5, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(5, aNode.LateFinish);

            Assert.Equal(2, bNode.EarlyStart);
            Assert.Equal(5, bNode.EarlyFinish);
            Assert.Equal(2, bNode.LateStart);
            Assert.Equal(5, bNode.LateFinish);

            Assert.Equal(1, cNode.EarlyStart);
            Assert.Equal(5, cNode.EarlyFinish);
            Assert.Equal(1, cNode.LateStart);
            Assert.Equal(5, cNode.LateFinish);
        }

        [Fact]
        public void FinishToFinish_LagAndLead()
        {
            Activity a = new Activity("A", 5);
            Activity b = new Activity("B", 6);
            Activity c = new Activity("C", 3);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType(LogicalRelationshipType.FinishToFinish, lag: 1));
            ActivityRelationship b2c = new ActivityRelationship(b, c, new ActivityRelationshipType(LogicalRelationshipType.FinishToFinish, lead: 1));

            Activities activities = new Activities(new Activity[] { a, b, c });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, b2c
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(5, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(6, finish.EarlyFinish);
            Assert.Equal(6, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(5, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(5, aNode.LateFinish);

            Assert.Equal(0, bNode.EarlyStart);
            Assert.Equal(6, bNode.EarlyFinish);
            Assert.Equal(0, bNode.LateStart);
            Assert.Equal(6, bNode.LateFinish);

            Assert.Equal(2, cNode.EarlyStart);
            Assert.Equal(5, cNode.EarlyFinish);
            Assert.Equal(3, cNode.LateStart);
            Assert.Equal(6, cNode.LateFinish);
        }

        [Fact]
        public void FinishToFinish_Parallel()
        {
            Activity a = new Activity("A", 7);
            Activity b = new Activity("B", 6);
            Activity c = new Activity("C", 3);
            Activity d = new Activity("D", 1);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType(LogicalRelationshipType.FinishToFinish));
            ActivityRelationship a2c = new ActivityRelationship(a, c, new ActivityRelationshipType(LogicalRelationshipType.FinishToFinish));
            ActivityRelationship b2d = new ActivityRelationship(b, d, new ActivityRelationshipType(LogicalRelationshipType.FinishToFinish));
            ActivityRelationship c2d = new ActivityRelationship(c, d, new ActivityRelationshipType(LogicalRelationshipType.FinishToFinish));

            Activities activities = new Activities(new Activity[] { a, b, c, d });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, a2c, b2d, c2d
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(6, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");
            ActivityNode dNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "D");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(7, finish.EarlyFinish);
            Assert.Equal(7, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(7, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(7, aNode.LateFinish);

            Assert.Equal(1, bNode.EarlyStart);
            Assert.Equal(7, bNode.EarlyFinish);
            Assert.Equal(1, bNode.LateStart);
            Assert.Equal(7, bNode.LateFinish);

            Assert.Equal(4, cNode.EarlyStart);
            Assert.Equal(7, cNode.EarlyFinish);
            Assert.Equal(4, cNode.LateStart);
            Assert.Equal(7, cNode.LateFinish);

            Assert.Equal(6, dNode.EarlyStart);
            Assert.Equal(7, dNode.EarlyFinish);
            Assert.Equal(6, dNode.LateStart);
            Assert.Equal(7, dNode.LateFinish);
        }

        [Fact]
        public void FinishToFinish_Parallel_LagAndLead()
        {
            Activity a = new Activity("A", 5);
            Activity b = new Activity("B", 6);
            Activity c = new Activity("C", 3);
            Activity d = new Activity("D", 4);
            Activity e = new Activity("E", 5);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType());
            ActivityRelationship b2c = new ActivityRelationship(b, c, new ActivityRelationshipType(LogicalRelationshipType.FinishToFinish, lag: 1));
            ActivityRelationship b2d = new ActivityRelationship(b, d, new ActivityRelationshipType(LogicalRelationshipType.FinishToFinish, lead: 1));
            ActivityRelationship c2e = new ActivityRelationship(c, e, new ActivityRelationshipType());
            ActivityRelationship d2e = new ActivityRelationship(d, e, new ActivityRelationshipType());

            Activities activities = new Activities(new Activity[] { a, b, c, d, e });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, b2c, b2d, c2e, d2e
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(7, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");
            ActivityNode dNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "D");
            ActivityNode eNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "E");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(17, finish.EarlyFinish);
            Assert.Equal(17, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(5, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(5, aNode.LateFinish);

            Assert.Equal(5, bNode.EarlyStart);
            Assert.Equal(11, bNode.EarlyFinish);
            Assert.Equal(5, bNode.LateStart);
            Assert.Equal(11, bNode.LateFinish);

            Assert.Equal(9, cNode.EarlyStart);
            Assert.Equal(12, cNode.EarlyFinish);
            Assert.Equal(9, cNode.LateStart);
            Assert.Equal(12, cNode.LateFinish);

            Assert.Equal(6, dNode.EarlyStart);
            Assert.Equal(10, dNode.EarlyFinish);
            Assert.Equal(8, dNode.LateStart);
            Assert.Equal(12, dNode.LateFinish);

            Assert.Equal(12, eNode.EarlyStart);
            Assert.Equal(17, eNode.EarlyFinish);
            Assert.Equal(12, eNode.LateStart);
            Assert.Equal(17, eNode.LateFinish);
        }
        #endregion

        #region Start-to-Finish
        [Fact]
        public void StartToFinish_Simple()
        {
            Activity a = new Activity("A", 10);
            Activity b = new Activity("B", 7);
            Activity c = new Activity("C", 1);
            Activity d = new Activity("D", 10);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType());
            ActivityRelationship b2c = new ActivityRelationship(b, c, new ActivityRelationshipType(LogicalRelationshipType.StartToFinish));
            ActivityRelationship c2d = new ActivityRelationship(c, d, new ActivityRelationshipType());

            Activities activities = new Activities(new Activity[] { a, b, c, d });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, b2c, c2d
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(6, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");
            ActivityNode dNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "D");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(20, finish.EarlyFinish);
            Assert.Equal(20, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(10, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(10, aNode.LateFinish);

            Assert.Equal(10, bNode.EarlyStart);
            Assert.Equal(17, bNode.EarlyFinish);
            Assert.Equal(10, bNode.LateStart);
            Assert.Equal(17, bNode.LateFinish);

            Assert.Equal(9, cNode.EarlyStart);
            Assert.Equal(10, cNode.EarlyFinish);
            Assert.Equal(9, cNode.LateStart);
            Assert.Equal(10, cNode.LateFinish);

            Assert.Equal(10, dNode.EarlyStart);
            Assert.Equal(20, dNode.EarlyFinish);
            Assert.Equal(10, dNode.LateStart);
            Assert.Equal(20, dNode.LateFinish);
        }

        [Fact]
        public void StartToFinish_LagAndLead()
        {
            Activity a = new Activity("A", 10);
            Activity b = new Activity("B", 3);
            Activity c = new Activity("C", 4);
            Activity d = new Activity("D", 3);
            Activity e = new Activity("E", 10);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType());
            ActivityRelationship b2c = new ActivityRelationship(b, c, new ActivityRelationshipType(LogicalRelationshipType.StartToFinish, lag: 1));
            ActivityRelationship c2d = new ActivityRelationship(c, d, new ActivityRelationshipType(LogicalRelationshipType.StartToFinish, lead: 1));
            ActivityRelationship d2e = new ActivityRelationship(d, e, new ActivityRelationshipType());

            Activities activities = new Activities(new Activity[] { a, b, c, d, e });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
             {
                a2b, b2c, c2d, d2e
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(7, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");
            ActivityNode dNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "D");
            ActivityNode eNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "E");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(16, finish.EarlyFinish);
            Assert.Equal(16, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(10, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(10, aNode.LateFinish);

            Assert.Equal(10, bNode.EarlyStart);
            Assert.Equal(13, bNode.EarlyFinish);
            Assert.Equal(10, bNode.LateStart);
            Assert.Equal(13, bNode.LateFinish);

            Assert.Equal(7, cNode.EarlyStart);
            Assert.Equal(11, cNode.EarlyFinish);
            Assert.Equal(7, cNode.LateStart);
            Assert.Equal(11, cNode.LateFinish);

            Assert.Equal(3, dNode.EarlyStart);
            Assert.Equal(6, dNode.EarlyFinish);
            Assert.Equal(3, dNode.LateStart);
            Assert.Equal(6, dNode.LateFinish);

            Assert.Equal(6, eNode.EarlyStart);
            Assert.Equal(16, eNode.EarlyFinish);
            Assert.Equal(6, eNode.LateStart);
            Assert.Equal(16, eNode.LateFinish);
        }

        [Fact]
        public void StartToFinish_Parallel()
        {
            Activity a = new Activity("A", 10);
            Activity b = new Activity("B", 6);
            Activity c = new Activity("C", 3);
            Activity d = new Activity("D", 2);
            Activity e = new Activity("E", 6);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType());
            ActivityRelationship b2c = new ActivityRelationship(b, c, new ActivityRelationshipType(LogicalRelationshipType.StartToFinish));
            ActivityRelationship b2d = new ActivityRelationship(b, d, new ActivityRelationshipType(LogicalRelationshipType.StartToFinish));
            ActivityRelationship c2e = new ActivityRelationship(c, e, new ActivityRelationshipType());
            ActivityRelationship d2e = new ActivityRelationship(d, e, new ActivityRelationshipType());

            Activities activities = new Activities(new Activity[] { a, b, c, d, e });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, b2c, b2d, c2e, d2e
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(7, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");
            ActivityNode dNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "D");
            ActivityNode eNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "E");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(16, finish.EarlyFinish);
            Assert.Equal(16, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(10, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(10, aNode.LateFinish);

            Assert.Equal(10, bNode.EarlyStart);
            Assert.Equal(16, bNode.EarlyFinish);
            Assert.Equal(10, bNode.LateStart);
            Assert.Equal(16, bNode.LateFinish);

            Assert.Equal(7, cNode.EarlyStart);
            Assert.Equal(10, cNode.EarlyFinish);
            Assert.Equal(7, cNode.LateStart);
            Assert.Equal(10, cNode.LateFinish);

            Assert.Equal(8, dNode.EarlyStart);
            Assert.Equal(10, dNode.EarlyFinish);
            Assert.Equal(8, dNode.LateStart);
            Assert.Equal(10, dNode.LateFinish);

            Assert.Equal(10, eNode.EarlyStart);
            Assert.Equal(16, eNode.EarlyFinish);
            Assert.Equal(10, eNode.LateStart);
            Assert.Equal(16, eNode.LateFinish);
        }

        [Fact]
        public void StartToFinish_Parallel_LagAndLead()
        {
            Activity a = new Activity("A", 10);
            Activity b = new Activity("B", 3);
            Activity c = new Activity("C", 4);
            Activity d = new Activity("D", 3);
            Activity e = new Activity("E", 2);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType());
            ActivityRelationship b2c = new ActivityRelationship(b, c, new ActivityRelationshipType(LogicalRelationshipType.StartToFinish, lead: 1));
            ActivityRelationship b2d = new ActivityRelationship(b, d, new ActivityRelationshipType(LogicalRelationshipType.StartToFinish, lag: 1));
            ActivityRelationship c2e = new ActivityRelationship(c, e, new ActivityRelationshipType());
            ActivityRelationship d2e = new ActivityRelationship(d, e, new ActivityRelationshipType());

            Activities activities = new Activities(new Activity[] { a, b, c, d, e });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
             {
                a2b, b2c, b2d, c2e, d2e
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(7, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");
            ActivityNode dNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "D");
            ActivityNode eNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "E");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(13, finish.EarlyFinish);
            Assert.Equal(13, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(10, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(10, aNode.LateFinish);

            Assert.Equal(10, bNode.EarlyStart);
            Assert.Equal(13, bNode.EarlyFinish);
            Assert.Equal(10, bNode.LateStart);
            Assert.Equal(13, bNode.LateFinish);

            Assert.Equal(5, cNode.EarlyStart);
            Assert.Equal(9, cNode.EarlyFinish);
            Assert.Equal(7, cNode.LateStart);
            Assert.Equal(11, cNode.LateFinish);

            Assert.Equal(8, dNode.EarlyStart);
            Assert.Equal(11, dNode.EarlyFinish);
            Assert.Equal(8, dNode.LateStart);
            Assert.Equal(11, dNode.LateFinish);

            Assert.Equal(11, eNode.EarlyStart);
            Assert.Equal(13, eNode.EarlyFinish);
            Assert.Equal(11, eNode.LateStart);
            Assert.Equal(13, eNode.LateFinish);
        }
        #endregion

        #region Start-toStart
        [Fact]
        public void StartToStart_Simple()
        {
            Activity a = new Activity("A", 5);
            Activity b = new Activity("B", 3);
            Activity c = new Activity("C", 3);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType());
            ActivityRelationship b2c = new ActivityRelationship(b, c, new ActivityRelationshipType(LogicalRelationshipType.StartToStart));

            Activities activities = new Activities(new Activity[] { a, b, c });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, b2c
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(5, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(8, finish.EarlyFinish);
            Assert.Equal(8, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(5, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(5, aNode.LateFinish);

            Assert.Equal(5, bNode.EarlyStart);
            Assert.Equal(8, bNode.EarlyFinish);
            Assert.Equal(5, bNode.LateStart);
            Assert.Equal(8, bNode.LateFinish);

            Assert.Equal(5, cNode.EarlyStart);
            Assert.Equal(8, cNode.EarlyFinish);
            Assert.Equal(5, cNode.LateStart);
            Assert.Equal(8, cNode.LateFinish);
        }

        [Fact]
        public void StartToStart_LagAndLead()
        {
            Activity a = new Activity("A", 5);
            Activity b = new Activity("B", 6);
            Activity c = new Activity("C", 10);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType(LogicalRelationshipType.StartToStart, lag: 1));
            ActivityRelationship b2c = new ActivityRelationship(b, c, new ActivityRelationshipType(LogicalRelationshipType.StartToStart, lead: 1));

            Activities activities = new Activities(new Activity[] { a, b, c });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, b2c
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(5, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(10, finish.EarlyFinish);
            Assert.Equal(10, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(5, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(5, aNode.LateFinish);

            Assert.Equal(1, bNode.EarlyStart);
            Assert.Equal(7, bNode.EarlyFinish);
            Assert.Equal(1, bNode.LateStart);
            Assert.Equal(7, bNode.LateFinish);

            Assert.Equal(0, cNode.EarlyStart);
            Assert.Equal(10, cNode.EarlyFinish);
            Assert.Equal(0, cNode.LateStart);
            Assert.Equal(10, cNode.LateFinish);
        }

        [Fact]
        public void StartToStart_Parallel()
        {
            Activity a = new Activity("A", 5);
            Activity b = new Activity("B", 6);
            Activity c = new Activity("C", 3);
            Activity d = new Activity("D", 7);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType(LogicalRelationshipType.StartToStart));
            ActivityRelationship a2c = new ActivityRelationship(a, c, new ActivityRelationshipType(LogicalRelationshipType.StartToStart));
            ActivityRelationship b2d = new ActivityRelationship(b, d, new ActivityRelationshipType());
            ActivityRelationship c2d = new ActivityRelationship(c, d, new ActivityRelationshipType());

            Activities activities = new Activities(new Activity[] { a, b, c, d });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, a2c, b2d, c2d
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(6, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");
            ActivityNode dNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "D");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(13, finish.EarlyFinish);
            Assert.Equal(13, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(5, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(5, aNode.LateFinish);

            Assert.Equal(0, bNode.EarlyStart);
            Assert.Equal(6, bNode.EarlyFinish);
            Assert.Equal(0, bNode.LateStart);
            Assert.Equal(6, bNode.LateFinish);

            Assert.Equal(0, cNode.EarlyStart);
            Assert.Equal(3, cNode.EarlyFinish);
            Assert.Equal(3, cNode.LateStart);
            Assert.Equal(6, cNode.LateFinish);

            Assert.Equal(6, dNode.EarlyStart);
            Assert.Equal(13, dNode.EarlyFinish);
            Assert.Equal(6, dNode.LateStart);
            Assert.Equal(13, dNode.LateFinish);
        }

        [Fact]
        public void StartToStart_Parallel_LagAndLead()
        {
            Activity a = new Activity("A", 5);
            Activity b = new Activity("B", 6);
            Activity c = new Activity("C", 3);
            Activity d = new Activity("D", 7);

            ActivityRelationship a2b = new ActivityRelationship(a, b, new ActivityRelationshipType(LogicalRelationshipType.StartToStart, lag: 1));
            ActivityRelationship a2c = new ActivityRelationship(a, c, new ActivityRelationshipType(LogicalRelationshipType.StartToStart, lag: 2));
            ActivityRelationship b2d = new ActivityRelationship(b, d, new ActivityRelationshipType(LogicalRelationshipType.StartToStart, lead: 2));
            ActivityRelationship c2d = new ActivityRelationship(c, d, new ActivityRelationshipType(LogicalRelationshipType.StartToStart, lead: 2));

            Activities activities = new Activities(new Activity[] { a, b, c, d });
            IEnumerable<ActivityRelationship> relationships = new List<ActivityRelationship>()
            {
                a2b, a2c, b2d, c2d
            };

            Diagram diagram = new Diagram(activities, relationships);

            Assert.Equal(6, diagram.ActivityNodes.Count);

            ActivityNode start = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Start");
            ActivityNode finish = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "Finish");
            ActivityNode aNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "A");
            ActivityNode bNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "B");
            ActivityNode cNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "C");
            ActivityNode dNode = diagram.ActivityNodes.FirstOrDefault(n => n.Activity.Name == "D");

            Assert.Equal(0, start.EarlyFinish);
            Assert.Equal(0, start.LateFinish);

            Assert.Equal(7, finish.EarlyFinish);
            Assert.Equal(7, finish.LateFinish);

            Assert.Equal(0, aNode.EarlyStart);
            Assert.Equal(5, aNode.EarlyFinish);
            Assert.Equal(0, aNode.LateStart);
            Assert.Equal(5, aNode.LateFinish);

            Assert.Equal(1, bNode.EarlyStart);
            Assert.Equal(7, bNode.EarlyFinish);
            Assert.Equal(2, bNode.LateStart);
            Assert.Equal(8, bNode.LateFinish);

            Assert.Equal(2, cNode.EarlyStart);
            Assert.Equal(5, cNode.EarlyFinish);
            Assert.Equal(2, cNode.LateStart);
            Assert.Equal(5, cNode.LateFinish);

            Assert.Equal(0, dNode.EarlyStart);
            Assert.Equal(7, dNode.EarlyFinish);
            Assert.Equal(0, dNode.LateStart);
            Assert.Equal(7, dNode.LateFinish);
        }
        #endregion
    }
}

