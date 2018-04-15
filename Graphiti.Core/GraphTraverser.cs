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
        public IEnumerable<GraphRoute> Traverse(string fromNode, string toNode, int maxEdgeCount)
        {
            if (!m_graph.NodeExists(fromNode))
            {
                throw new Exception($"Node: {fromNode} does not exist.");
            }

            if (!m_graph.NodeExists(toNode))
            {
                throw new Exception($"Node: {toNode} does not exist.");
            }
            return TraverseRecursive(fromNode, toNode, maxEdgeCount);
        }

        private IEnumerable<GraphRoute> TraverseRecursive(string fromNode, string toNode, int maxEdgeCount)
        {
            if (maxEdgeCount == 0)
            {
                return Enumerable.Empty<GraphRoute>();
            }

            var neighbors = m_graph.GetNeighbors(fromNode);

            IEnumerable<GraphRoute> result = neighbors
                .Select(edge => TraverseRecursive(edge.ToNode, toNode, maxEdgeCount - 1)
                                .Select(route => route.AppendToStart(edge)))
                .SelectMany(flat => flat)
                .Concat(neighbors.Where(x => x.ToNode == toNode).Select(y => new GraphRoute(y)));

            return result;
        }
    }
}
