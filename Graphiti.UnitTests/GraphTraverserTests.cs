using System;
using System.Linq;
using Graphiti.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Graphiti.UnitTests
{
    [TestClass]
    public class GraphPathFinderTests
    {
        [TestMethod]
        public void DirectRoute()
        {
            Graph graph = new Graph();
            graph.AddNode("A");
            graph.AddNode("B");
            GraphEdge edge = new GraphEdge("A", "B", 1);
            graph.AddEdge(edge);

            GraphTraverser traverser = new GraphTraverser(graph);
            var result = traverser.Traverse("A", "B");

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(new GraphRoute(new[] { edge }), result.First());
        }

        [TestMethod]
        public void IndirectRoute()
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
            var result = traverser.Traverse("A", "C");

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(new GraphRoute(new[] { ab_edge, bc_edge }), result.First());
        }

        public void CyclicRoute()
        {
            Graph graph = new Graph();
            graph.AddNode("A");
            graph.AddNode("B");
            GraphEdge ab_edge = new GraphEdge("A", "B", 1);
            GraphEdge ba_edge = new GraphEdge("B", "A", 2);
            graph.AddEdge(ab_edge);
            graph.AddEdge(ba_edge);

            GraphTraverser traverser = new GraphTraverser(graph);
            var result = traverser.Traverse("A", "A");

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(new GraphRoute(new[] { ab_edge, ba_edge }), result.First());
        }

        [TestMethod]
        public void MutipleRoutes()
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
            var result = traverser.Traverse("A", "D");

            Assert.AreEqual(2, result.Count());

            var shortRoute = result.FirstOrDefault(r => r.GetEdgeCount() == 1);
            Assert.AreEqual(new GraphRoute(new[] { ad_edge }), shortRoute);

            var longRoute = result.FirstOrDefault(r => r.GetEdgeCount() == 3);
            Assert.AreEqual(new GraphRoute(new[] { ab_edge, bc_edge, cd_edge }), longRoute);
        }

        [TestMethod]
        public void NoRoute()
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
            var result = traverser.Traverse("A", "C");

            Assert.AreEqual(0, result.Count());
        }
    }
}