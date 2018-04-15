using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Graphiti.Core
{
    public class GraphLoader
    {
        public Graph Load(string input)
        {
            string pattern = @"\w+";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = r.Match(input);

            Graph graph = new Graph();

            while (match.Success)
            {
                AddEdge(graph, match.Value);
                match = match.NextMatch();
            }

            return graph;
        }

        private void AddEdge(Graph graph, string edge)
        {
            string pattern = @"^([a-zA-Z])([a-zA-Z])([0-9]+)$";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = r.Match(edge);

            if (match.Success)
            {
                string from = match.Groups[1].ToString();
                string to = match.Groups[2].ToString();
                int weight = Int32.Parse(match.Groups[3].ToString());

                graph.AddNode(from);
                graph.AddNode(to);
                graph.AddEdge(from, to, weight);
            }
            else
            {
                throw new Exception($"Invalid Edge: {edge}");
            }
        }
    }
}