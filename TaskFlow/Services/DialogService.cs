using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Services
{
    public class DialogService : IDialogService
    {
        public Task<bool> ShowConfirmationAsync(string title, string message)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, "Ok", "Cancel");
        }

        public Task<string> DisplayPromptAsync(string title, string message)
        {
            return Application.Current.MainPage.DisplayPromptAsync(title, message, "Ok", "Cancel");
        }
    }
}
