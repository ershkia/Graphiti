using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphiti.Core
{
    public class GraphTraverser
    {
        private readonly Graph m_graph;

        public GraphTraverser(Graph graph)
        {
            m_graph = graph;
        }
        public IEnumerable<GraphRoute> Traverse(string fromNode, string toNode)
        {
            if (!m_graph.NodeExists(fromNode))
            {
                throw new Exception($"Node: {fromNode} does not exist.");
            }

            if (!m_graph.NodeExists(toNode))
            {
                throw new Exception($"Node: {toNode} does not exist.");
            }
            return Traverse(fromNode, toNode, new List<string>());
        }

        private IEnumerable<GraphRoute> Traverse(string fromNode, string toNode, IEnumerable<string> visitedNodes)
        {
            var neighbors = m_graph.GetNeighbors(fromNode);

            IList<GraphRoute> result = neighbors
                .Where(neighbor => neighbor.ToNode != toNode && !visitedNodes.Contains(neighbor.ToNode))
                .Select(edge => Traverse(edge.ToNode, toNode, visitedNodes.Concat(new[] { fromNode }))
                .Select(route => route.AppendToStart(edge)))
                .SelectMany(flat => flat)
                .ToList();

            IEnumerable<GraphEdge> directPath = neighbors.Where(x => x.ToNode == toNode);

            if (directPath.Any())
            {
                result.Add(new GraphRoute(directPath));
            }
            return result;
        }
    }
}
