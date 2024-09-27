using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskFlow.ViewModels;

namespace TaskFlow
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            
            BindingContext = vm;
        }

        public void Test()
        {
            //this.dis
        }

        private void Entry_Completed(object sender, EventArgs e)
        {

        }

        private void Entry_Completed_1(object sender, EventArgs e)
        {

        }
    }
}
