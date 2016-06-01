using System.IO;
using Microsoft.Win32;
using Prism.Events;
using Prism.Mvvm;

namespace Dotter.Core.ViewModels
{
    public class TextEditorViewModel : BindableBase
    {
        private string _input;
        private readonly IEventAggregator _eventAggregator;

        public TextEditorViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<ClearTextInputEvent>()?.Subscribe(_ => Input = string.Empty);
            eventAggregator.GetEvent<FillTextInputEvent>()?.Subscribe(text => Input = text);
            eventAggregator.GetEvent<LoadFileEvent>()?.Subscribe(OnLoadFile);
            eventAggregator.GetEvent<SaveFileEvent>()?.Subscribe(_ => OnSaveFile());
        }

        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                OnPropertyChanged();
                _eventAggregator.GetEvent<TextInputUpdatedEvent>().Publish(value);
            }
        }


        internal string CurrentFileName { get; set; }

        internal void OnLoadFile(string fileName)
        {
            var text = File.ReadAllText(fileName);
            Input = text;
            CurrentFileName = fileName;

            _eventAggregator.GetEvent<StatusbarMessageEvent>().Publish(fileName + " loaded");
            _eventAggregator.GetEvent<FileLoadedEvent>().Publish(Input);
            _eventAggregator.GetEvent<TextInputUpdatedEvent>().Publish(Input);
        }

        private void OnSaveFile()
        {
            if (string.IsNullOrEmpty(CurrentFileName))
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "DOT files (*.dot)|*.dot|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() != true) return;

                CurrentFileName = saveFileDialog.FileName;
            }

            File.WriteAllText(CurrentFileName, Input);
            _eventAggregator.GetEvent<FileSavedEvent>().Publish(Input);

            _eventAggregator.GetEvent<StatusbarMessageEvent>().Publish(CurrentFileName + " saved");
        }
    }
}