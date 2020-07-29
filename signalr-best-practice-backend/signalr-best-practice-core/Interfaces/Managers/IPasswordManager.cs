namespace signalr_best_practice_core.Interfaces.Managers
{
    public interface IPasswordManager
    {
        string GetHash(string value);
    }
}
