using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CEN207_Assessment_2.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private string displayName = "";

        public LoginPage()
        {
            InitializeComponent();
           displayName_TXT.Text = App.getDisplayName();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            displayName = displayName_TXT.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.setDisplayName(displayName);
        }
    }
}
