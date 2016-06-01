using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Dotter.Core.Model;
using Dotter.Core.ViewModels;
using Prism.Commands;
using Prism.Events;

namespace Dotter.Core.Views
{
    /// <summary>
    /// Interaction logic for WizardView.xaml
    /// </summary>
    public partial class WizardView : Window, INotifyPropertyChanged
    {
        readonly IEventAggregator _eventAggregator;

        public WizardView(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            InitializeComponent();
        }

        #region Close
        public DelegateCommand CloseCommand
        {
            get { return new DelegateCommand(Close); }
        }
        #endregion

        #region Finish
        public DelegateCommand FinishCommand
        {
            get { return new DelegateCommand(OnFinish, CanFinish).ObservesProperty(()=>SelectedPage); }
        }

        void OnFinish()
        {
            var description = new GraphDescription();

            var pageVm = _infoPageView.DataContext as WizardInfoPageViewModel;
            description = pageVm.FillDescription(description);
            var edgesVm = _edgesPageView.DataContext as WizardEdgesPageViewModel;
            description = edgesVm.FillDescription(description);
            var nodesVm = _nodesPageView.DataContext as WizardNodesPageViewModel;
            description = nodesVm.FillDescription(description);

            _eventAggregator.GetEvent<FillTextInputEvent>().Publish(description.ToString());

            Close();
        }

        bool CanFinish()
        {
            return SelectedPage > 1;
        }
        #endregion

        #region Backward
        public DelegateCommand BackwardCommand
        {
            get { return new DelegateCommand(OnBackward, CanBackward).ObservesProperty(() => SelectedPage); }
        }

        void OnBackward()
        {
            SelectedPage -= 1;
        }

        bool CanBackward()
        {
            return SelectedPage > 0;
        }
        #endregion

        #region Forward
        public DelegateCommand ForwardCommand
        {
            get { return new DelegateCommand(OnForward, CanForward).ObservesProperty(() => SelectedPage); }
        }

        void OnForward()
        {
            SelectedPage += 1;
        }

        bool CanForward()
        {
            return SelectedPage < 2;
        }
        #endregion

        #region Selected Page
        int _selectedPage;
        public int SelectedPage
        {
            get { return _selectedPage; }
            set { _selectedPage = value; OnPropertyChanged(); }
        }
        #endregion

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
