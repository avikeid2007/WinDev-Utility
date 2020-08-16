using System.Threading.Tasks;

namespace WinDevUtility.Services
{
    public interface IDialogService
    {
        Task AlertAsync(string content, string title);
        Task AlertAsync(string content);
    }
}
