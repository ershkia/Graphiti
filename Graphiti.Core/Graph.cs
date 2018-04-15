using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Graphiti.Core
{
    public class Graph
    {
        private ConcurrentDictionary<string, ConcurrentBag<GraphEdge>> m_nodes;

        public Graph()
        {
            m_nodes = new ConcurrentDictionary<string, ConcurrentBag<GraphEdge>>();
        }

        public void AddNode(string node)
        {
            if (NodeExists(node))
            {
                return;
            }
            var noNeighbors = new ConcurrentBag<GraphEdge>();
            m_nodes.AddOrUpdate(node, noNeighbors, (key, value) => noNeighbors);
        }

        public void AddEdge(GraphEdge edge)
        {
            AddEdge(edge.FromNode, edge.ToNode, edge.Weight);
        }

        public bool TryGetEdge(string from, string to, out GraphEdge edge)
        {
            edge = null;
            if (!NodeExists(from) || !NodeExists(to))
            {
                return false;
            }
            edge = m_nodes[from].FirstOrDefault(x => x.ToNode == to);
            return edge != null;
        }

        public void AddEdge(string fromNode, string toNode, float weight)
        {
            if (!NodeExists(fromNode))
            {
                throw new Exception($"Node: {fromNode} does not exist.");
            }

            if (!NodeExists(toNode))
            {
                throw new Exception($"Node: {toNode} does not exist.");
            }

            if (fromNode == toNode)
            {
                throw new Exception("fromNode and toNode cannot be the same.");
            }

            GraphEdge edge = new GraphEdge(fromNode, toNode, weight);

            if (m_nodes[fromNode].Contains(edge))
            {
                throw new Exception($"Edge: <{fromNode},{toNode}> exists already.");
            }

            m_nodes[fromNode].Add(edge);
        }

        public IList<string> GetNodes()
        {
            return m_nodes.Keys.ToList();
        }

        public IEnumerable<GraphEdge> GetEdges()
        {
            return m_nodes.Values.SelectMany(flat => flat);
        }

        public IReadOnlyCollection<GraphEdge> GetNeighbors(string node)
        {
            if (!NodeExists(node))
            {
                throw new Exception($"Node: {node} does not exist.");
            }

            return m_nodes[node];
        }

        public bool NodeExists(string node)
        {
            return m_nodes.ContainsKey(node);
        }

    }
}
