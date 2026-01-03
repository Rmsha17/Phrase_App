namespace Phrase_App.Core.Interfaces
{
    public interface ILogService
    {
        Task<bool> LogErrorAsync(ErrorLog log);
    }
}
