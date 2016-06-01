using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Dotter.Core.Model;
using Prism.Events;
using Prism.Mvvm;

namespace Dotter.Core.ViewModels
{
    public class WizardNodesPageViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        public ObservableCollection<GraphNodeViewModel> Nodes { get; set; }
        public ObservableCollection<GraphShapeViewModel> AvailableShapes { get; set; }
        public ObservableCollection<string> AvailableColors { get; set; }

        private object _selectedValue;
        public object SelectedValue
        {
            get { return _selectedValue; }
            set { _selectedValue = value;
                OnPropertyChanged();}
        }

        private GraphNodeViewModel _selectedItem;
        public GraphNodeViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value;
                OnPropertyChanged(); }
        }

        public WizardNodesPageViewModel(IEventAggregator eventAggregator)
        {
            Nodes = new ObservableCollection<GraphNodeViewModel>();

            AvailableShapes = new ObservableCollection<GraphShapeViewModel>();
            AvailableShapes.AddRange(GetAvailableShapes());
            AvailableColors = new ObservableCollection<string>();
            AvailableColors.AddRange(GetAvailableColors());

            eventAggregator.GetEvent<NewNodeInConnectAdded>().Subscribe(AddNodeIfMissing);
            _eventAggregator = eventAggregator;
        }

        public GraphDescription FillDescription(GraphDescription description)
        {
            description.Nodes = new List<GraphNode>();
            if (Nodes != null)
                description.Nodes.AddRange(from node in Nodes select new GraphNode()
                {
                    Name = node.Name,
                    Label = node.Label,
                    Shape = node.Shape,
                    Color = node.Color,
                    FontColor = node.FontColor,
                    FontSize = node.FontSize,
                    Style = node.Style,
                    FillColor = node.FillColor,
                });
            return description;
        }


        private void AddNodeIfMissing(string node)
        {
            if (Nodes.FirstOrDefault(x => x.Name==node) == null) Nodes.Add(new GraphNodeViewModel { Name = node });
        }

        private IEnumerable<string> GetAvailableColors()
        {
            yield return "default";
            yield return "black";
            yield return "blue";
            yield return "darkblue";
            yield return "lightblue";
            yield return "red";
            yield return "darkred";
            yield return "lightred";
            yield return "gray";
            yield return "darkgray";
            yield return "lightgray";
            yield return "orange";
            yield return "darkorange";
            yield return "lightorange";
            yield return "brown";
            yield return "darkbrown";
            yield return "lightbrown";
            yield return "cyan";
            yield return "darkcyan";
            yield return "lightcyan";
            yield return "violet";
            yield return "darkviolet";
            yield return "lightviolet";
            yield return "yellow";
            yield return "darkyellow";
            yield return "lightyellow";
            yield return "green";
            yield return "darkgreen";
            yield return "lightgreen";
            yield return "white";
        }

        private IEnumerable<GraphShapeViewModel> GetAvailableShapes()
        {
            yield return new GraphShapeViewModel() { Name = "Box", Value = "box" ,
                ImageSource = FindImageSource("Shape.Box") };
            yield return new GraphShapeViewModel() { Name = "Polygon", Value = "polygon",
                ImageSource = FindImageSource("Shape.Polygon") };
            yield return new GraphShapeViewModel() { Name = "Ellipse", Value = "ellipse",
                ImageSource = FindImageSource("Shape.Ellipse") };
            yield return new GraphShapeViewModel() { Name = "Oval", Value = "oval",
                ImageSource = FindImageSource("Shape.Oval") };
            yield return new GraphShapeViewModel() { Name = "Circle", Value = "circle",
                ImageSource = FindImageSource("Shape.Circle") };
            yield return new GraphShapeViewModel() { Name = "Point", Value = "point",
                ImageSource = FindImageSource("Shape.Point") };
            yield return new GraphShapeViewModel() { Name = "Egg", Value = "egg",
                ImageSource = FindImageSource("Shape.Egg") };
            yield return new GraphShapeViewModel() { Name = "Triangle", Value = "triangle",
                ImageSource = FindImageSource("Shape.Triangle") };
            yield return new GraphShapeViewModel() { Name = "Plain text", Value = "plaintext",
                ImageSource = FindImageSource("Shape.Plaintext") };
            yield return new GraphShapeViewModel() { Name = "Plain", Value = "plain",
                ImageSource = FindImageSource("Shape.Plain") };
            yield return new GraphShapeViewModel() { Name = "Diamond", Value = "diamond",
                ImageSource = FindImageSource("Shape.Diamond") };
            yield return new GraphShapeViewModel() { Name = "Trapezium", Value = "trapezium",
                ImageSource = FindImageSource("Shape.Trapezium") };
            yield return new GraphShapeViewModel() { Name = "Parallelogram", Value = "parallelogram",
                ImageSource = FindImageSource("Shape.Parallelogram") };
            yield return new GraphShapeViewModel() { Name = "House", Value = "house",
                ImageSource = FindImageSource("Shape.House") };
            yield return new GraphShapeViewModel() { Name = "Pentagon", Value = "pentagon",
                ImageSource = FindImageSource("Shape.Pentagon") };
            yield return new GraphShapeViewModel() { Name = "Hexagon", Value = "hexagon",
                ImageSource = FindImageSource("Shape.Hexagon") };
            yield return new GraphShapeViewModel() { Name = "Septagon", Value = "septagon",
                ImageSource = FindImageSource("Shape.Septagon") };
            yield return new GraphShapeViewModel() { Name = "Octagon", Value = "octagon",
                ImageSource = FindImageSource("Shape.Octagon") };
            yield return new GraphShapeViewModel() { Name = "Double cirlce", Value = "doublecircle",
                ImageSource = FindImageSource("Shape.DoubleCircle") };
            yield return new GraphShapeViewModel() { Name = "Double octagon", Value = "doubleoctagon",
                ImageSource = FindImageSource("Shape.DoubleOctagon") };
            yield return new GraphShapeViewModel() { Name = "Triple octagon", Value = "tripleoctagon",
                ImageSource = FindImageSource("Shape.TripleOctagon") };
            yield return new GraphShapeViewModel() { Name = "Inverted triangle", Value = "invtriangle",
                ImageSource = FindImageSource("Shape.Invtriangle") };
            yield return new GraphShapeViewModel() { Name = "Inverted trapezium", Value = "invtrapezium",
                ImageSource = FindImageSource("Shape.Invtrapezium") };
            yield return new GraphShapeViewModel() { Name = "Inverted house", Value = "invhouse",
                ImageSource = FindImageSource("Shape.Invhouse") };
            yield return new GraphShapeViewModel() { Name = "Mdiamond", Value = "Mdiamond",
                ImageSource = FindImageSource("Shape.Mdiamond") };
            yield return new GraphShapeViewModel() { Name = "Msquare", Value = "Msquare",
                ImageSource = FindImageSource("Shape.Msquare") };
            yield return new GraphShapeViewModel() { Name = "Mcircle", Value = "Mcircle",
                ImageSource = FindImageSource("Shape.Mcircle") };
        }

        private ImageSource FindImageSource(string name)
        {
            return new BitmapImage(new Uri("pack://application:,,,/Dotter.Theme;component/Resources/Images/"+name+".png"));
        }
    }
}