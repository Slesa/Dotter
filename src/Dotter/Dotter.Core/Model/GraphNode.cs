
using Dotter.Core.ViewModels;

namespace Dotter.Core.Model
{
    public class GraphNode
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public GraphShapeViewModel Shape { get; set; }
        public string Color { get; set; }
        public string FontColor { get; set; }
        public int FontSize { get; set; }
        public string Style { get; set; }
        public string FillColor { get; set; }
    }
}