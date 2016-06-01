using System.Dynamic;
using System.Windows;
using Dotter.Core.Views;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Dotter.Core.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private bool _inputEmpty;
        private bool _inputDifferent;
        string _previewFilename;

        public MenuViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<TextInputUpdatedEvent>().Subscribe(OnInputUpdated);
            eventAggregator.GetEvent<FileSavedEvent>().Subscribe(OnInputLoadedOrSaved);
            eventAggregator.GetEvent<FileLoadedEvent>().Subscribe(OnInputLoadedOrSaved);
            eventAggregator.GetEvent<PreviewUpdatedEvent>().Subscribe(OnPreviewUpdated);
            OnInputUpdated(string.Empty);
        }

        #region File New
        public DelegateCommand FileNewCommand
        {
            get { return new DelegateCommand(DoFileNew, CanFileNew).ObservesProperty(()=>this.InputEmpty); }
        }


        private void DoFileNew()
        {
            _eventAggregator.GetEvent<ClearTextInputEvent>().Publish(0);
        }
        private bool CanFileNew()
        {
            return !InputEmpty;
        }
        #endregion

        #region File Open
        public DelegateCommand FileOpenCommand { get { return new DelegateCommand(DoFileOpen); } }

        private void DoFileOpen()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DOT files (*.dot)|*.dot|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() != true) return;

            _eventAggregator.GetEvent<LoadFileEvent>().Publish(openFileDialog.FileName);
        }
        #endregion

        #region File Save
        public DelegateCommand FileSaveCommand
        {
            get { return new DelegateCommand(DoFileSave, CanFileSave).ObservesProperty(() => this.InputDifferent); }
        }

        private void DoFileSave()
        {
            /*
                        var saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "*.dot";
                        if (saveFileDialog.ShowDialog() != true) return;

                        _eventAggregator.GetEvent<SaveFileEvent>().Publish(saveFileDialog.FileName);
            */
            _eventAggregator.GetEvent<SaveFileEvent>().Publish(0);
        }
        private bool CanFileSave()
        {
            return InputDifferent;
        }
        #endregion

        #region Copy file name

        public DelegateCommand CopyFileNameCommand
        {
            get { return new DelegateCommand(DoCopyFileName, CanCopyFileName).ObservesProperty(()=>this.PreviewFilename);}
        }

        void DoCopyFileName()
        {
            Clipboard.SetText(PreviewFilename);   
        }

        bool CanCopyFileName()
        {
            return !string.IsNullOrEmpty(PreviewFilename);
        }

        #endregion

        #region Copy file ikmage

        public DelegateCommand CopyFileContentCommand
        {
            get { return new DelegateCommand(DoCopyFileContent, CanCopyFileContent).ObservesProperty(() => this.PreviewFilename); }
        }

        void DoCopyFileContent()
        {
            _eventAggregator.GetEvent<CopyPreviewEvent>().Publish(0);
        }

        bool CanCopyFileContent()
        {
            return !string.IsNullOrEmpty(PreviewFilename);
        }

        #endregion

        #region Wizard

        WizardView _wizardView;

        public DelegateCommand WizardCommand
        {
            get { return new DelegateCommand(DoWizard); }
        }


        private void DoWizard()
        {
            if (_wizardView == null)
            {
                _wizardView = new WizardView(_eventAggregator);
                _wizardView.Closed += (sender, args) => _wizardView = null;
                _wizardView.Show();
            }
            else
            {
                _wizardView.Close();
            }
        }
        #endregion

        private string FormerInput { get; set; }

        bool InputEmpty
        {
            get { return _inputEmpty; }
            set { _inputEmpty = value; OnPropertyChanged(); }
        }

        bool InputDifferent
        {
            get { return _inputDifferent; }
            set { _inputDifferent = value; OnPropertyChanged(); }
        }

        private void OnInputUpdated(string text)
        {
            InputEmpty = string.IsNullOrEmpty(text);
            InputDifferent = FormerInput != text;
        }

        private void OnInputLoadedOrSaved(string text)
        {
            FormerInput = text;
            InputDifferent = false;
        }

        string PreviewFilename
        {
            get { return _previewFilename; }
            set { _previewFilename = value; OnPropertyChanged(); }
        }

        void OnPreviewUpdated(string currentFile)
        {
            PreviewFilename = currentFile;
        }
    }
}