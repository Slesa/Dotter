using Prism.Events;

namespace Dotter.Core
{
    public class StatusbarMessageEvent : PubSubEvent<string> {}
    public class TextInputUpdatedEvent : PubSubEvent<string> { }
    public class TextInputValidatedEvent : PubSubEvent<string> { }
    public class InvalidInputEvent : PubSubEvent<int> { }
    public class ClearTextInputEvent : PubSubEvent<int> { }
    public class FillTextInputEvent : PubSubEvent<string> { }
    public class LoadFileEvent : PubSubEvent<string> { }
    public class SaveFileEvent : PubSubEvent<int> { }
    public class FileSavedEvent : PubSubEvent<string> { }
    public class FileLoadedEvent : PubSubEvent<string> { }
    public class PreviewUpdatedEvent : PubSubEvent<string> { }
    public class CopyPreviewEvent : PubSubEvent<int> { }

    public class NewNodeInConnectAdded : PubSubEvent<string> { }
}