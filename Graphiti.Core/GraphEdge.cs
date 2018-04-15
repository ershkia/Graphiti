using System;

namespace Graphiti.Core
{
    public sealed class GraphEdge
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            GraphEdge other = (GraphEdge)obj;

            return (FromNode == other.FromNode) && (ToNode == other.ToNode);
        }

        public override int GetHashCode()
        {
            return this.FromNode.GetHashCode() * this.ToNode.GetHashCode();
        }
    }
}