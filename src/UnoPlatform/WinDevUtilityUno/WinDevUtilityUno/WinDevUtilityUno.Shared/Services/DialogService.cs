﻿using System;
using System.Threading.Tasks;

using Windows.UI.Popups;

namespace WinDevUtilityUno.Services
{
    public class DialogService : IDialogService
    {
        public async Task AlertAsync(string content, string title)
        {
            await new MessageDialog(content, title).ShowAsync();
        }

        public async Task AlertAsync(string content)
        {
            await new MessageDialog(content).ShowAsync();
        }
    }

    public interface IDialogService
    {
        Task AlertAsync(string content, string title);
        Task AlertAsync(string content);
    }
}
