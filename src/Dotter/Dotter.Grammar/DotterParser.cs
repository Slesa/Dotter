using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Dotter.Grammar
{
    public class DotterParser
    {
        readonly IHandleParserErrors _errorHandler;

        public DotterParser(IHandleParserErrors errorHandler)
        {
            _errorHandler = errorHandler;
        }

        public bool ParseValid(string text)
        {
            //var inputStream = new StreamReader(Console.OpenStandardInput());
            var input = new AntlrInputStream(text);
            var lexer = new DOTLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new DOTParser(tokens);
            parser.RemoveErrorListeners();

            var errorListener = new ErrorListener(_errorHandler);
            parser.AddErrorListener(errorListener);

            IParseTree tree = parser.graph();
            var tmp = tree.ToStringTree(parser);
//            Console.WriteLine(tree.ToStringTree(parser));
//            CalculatorVisitor visitor = new CalculatorVisitor();
//            Console.WriteLine(visitor.Visit(tree));

            return !errorListener.HasParserErrors;
        } 
    }
}