﻿using System;
using System.Collections.Generic;
using System.Linq;
using Graphiti.Core;

namespace Graphiti.RailroadService
{
    public class MapQuery
    {
        private readonly Graph m_map;
        public MapQuery(Graph map)
        {
            m_map = map;
        }
        public float GetTotalDistance(IEnumerable<string> stops)
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

        public IEnumerable<GraphRoute> GetRoutesByStopCount(string from, string to, int? minStops, int maxStops)
        {
            GraphTraverser traverser = new GraphTraverser(m_map);
            var allRoutes = traverser.TraverseByStops(from, to, maxStops);
            return allRoutes.Where(x =>
            {
                int stops = x.GetEdgeCount();
                return (minStops == null || stops >= minStops.Value);
            });
        }

        public IEnumerable<GraphRoute> GetRoutesByDistance(string from, string to, float maxTotalDistance)
        {
            GraphTraverser traverser = new GraphTraverser(m_map);
            return traverser.TraverseByWeight(from, to, maxTotalDistance);
        }

        public GraphRoute GetRouteWithMinStops(string from, string to)
        {
            GraphTraverser traverser = new GraphTraverser(m_map);
            int maxStops = m_map.GetEdges().Count();
            var allRoutes = traverser.TraverseByStops(from, to, maxStops);

            if (!allRoutes.Any())
            {
                throw new RouteNotFoundException();
            }
            return allRoutes.OrderBy(x => x.GetTotalWeight()).First();
        }
    }
}
