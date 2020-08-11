using System;
using System.Threading.Tasks;

using WinDevUtility.ViewModels;

namespace WinDevUtility.Services
{
    public interface IUserDataService
    {
        event EventHandler<UserViewModel> UserDataUpdated;

        void Initialize();

        Task<UserViewModel> GetUserAsync();
    }
}
