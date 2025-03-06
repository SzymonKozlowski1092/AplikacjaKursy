using Dziekanat.ViewModel;
using System.Windows;
namespace Dziekanat.Views
{
    /// <summary>
    /// Interaction logic for LecturerView.xaml
    /// </summary>
    public partial class LecturerView : Window
    {
        public LecturerView(string UserId)
        {
            InitializeComponent();
            var lecturerViewModel = new LecturerViewModel(UserId);
            this.DataContext = lecturerViewModel;
        }
    }
}
