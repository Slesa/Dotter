using Antlr4.Runtime;

namespace Dotter.Grammar
{
    public class ErrorDescription
    {
        public int Line { get; private set; }
        public int Column { get; set; }
        public string Message { get; private set; }

        public ErrorDescription(int line, int column, string message)
        {
            Line = line;
            Column = column;
            Message = message;
        }
    }

    public class ErrorListener : BaseErrorListener
    {
        public ErrorListener(IHandleParserErrors errorHandler)
        {
            ErrorHandler = errorHandler;
        }

        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
            RecognitionException e)
        {
            base.SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, msg, e);

            ErrorHandler.HandleError(new ErrorDescription(line, charPositionInLine, msg));
            HasParserErrors = true;
        }

        public IHandleParserErrors ErrorHandler { private get; set; }
        public bool HasParserErrors { get; private set; }
    }
}