using Prism.Mvvm;

namespace Dotter.Core.ViewModels
{
    public class GraphNodeViewModel : BindableBase
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        string _label;
        public string Label
        {
            get { return _label; }
            set { _label = value; OnPropertyChanged(); }
        }

        GraphShapeViewModel _shape;
        public GraphShapeViewModel Shape
        {
            get { return _shape; }
            set { _shape = value; OnPropertyChanged(); }
        }

        string _color;
        public string Color
        {
            get { return _color; }
            set { _color = value; OnPropertyChanged(); }
        }

        string _fontColor;
        public string FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; OnPropertyChanged(); }
        }

        int _fontSize;
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; OnPropertyChanged(); }
        }

        string _style;
        public string Style
        {
            get { return _style; }
            set { _style = value; OnPropertyChanged(); }
        }

        string _fillColor;
        public string FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; OnPropertyChanged(); }
        }
    }
}