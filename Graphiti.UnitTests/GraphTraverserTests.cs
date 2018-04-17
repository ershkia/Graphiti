using System;
using System.Linq;
using Graphiti.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Graphiti.UnitTests
{
    [TestClass]
    public class GraphTraverserTest
    {
        [TestMethod]
        public void TraverseByStops_DirectRoute()
        {
            Graph graph = new Graph();
            graph.AddNode("A");
            graph.AddNode("B");

            GraphEdge edge = new GraphEdge("A", "B", 1);
            graph.AddEdge(edge);

            GraphTraverser traverser = new GraphTraverser(graph);
            var result = traverser.TraverseByStops("A", "B", 10);

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(new GraphRoute(new[] { edge }), result.First());
        }

        [TestMethod]
        public void TraverseByStops_IndirectRoute()
        {
            Graph graph = new Graph();
            graph.AddNode("A");
            graph.AddNode("B");
            graph.AddNode("C");

            GraphEdge ab_edge = new GraphEdge("A", "B", 1);
            GraphEdge bc_edge = new GraphEdge("B", "C", 2);
            graph.AddEdge(ab_edge);
            graph.AddEdge(bc_edge);

            GraphTraverser traverser = new GraphTraverser(graph);
            var result = traverser.TraverseByStops("A", "C", 10);

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(new GraphRoute(new[] { ab_edge, bc_edge }), result.First());
        }

        public void TraverseByStops_CyclicRoute()
        {
            Graph graph = new Graph();
            graph.AddNode("A");
            graph.AddNode("B");
            GraphEdge ab_edge = new GraphEdge("A", "B", 1);
            GraphEdge ba_edge = new GraphEdge("B", "A", 2);
            graph.AddEdge(ab_edge);
            graph.AddEdge(ba_edge);

            GraphTraverser traverser = new GraphTraverser(graph);
            var result = traverser.TraverseByStops("A", "A", 2);

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(new GraphRoute(new[] { ab_edge, ba_edge }), result.First());
        }

        [TestMethod]
        public void TraverseByStops_RespectMaxStops()
        {
            Graph graph = new Graph();
            graph.AddNode("A");
            graph.AddNode("B");

            GraphEdge ab_edge = new GraphEdge("A", "B", 1);
            GraphEdge ba_edge = new GraphEdge("B", "A", 2);
            graph.AddEdge(ab_edge);
            graph.AddEdge(ba_edge);

            GraphTraverser traverser = new GraphTraverser(graph);
            var result = traverser.TraverseByStops("A", "A", 7);

            Assert.AreEqual(3, result.Count());
            var shortRoute = result.Where(x => x.GetEdgeCount() == 2).FirstOrDefault();
            var mediumRoute = result.Where(x => x.GetEdgeCount() == 4).FirstOrDefault();
            var longRoute = result.Where(x => x.GetEdgeCount() == 6).FirstOrDefault();

            Assert.AreEqual(new GraphRoute(new[] { ab_edge, ba_edge }), shortRoute);
            Assert.AreEqual(new GraphRoute(new[] { ab_edge, ba_edge, ab_edge, ba_edge }), mediumRoute);
            Assert.AreEqual(new GraphRoute(new[] { ab_edge, ba_edge, ab_edge, ba_edge, ab_edge, ba_edge }), longRoute);
        }

        [TestMethod]
        public void TraverseByStops_MutipleRoutes()
        {
            Graph graph = new Graph();
            graph.AddNode("A");
            graph.AddNode("B");
            graph.AddNode("C");
            graph.AddNode("D");

            GraphEdge ab_edge = new GraphEdge("A", "B", 1);
            GraphEdge bc_edge = new GraphEdge("B", "C", 2);
            GraphEdge cd_edge = new GraphEdge("C", "D", 3);
            GraphEdge ad_edge = new GraphEdge("A", "D", 4);
            graph.AddEdge(ab_edge);
            graph.AddEdge(bc_edge);
            graph.AddEdge(cd_edge);
            graph.AddEdge(ad_edge);

            GraphTraverser traverser = new GraphTraverser(graph);
            var result = traverser.TraverseByStops("A", "D", 3);

            Assert.AreEqual(2, result.Count());

            var shortRoute = result.FirstOrDefault(r => r.GetEdgeCount() == 1);
            Assert.AreEqual(new GraphRoute(new[] { ad_edge }), shortRoute);

            var longRoute = result.FirstOrDefault(r => r.GetEdgeCount() == 3);
            Assert.AreEqual(new GraphRoute(new[] { ab_edge, bc_edge, cd_edge }), longRoute);
        }

        [TestMethod]
        public void TraverseByStops_NoRoute()
        {
            Graph graph = new Graph();
            graph.AddNode("A");
            graph.AddNode("B");
            graph.AddNode("C");

            GraphEdge ab_edge = new GraphEdge("A", "B", 1);
            GraphEdge ca_edge = new GraphEdge("C", "A", 2);
            graph.AddEdge(ab_edge);
            graph.AddEdge(ca_edge);

            GraphTraverser traverser = new GraphTraverser(graph);
            var result = traverser.TraverseByStops("A", "C", 3);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void TraverseByWeight_RespectMaxWeight()
        {
            Graph graph = new Graph();
            graph.AddNode("A");
            graph.AddNode("B");
            graph.AddNode("C");

            GraphEdge ab_edge = new GraphEdge("A", "B", 1);
            GraphEdge ba_edge = new GraphEdge("B", "A", 1);
            GraphEdge bc_edge = new GraphEdge("B", "C", 3);
            graph.AddEdge(ab_edge);
            graph.AddEdge(ba_edge);
            graph.AddEdge(bc_edge);

            GraphTraverser traverser = new GraphTraverser(graph);
            var result = traverser.TraverseByWeight("A", "C", 9);

            Assert.AreEqual(3, result.Count());
        }

    }
}