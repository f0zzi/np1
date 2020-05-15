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
using System.Net;
using System.Net.Sockets;
using System.Collections.ObjectModel;
using server;

namespace np1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Person> persons;
        public MainWindow()
        {
            InitializeComponent();
            persons = new ObservableCollection<Person>();
            lbPersons.ItemsSource = persons;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(IPAddress.Parse("127.0.0.1"), 2020);
            if (client.Connected && !String.IsNullOrWhiteSpace(tbSearch.Text))
            {
                byte[] data = Encoding.UTF8.GetBytes(tbSearch.Text);
                client.Send(data);
                const int SIZE = 1024;
                byte[] response = new byte[SIZE];
                int count = client.Receive(response);
                Person[] resp = Formatter.Deserialize(response);
                persons.Clear();
                foreach (var item in resp)
                {
                    persons.Add(item);
                }
                tbSearch.Clear();
                client.Close();
            }
        }
    }
}
