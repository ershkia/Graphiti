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
        public void DirectPath()
        {
            Graph graph = new Graph();
            graph.AddNode("A");
            graph.AddNode("B");
            graph.AddEdge("A", "B", 1);

            GraphPathFinder pathFinder = new GraphPathFinder(graph);
            var result = pathFinder.FindPaths("A", "B").ToList();

            Assert.AreEqual(1, result.Count());

            CollectionAssert.AreEqual(new[] { "A" }, result[0].Select(x => x.FromNode).ToList());
            CollectionAssert.AreEqual(new[] { "B" }, result[0].Select(x => x.ToNode).ToList());
        }

        [TestMethod]
        public void IndirectPath()
        {
            Graph graph = new Graph();
            graph.AddNode("A");
            graph.AddNode("B");
            graph.AddNode("C");
            graph.AddEdge("A", "B", 1);
            graph.AddEdge("B", "C", 2);
            graph.AddEdge("C", "B", 3);

            GraphPathFinder pathFinder = new GraphPathFinder(graph);
            var result = pathFinder.FindPaths("A", "C").ToList();

            Assert.AreEqual(1, result.Count());

            CollectionAssert.AreEqual(new[] { "A", "B" }, result[0].Select(x => x.FromNode).ToList());
            CollectionAssert.AreEqual(new[] { "B", "C" }, result[0].Select(x => x.ToNode).ToList());
        }

        public void FindPathBackHome()
        {
            Graph graph = new Graph();
            graph.AddNode("A");
            graph.AddNode("B");
            graph.AddEdge("A", "B", 1);
            graph.AddEdge("B", "A", 2);

            GraphPathFinder pathFinder = new GraphPathFinder(graph);
            var result = pathFinder.FindPaths("A", "A").ToList();

            Assert.AreEqual(1, result.Count());

            CollectionAssert.AreEqual(new[] { "A", "B" }, result[0].Select(x => x.FromNode).ToList());
            CollectionAssert.AreEqual(new[] { "B", "A" }, result[0].Select(x => x.ToNode).ToList());


        }
    }
}