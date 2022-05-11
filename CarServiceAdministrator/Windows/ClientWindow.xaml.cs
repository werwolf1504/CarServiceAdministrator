using CarServiceAdministrator.Mapping;
using NHB;
using NHibernate.Transform;
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

namespace CarServiceAdministrator.Windows
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            using (var session = NHibernateSessionFactory.OpenSession())
            {
                Client client = new Client
                {
                    Name = NameClientTextBlock.Text,
                    Surname = SurnameClientTextBlock.Text,
                    Phone = PhoneClientTextBlock.Text,
                    Email = EmailClientTextBlock.Text,
                    Car = CarClientTextBlock.Text,
                    CarType = CarTypeClientTextBlock.Text,
                    WinNo = WinNoClientTextBlock.Text,
                    TableNo = TableNoClientTextBlock.Text,
                };
                session.Save(client);

                Close();
            }
        }
    }
}
