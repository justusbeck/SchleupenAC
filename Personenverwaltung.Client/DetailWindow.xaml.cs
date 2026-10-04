using System.Windows;

namespace Personenverwaltung.Client
{
    public partial class DetailWindow : Window
    {
        public DetailWindow()
        {
            InitializeComponent();
        }

        public void ShowPersonDetails(PersonDetailDto personDetail)
        {
            DataContext = personDetail;
        }
    }
}