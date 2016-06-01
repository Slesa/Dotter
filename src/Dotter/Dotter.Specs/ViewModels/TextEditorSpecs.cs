using Dotter.Core;
using Dotter.Core.ViewModels;
using FluentAssertions;
using Moq;
using Prism.Events;
using TestFx;
using TestFx.SpecK;

namespace Dotter.Specs.ViewModels
{
    public static class FizzBuzzer
    {
        public static string Calculcate(int number)
        {
            var dividebleByThree = number % 3 == 0;
            var dividebleByFive = number % 5 == 0;

            if (dividebleByThree && dividebleByFive)
                return "FizzBuzz";
            if (dividebleByThree)
                return "Fizz";
            if (dividebleByFive)
                return "Buzz";

            return number.ToString();
        }
    }

    [Subject(typeof(FizzBuzzer), "Fizzme")]
    public class FizzBuzzerSpec : Spec
    {
        int Number;

        FizzBuzzerSpec() // ctor
        {
            Specify(x => FizzBuzzer.Calculcate(Number))
                .Case("Dividable by 3", _ => _
                   .Given("Number is 3", x => Number = 3)
                   .It("returns Fizz", x => x.Result.Should().Be("Fizz")));
        }
    }

    [Subject(typeof (TextEditorViewModel), "Initialize")]
    public class When_texteditor_is_initialized : Spec<TextEditorViewModel>
    {
        [Injected]
        [Default]
        EventAggregator eventAggregator;

        When_texteditor_is_initialized()
        {
            Specify (_ => _)
                .DefaultCase (_ => _
                    .It ("should have no input", x => x.Subject.Input.Should ().BeNullOrEmpty ())
                    .It ("should have no current file name", x => x.Subject.CurrentFileName.Should ().BeNullOrEmpty ()));
        }
    }

    [Subject(typeof (TextEditorViewModel), "Loading")]
    public class When_texteditor_loads_file : Spec<TextEditorViewModel>
    {
        When_texteditor_loads_file()
        {
            Specify(x => x.OnLoadFile (@"data\SimpleGraph"))
                .DefaultCase(_ => _
                .It("should publish statusbar event", _1 => _statusbarMessageEvent.Verify(x => x.Publish(It.IsAny<string>())))
                .It("should publish fileloaded event", _2 => _fileLoadedEvent.Verify(x => x.Publish(It.IsAny<string>()))));
        }

        public override TextEditorViewModel CreateSubject()
        {
            var _ea = new Mock<IEventAggregator>();
            //var _clearTextInputEvent = new Mock<ClearTextInputEvent>();
            //var _fillTextInputEvent = new Mock<FillTextInputEvent>();
            //var _loadFileEvent = new Mock<LoadFileEvent>();
            //var _saveFileEvent = new Mock<SaveFileEvent>();
            _fileLoadedEvent = new Mock<FileLoadedEvent> ();
            _statusbarMessageEvent = new Mock<StatusbarMessageEvent> ();
            //_ea.Setup(x => x.GetEvent<ClearTextInputEvent>()).Returns(_clearTextInputEvent.Object);
            //_ea.Setup(x => x.GetEvent<FillTextInputEvent>()).Returns(_fillTextInputEvent.Object);
            //_ea.Setup(x => x.GetEvent<LoadFileEvent>()).Returns(_loadFileEvent.Object);
            //_ea.Setup(x => x.GetEvent<SaveFileEvent>()).Returns(_saveFileEvent.Object);
            _ea.Setup(x => x.GetEvent<StatusbarMessageEvent>()).Returns(_statusbarMessageEvent.Object);
            _ea.Setup(x => x.GetEvent<FileLoadedEvent>()).Returns(_fileLoadedEvent.Object);

            _sut = new TextEditorViewModel(_ea.Object);
            return _sut;
        }

        //Mock<IEventAggregator> _ea;
        TextEditorViewModel _sut;
        //Mock<ClearTextInputEvent> _clearTextInputEvent;
        //Mock<FillTextInputEvent> _fillTextInputEvent;
        //Mock<LoadFileEvent> _loadFileEvent;
        //Mock<SaveFileEvent> _saveFileEvent;
        Mock<StatusbarMessageEvent> _statusbarMessageEvent;
        Mock<FileLoadedEvent> _fileLoadedEvent;
    }

    /*


    [Subject(typeof (TextEditorViewModel))]
    internal class When_loaded_was_published
    {
        Establish context = () =>
        {
            _eventAggregator = new Mock<IEventAggregator>();
            _sut = new TextEditorViewModel(_eventAggregator.Object);
            _statusbarEvent = new Mock<StatusbarMessageEvent>();
            _eventAggregator.Setup(x => x.GetEvent<StatusbarMessageEvent>()).Returns(_statusbarEvent.Object);
            _fileLoadedEvent = new Mock<FileLoadedEvent>();
            _eventAggregator.Setup(x => x.GetEvent<FileLoadedEvent>()).Returns(_fileLoadedEvent.Object);
        };

        Because of = () =>
        {
            _sut.OnLoadFile(@"data\SimpleGraph");
        };


        static Mock<IEventAggregator> _eventAggregator;
        static TextEditorViewModel _sut;
        static Mock<StatusbarMessageEvent> _statusbarEvent;
        static Mock<FileLoadedEvent> _fileLoadedEvent;
    }
    */
}
