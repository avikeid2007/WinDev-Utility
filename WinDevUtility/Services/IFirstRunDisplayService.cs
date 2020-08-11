using System.Threading.Tasks;

namespace WinDevUtility.Services
{
    public interface IFirstRunDisplayService
    {
        Task ShowIfAppropriateAsync();
    }
}
