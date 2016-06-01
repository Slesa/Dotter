using Dotter.Core.Model;
using Prism.Mvvm;

namespace Dotter.Core.ViewModels
{
    public class WizardInfoPageViewModel : BindableBase
    {
        public WizardInfoPageViewModel()
        {
            GraphType = GraphType.Graph;
            FlowModel = FlowModel.LeftToRight;
        }

        public GraphDescription FillDescription(GraphDescription description)
        {
            description.Name = GraphName;
            description.Type = GraphType;
            description.FlowModel = FlowModel;
            return description;
        }

        string _graphName;
        public string GraphName
        {
            get { return _graphName; }
            set { _graphName = value; OnPropertyChanged(); }
        }

        GraphType _graphType;
        public GraphType GraphType
        {
            get { return _graphType; }
            set { _graphType = value; OnPropertyChanged(); }
        }

        FlowModel _flowModel;
        public FlowModel FlowModel
        {
            get { return _flowModel; }
            set { _flowModel = value; OnPropertyChanged(); }
        }
    }
}