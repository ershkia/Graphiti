using System;

namespace Graphiti.Core
{
    public sealed class GraphEdge : IEquatable<GraphEdge>
    {
        public GraphEdge(string fromNode, string toNode, float weight)
        {
            FromNode = fromNode;
            ToNode = toNode;
            Weight = weight;
        }

        public string FromNode { get; private set; }
        public string ToNode { get; private set; }
        public float Weight { get; private set; }

        bool IEquatable<GraphEdge>.Equals(GraphEdge other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

            return (FromNode == other.FromNode) && (ToNode == other.ToNode);
        }
    }
}