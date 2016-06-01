using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Dotter.Core.Model;
using Prism.Commands;
using Prism.Events;

namespace Dotter.Core.ViewModels
{
    public class WizardEdgesPageViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<GraphEdgeViewModel> Edges { get; set; }
        public ObservableCollection<string> Nodes { get; set; }
        public int SelectedIndex { get; set; }

        public WizardEdgesPageViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            Nodes = new ObservableCollection<string>();
            Edges = new ObservableCollection<GraphEdgeViewModel>();
            Edges.CollectionChanged += OnEdgesChanged;
        }

        void OnEdgesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (GraphEdgeViewModel item in e.NewItems)
                {
                    item.PropertyChanged += OnEdgesPropertyChanged;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (GraphEdgeViewModel item in e.OldItems)
                {
                    item.PropertyChanged -= OnEdgesPropertyChanged;
                }
            }
        }

        void OnEdgesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var edgevm = sender as GraphEdgeViewModel;
            if (e.PropertyName == "From")
            {
                if (!Nodes.Contains(edgevm.From)) AddNodeToCollection(edgevm.From);
                
            }
            if (e.PropertyName == "To")
            {
                if (!Nodes.Contains(edgevm.To)) AddNodeToCollection(edgevm.To);
            }
        }

        private void AddNodeToCollection(string node)
        {
            Nodes.Add(node);
            _eventAggregator.GetEvent<NewNodeInConnectAdded>().Publish(node);
        }

        public GraphDescription FillDescription(GraphDescription description)
        {
            description.Edges = new List<GraphEdge>();
            description.Edges.AddRange(from edge in Edges select new GraphEdge { From = edge.From, To = edge.To });
            return description;
        }

        #region Add
        public DelegateCommand AddCommand
        {
            get { return new DelegateCommand(OnAdd, CanAdd); }
        }

        void OnAdd()
        {
            Edges.Add(new GraphEdgeViewModel());
        }

        bool CanAdd()
        {
            if (!Edges.Any()) return true;
            var edge = Edges[Edges.Count-1];
            return !string.IsNullOrEmpty(edge.From) && !string.IsNullOrEmpty(edge.To);
        }

        #endregion

        #region Remove
        public DelegateCommand RemoveCommand
        {
            get { return new DelegateCommand(OnRemove, CanRemove).ObservesProperty(() => SelectedIndex); }
        }

        void OnRemove()
        {
            Edges.RemoveAt(SelectedIndex);
        }

        bool CanRemove()
        {
            return SelectedIndex >= 0;
        }
        #endregion
    }
}