using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Graphiti.Core
{
    public class GraphRoute
    {
        private IEnumerable<GraphEdge> m_edges;

        public GraphRoute(IEnumerable<GraphEdge> edges)
        {
            this.m_edges = edges;
        }
        public GraphRoute(GraphEdge edge) : this(new[] { edge }) { }

        public GraphRoute AppendToStart(GraphEdge y)
        {
            return new GraphRoute(new[] { y }.Concat(m_edges));
        }

        public int GetEdgeCount()
        {
            return m_edges.Count();
        }

        public float GetTotalWeight()
        {
            return m_edges.Sum(x => x.Weight);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            GraphRoute other = (GraphRoute)obj;

            if (m_edges.Count() != other.m_edges.Count())
            {
                return false;
            }
            for (int i = 0; i < m_edges.Count(); i++)
            {
                if (!m_edges.ElementAt(i).Equals(other.m_edges.ElementAt(i)))
                {
                    return false;
                }
            }
            return true;
        }

        public IReadOnlyCollection<GraphEdge> GetEdges(){
            return m_edges.ToList().AsReadOnly();
        }

        public override int GetHashCode()
        {
            return this.m_edges.GetHashCode() * this.m_edges.GetHashCode();
        }
    }
}