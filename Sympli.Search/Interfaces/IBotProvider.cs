namespace Sympli.Search.Interfaces
{
    public interface IBotProvider
    {
        public IBotService GetBotService(string searchEngine);
    }
}
