using Dziekanat.ViewModel;
using System.Windows;

namespace Dziekanat.Views
{
    /// <summary>
    /// Interaction logic for StudentView.xaml
    /// </summary>
    public partial class StudentView : Window
    {
        public StudentView(string UserId)
        {
            InitializeComponent();
            var studentViewModel = new StudentViewModel(UserId);
            this.DataContext = studentViewModel;
        }
    }
}
