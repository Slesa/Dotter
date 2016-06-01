using System.Collections.ObjectModel;
using Dotter.Grammar;
using Prism.Events;
using Prism.Mvvm;

namespace Dotter.Core.ViewModels
{
    public class GrammarViewModel : BindableBase, IHandleParserErrors
    {
        readonly IEventAggregator _eventAggregator;
        readonly DotterParser _parser;
        bool _hasErrors;

        public GrammarViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            ParserErrors = new ObservableCollection<ErrorDescription>();
            _parser = new DotterParser(this);

            eventAggregator.GetEvent<TextInputUpdatedEvent>().Subscribe(OnTextUpdated);
        }

        void OnTextUpdated(string text)
        {
            HasErrors = false;
            ParserErrors.Clear();
            if( _parser.ParseValid(text) )
                _eventAggregator.GetEvent<TextInputValidatedEvent>().Publish(text);
            else
                _eventAggregator.GetEvent<InvalidInputEvent>().Publish(0);
        }

        public ObservableCollection<ErrorDescription> ParserErrors { get; private set; }

        public bool HasErrors
        {
            get { return _hasErrors; }
            private set
            {
                if (_hasErrors == value) return;
                _hasErrors = value; 
                OnPropertyChanged();
            }
        }

        public void HandleError(ErrorDescription error)
        {
            HasErrors = true;
            ParserErrors.Add(error);
        }
    }
}