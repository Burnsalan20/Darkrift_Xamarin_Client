using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FirstXamarinProject.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public static AboutViewModel instance;

        public AboutViewModel()
        {
            instance = this;
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
        }

        public ICommand OpenWebCommand { get; }

        public void SetTitle(String title)
        {
            Title = title;
        }
    }
}