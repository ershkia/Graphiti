using System;
using System.Linq;
using Graphiti.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Graphiti.UnitTests
{
    [TestClass]
    public class GraphLoaderTests
    {
        [TestMethod]
        public void Load()
        {
            GraphLoader graphLoader = new GraphLoader();
            Graph graph = graphLoader.Load("AB200, BC100 ");
            Assert.IsNotNull(graph);
            var nodes = graph.GetNodes();

            CollectionAssert.AreEquivalent(new[] { "A", "B", "C" }.ToList(), nodes.ToList());

            var neighbors = graph.GetNeighbors("A").ToList();

            Assert.AreEqual(1, neighbors.Count);
            Assert.AreEqual("A", neighbors[0].FromNode);
            Assert.AreEqual("B", neighbors[0].ToNode);
            Assert.AreEqual(200, neighbors[0].Weight);
        }

        //TODO tests for invalid inputs
    }
}