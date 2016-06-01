using Prism.Mvvm;

namespace Dotter.Core.ViewModels
{
    public class GraphEdgeViewModel : BindableBase
    {
        string _from;
        public string From
        {
            get { return _from; }
            set { _from = value; OnPropertyChanged();}
        }

        string _to;
        public string To
        {
            get { return _to; }
            set { _to = value; OnPropertyChanged(); }
        }
    }
}