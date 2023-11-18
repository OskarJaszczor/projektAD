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
    /// Logika interakcji dla klasy Lobby.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        public string? log;
        public Lobby()
        {
            InitializeComponent();
            
            log = MainWindow.log;

            serverListBox.Items.Add("1");
            serverLeaderBoard.Items.Add(log);
            
        }
    }
}
