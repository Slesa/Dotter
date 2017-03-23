using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dotter.Core.Model
{
    public class GraphDescription
    {
        public string Name { get; set; }
        public GraphType Type { get; set; }
        public FlowModel FlowModel { get; set; }
        public List<GraphEdge> Edges { get; set; }
        public List<GraphNode> Nodes { get; set; }

        private string FlowModelToString(FlowModel flowModel)
        {
            switch (flowModel)
            {
                case FlowModel.RightToLeft:
                    return "RL";
                case FlowModel.LeftToRight:
                    return "LR";
                case FlowModel.BottomToTop:
                    return "BT";
            }
            return string.Empty;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Type == GraphType.Digraph) sb.Append("digraph ");
            if (Type == GraphType.Graph) sb.Append("graph ");
            sb.Append(Name).AppendLine("{");

            var fm = FlowModelToString(FlowModel);
            if (!string.IsNullOrEmpty(fm))
            {
                sb.Append("rankdir=").AppendLine(fm);
            }
            
            foreach (var node in Nodes)
            {
                var nodeText = GetNodeFields(node).ToList();
                if (nodeText.Any())
                {
                    sb.Append(node.Name).Append(("["));
                    sb.Append(string.Join(",", nodeText));
                    sb.Append("]");
                    sb.AppendLine();
                }
                //T[label = "Teacher" color = Blue, fontcolor = Red, fontsize = 24, ]      // node T
            }

            var direct = Type == GraphType.Digraph ? " -> " : " -- ";
            foreach (var edge in Edges)
            {
                sb.Append(edge.From).Append(direct).AppendLine(edge.To);
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private IEnumerable<string> GetNodeFields(GraphNode node)
        {
            if (!string.IsNullOrEmpty(node.Label)) yield return "label=\"" + node.Label + "\"";
            if (!string.IsNullOrEmpty(node.Color)) yield return "color=" + node.Color;
            if (!string.IsNullOrEmpty(node.FontColor)) yield return "fontcolor=" + node.FontColor;
            if (node.FontSize != 0) yield return "fontsize=" + node.FontSize;
            if (node.Shape!=null) yield return "shape="+node.Shape.Value;
        }
    }
}