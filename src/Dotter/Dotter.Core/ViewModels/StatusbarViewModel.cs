using System;
using System.Threading.Tasks;
using System.Windows.Interop;
using Prism.Events;
using Prism.Mvvm;

namespace Dotter.Core.ViewModels
{
    public class StatusbarViewModel : BindableBase
    {
        private string _message;

        public StatusbarViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<StatusbarMessageEvent>().Subscribe(OnMessage);
        }

        private void OnMessage(string msg)
        {
            Message = msg;
            Task.Factory.StartNew(() => Task.Delay(3000).ContinueWith(new Action<Task>(_ => Message = string.Empty)));
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }
    }
}