using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Dotter.Core.Helpers;
using Prism.Events;
using Prism.Mvvm;

namespace Dotter.Core.ViewModels
{
    public class PreviewViewModel : BindableBase
    {
        readonly IEventAggregator _eventAggregator;
        private readonly DotCaller _dotCaller = new DotCaller();
        private Uri _preview;


        public PreviewViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
//            eventAggregator.GetEvent<TextInputValidatedEvent>().Subscribe(OnInputUpdated);
            eventAggregator.GetEvent<TextInputUpdatedEvent>().Subscribe(OnInputUpdated);
            eventAggregator.GetEvent<CopyPreviewEvent>().Subscribe(_ => OnCopyPreview());
            eventAggregator.GetEvent<InvalidInputEvent>().Subscribe(_ => { Preview = null; });
        }


        BitmapImage _previewImage;
        public BitmapImage PreviewImage
        {
            get { return _previewImage; }
            private set
            {
                _previewImage = value;
                OnPropertyChanged();
            }
        }

        public Uri Preview
        {
            get { return _preview; }
            set { 
                _preview = value; 
                OnPropertyChanged();

                if (value == null)
                {
                    PreviewImage = null;
                    return;
                }

                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = _preview;
                image.EndInit();
                PreviewImage = image;
            }
        }

        string _oldPreviewFile;

        private void OnInputUpdated(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Preview = null;
                return;
            }

            var previewFile = _dotCaller.Run(text);
            if (string.IsNullOrEmpty(previewFile))
            {
                _eventAggregator.GetEvent<StatusbarMessageEvent>().Publish("Could not process input");
                return;
            }

            Preview = new Uri(previewFile, UriKind.Absolute);
            if (!string.IsNullOrEmpty(_oldPreviewFile))
            {
                try
                {
                    File.Delete(_oldPreviewFile);
                }
                catch (Exception)
                {
                }
            }
            _oldPreviewFile = previewFile;
            _eventAggregator.GetEvent<PreviewUpdatedEvent>().Publish(previewFile);
            _eventAggregator.GetEvent<StatusbarMessageEvent>().Publish("Preview updated");
        }

        void OnCopyPreview()
        {
            Clipboard.SetImage(PreviewImage);
        }
    }
}