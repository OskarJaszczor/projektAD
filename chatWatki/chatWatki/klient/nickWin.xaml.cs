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
using System.Windows.Shapes;


namespace klient
{
    /// <summary>
    /// Logika interakcji dla klasy nickWin.xaml
    /// </summary>
    
    public partial class nickWin : Window
    {
        public string Login { get; private set; } = "anonymous";

        public nickWin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login = loginTxt.Text;
            DialogResult = true;
            Visibility = Visibility.Hidden;
            Lobby lobby = new Lobby();

            if (lobby.ShowDialog() == true)
            {
                lobby.Show();
            }
        }
    }
}
