using System;
using System.Collections.Generic;
using System.Linq;
using Graphiti.Core;

namespace Graphiti.RailroadService
{
    public class RouteQuery
    {
        private readonly Graph m_map;
        public RouteQuery(Graph map)
        {
            m_map = map;
        }
        public float CalculateDistance(IEnumerable<string> stops)
        {
            string from = null;
            string to = null;
            float distance = 0;

            foreach (string stop in stops)
            {
                from = to;
                to = stop;
                if (from != null)
                {
                    if (!m_map.TryGetEdge(from, to, out GraphEdge edge))
                    {
                        throw new RouteNotFoundException();
                    }
                    distance += edge.Weight;
                }
            }
            return distance;
        }

        public IEnumerable<GraphRoute> GetAllRoutes(string from, string to, int? minStops, int? maxStops)
        {
            GraphTraverser traverser = new GraphTraverser(m_map);
            var allRoutes = traverser.Traverse(from, to);
            return allRoutes.Where(x =>
            {
                int stops = x.GetEdgeCount();
                return (minStops == null || stops >= minStops.Value) &&
                    (maxStops == null || stops <= maxStops.Value);
            });
        }
    }
}
