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
            Assert.AreSame("A", nodes[0]);
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

            Assert.AreEqual(0, graph.GetNeighbors("B").Count);
        }
    }
}
