namespace Dotter.Grammar
{
    public interface IHandleParserErrors
    {
        void HandleError(ErrorDescription error);
    }
}