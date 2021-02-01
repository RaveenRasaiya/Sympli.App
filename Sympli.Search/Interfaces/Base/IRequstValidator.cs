namespace Sympli.Search.Interfaces
{
    public interface IRequstValidator<T> where T : class
    {
        string IsValid(T request);
    }
}