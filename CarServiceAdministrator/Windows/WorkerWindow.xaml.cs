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
    /// Interaction logic for WorkerWindow.xaml
    /// </summary>
    public partial class WorkerWindow : Window
    {
        Login login;
        Users users;
        List<Users> usersList;

        public WorkerWindow(Login login)
        {
            this.login = login;
            InitializeComponent();
            usersList = new List<Users>();
            using (var session = NHibernateSessionFactory.OpenSession())
            {
                string query = "select * from Users";
                usersList = session.CreateSQLQuery(query).SetResultTransformer(new AliasToBeanResultTransformer(typeof(Users))).List<Users>().ToList();

                if (usersList.Count == 0)
                    MessageBox.Show("Do not found any logins");
                UserDataGrid.ItemsSource = usersList;
                Users currentUser = usersList.First(x=>x.LoginID == login.ID);
                NameTextBlock.Text = currentUser.FirstName;
                LastNameTextBlock.Text = currentUser.LastName;
                EmailTextBlock.Text = currentUser.Email;
                PhoneTextBlock.Text = currentUser.Phone;
            }
        }

        public WorkerWindow()
        {
            InitializeComponent();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            usersList = new List<Users>();
            using (var session = NHibernateSessionFactory.OpenSession())
            {
                string query = "select FirstName, LastName, Email, Phone, u.ID, u.LoginID from Users u join Login l on u.LoginID = l.ID where l.RoleID = 2";
                usersList = session.CreateSQLQuery(query).SetResultTransformer(new AliasToBeanResultTransformer(typeof(Users))).List<Users>().ToList();

                if (usersList.Count == 0)
                    MessageBox.Show("Do not found any logins");
                UserDataGrid.ItemsSource = usersList;
            }
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void UserDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            users = UserDataGrid.SelectedItem as Users;
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            using (var session = NHibernateSessionFactory.OpenSession())
            {
                //string query = "select * from Users u join Login l on u.LoginID = l.ID where l.RoleID = 2";
                //usersList = session.CreateSQLQuery(query).SetResultTransformer(new AliasToBeanResultTransformer(typeof(Users))).List<Users>().ToList();

                if (users != null)
                {
                    var deleteUser = session.Get<Users>(users.ID);
                    session.Delete(deleteUser);
                    session.Flush();

                    string query = "select FirstName, LastName, Email, Phone, u.ID, u.LoginID from Users u join Login l on u.LoginID = l.ID where l.RoleID = 2";
                    usersList = session.CreateSQLQuery(query).SetResultTransformer(new AliasToBeanResultTransformer(typeof(Users))).List<Users>().ToList();
                    if (usersList.Count == 0)
                        MessageBox.Show("Do not found any logins");
                    UserDataGrid.ItemsSource = usersList;
                }
                //if (usersList.Count == 0)
                //    MessageBox.Show("Do not found any logins");
                //UserDataGrid.ItemsSource = usersList;
            }
        }
    }
}
