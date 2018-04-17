using System;
using System.Linq;
using Graphiti.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Graphiti.UnitTests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void AddNode()
        {
            Graph graph = new Graph();

            graph.AddNode("A");
            var nodes = graph.GetNodes();

            Assert.IsNotNull(nodes);
            Assert.AreEqual(1, nodes.Count);
            Assert.AreSame("A", nodes.First());
        }

        [TestMethod]
        public void AddNode_DuplicateCallsAllowed()
        {
            Graph graph = new Graph();

            graph.AddNode("A");
            graph.AddNode("A");
            var nodes = graph.GetNodes();

            Assert.IsNotNull(nodes);
            Assert.AreEqual(1, nodes.Count);
            Assert.AreSame("A", nodes.First());
        }

        [TestMethod]
        public void AddEdge()
        {
            Graph graph = new Graph();

            graph.AddNode("A");
            graph.AddNode("B");
            graph.AddEdge("A", "B", 300);

            var neighbors = graph.GetNeighbors("A").ToList();

            Assert.IsNotNull(neighbors);
            Assert.AreEqual(1, neighbors.Count);
            Assert.AreEqual("A", neighbors[0].FromNode);
            Assert.AreEqual("B", neighbors[0].ToNode);
            Assert.AreEqual(300, neighbors[0].Weight);
        }

        [TestMethod]
        public void AddEdge_DuplicateCalls_Throw()
        {
            Graph graph = new Graph();

            graph.AddNode("A");
            graph.AddNode("B");
            graph.AddEdge("A", "B", 300);

            Assert.ThrowsException<Exception>(() => graph.AddEdge("A", "B", 300));
        }

        [TestMethod]
        public void AddEdge_MissingFromNode_Throw(){
            Graph graph = new Graph();

            graph.AddNode("B");

            Assert.ThrowsException<Exception>(() => graph.AddEdge("A", "B", 300));
        }

        [TestMethod]
        public void AddEdge_MissingToNode_Throw(){
            Graph graph = new Graph();

            graph.AddNode("A");

            Assert.ThrowsException<Exception>(() => graph.AddEdge("A", "B", 300));
        }

        //TODO Tests for other public methods.
    }
}
