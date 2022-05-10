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
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
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
                Login login = new Login();
                login.UserName = UserNameTextBox.Text;
                login.Password = PasswordBox.Password;
                login.RoleID = 2;

                string query = "select * from Login";
                session.Save(login);
                
                List<Login> loginList = session.CreateSQLQuery(query).SetResultTransformer(new AliasToBeanResultTransformer(typeof(Login))).List<Login>().ToList();
                login.ID = loginList.First(x=>x.UserName == login.UserName && x.Password == login.Password).ID;

                Users userNew = new Users() { LoginID = login.ID, FirstName = FirstNameTextBox.Text, LastName = LastNameTextBox.Text, Email = EmailTextBox.Text, Phone = PhoneTextBox.Text };
                session.Save(userNew);

                Close();
                //if (usersList.Count == 0)
                //    MessageBox.Show("Do not found any logins");
                //UserDataGrid.ItemsSource = usersList;
            }
        }
    }
}
