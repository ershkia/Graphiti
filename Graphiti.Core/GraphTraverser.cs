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

        public IEnumerable<GraphRoute> TraverseByStops(string fromNode, string toNode, int maxStops)
        {
            return Traverse(fromNode, toNode, maxStops, (x, y) => y - 1);
        }

        public IEnumerable<GraphRoute> TraverseByWeight(string fromNode, string toNode, float maxTotalWeight)
        {
            return Traverse(fromNode, toNode, maxTotalWeight, (x, y) => y - x.Weight);
        }

        private IEnumerable<GraphRoute> Traverse(string fromNode, string toNode, float maxCredit, Func<GraphEdge, float, float> creditor)
        {
            if (!m_graph.NodeExists(fromNode))
            {
                throw new Exception($"Node: {fromNode} does not exist.");
            }

            if (!m_graph.NodeExists(toNode))
            {
                throw new Exception($"Node: {toNode} does not exist.");
            }
            return TraverseRecursive(fromNode, toNode, maxCredit, creditor);
        }

        private IEnumerable<GraphRoute> TraverseRecursive(string fromNode, string toNode, float creditLeft, Func<GraphEdge, float, float> creditor)
        {
            if (creditLeft <= 0)
            {
                return Enumerable.Empty<GraphRoute>();
            }

            var neighbors = m_graph.GetNeighbors(fromNode);

            IEnumerable<GraphRoute> result = neighbors
                .Select(edge => TraverseRecursive(edge.ToNode, toNode, creditor(edge, creditLeft), creditor)
                                .Select(route => route.AppendToStart(edge)))
                .SelectMany(flat => flat)
                .Concat(neighbors
                    .Where(x => x.ToNode == toNode && creditor(x, creditLeft) >= 0)
                    .Select(y => new GraphRoute(y)));

            return result;
        }
    }
}
