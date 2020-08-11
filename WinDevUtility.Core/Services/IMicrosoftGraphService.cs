using System;
using System.Threading.Tasks;

using WinDevUtility.Core.Models;

namespace WinDevUtility.Core.Services
{
    public interface IMicrosoftGraphService
    {
        Task<User> GetUserInfoAsync(string accessToken);

        Task<string> GetUserPhoto(string accessToken);
    }
}
