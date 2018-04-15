using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphiti.Core
{
    public class GraphPathFinder
    {
        private readonly Graph m_graph;

        public GraphPathFinder(Graph graph)
        {
            m_graph = graph;
        }
        public IEnumerable<IEnumerable<GraphEdge>> FindPaths(string fromNode, string toNode)
        {
            if (!m_graph.NodeExists(fromNode))
            {
                throw new Exception($"Node: {fromNode} does not exist.");
            }

            if (!m_graph.NodeExists(toNode))
            {
                throw new Exception($"Node: {toNode} does not exist.");
            }
            return FindPaths(fromNode, toNode, new List<string>());
        }

        private IEnumerable<IEnumerable<GraphEdge>> FindPaths(string fromNode, string toNode, IEnumerable<string> visitedNodes)
        {
            var neighbors = m_graph.GetNeighbors(fromNode);

            var result = neighbors
                .Where(n => n.ToNode != toNode && !visitedNodes.Contains(n.ToNode))
                .Select(y => FindPaths(y.ToNode, toNode, visitedNodes.Concat(new[] { fromNode }))
                .SelectMany(z => new[] { y }.Concat(z)));

            var directPath = neighbors.Where(x => x.ToNode == toNode);
           
            if (directPath.Any())
            {
                result = new[] { directPath }.Concat(result);
            }
            return result;
        }
    }
}
