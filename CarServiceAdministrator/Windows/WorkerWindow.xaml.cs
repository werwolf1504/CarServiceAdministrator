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
        Product currentProduct;
        Client currentClient;
        List<Users> usersList;
        List<Product> productsList;
        List<Client> clientsList;

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

                query = "select * from Product";
                productsList = session.CreateSQLQuery(query).SetResultTransformer(new AliasToBeanResultTransformer(typeof(Product))).List<Product>().ToList();

                query = "select * from Client";
                clientsList = session.CreateSQLQuery(query).SetResultTransformer(new AliasToBeanResultTransformer(typeof(Client))).List<Client>().ToList();

                //if (usersList.Count == 0)
                //    MessageBox.Show("Do not found any Product");


                Users currentUser = usersList.First(x=>x.LoginID == login.ID);
                UserDataGrid.ItemsSource = usersList.Where(x => x.ID != currentUser.ID);
                ProductDataGrid.ItemsSource = productsList;
                ClientDataGrid.ItemsSource = clientsList;
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
            }
        }

        private void RefreshProductButton_Click(object sender, RoutedEventArgs e)
        {
            productsList = new List<Product>();
            using (var session = NHibernateSessionFactory.OpenSession())
            {
                string query = "select * from Product";
                productsList = session.CreateSQLQuery(query).SetResultTransformer(new AliasToBeanResultTransformer(typeof(Product))).List<Product>().ToList();
                ProductDataGrid.ItemsSource = productsList;
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            using (var session = NHibernateSessionFactory.OpenSession())
            {
                Product product = new Product();
                product.ProductNo = PartNumberTextBlock.Text;
                if(int.TryParse(QuantityTextBlock.Text, out int quantity))
                    product.Quantity = quantity;
                else
                    product.Quantity = 0;
                product.Description = DescriptionTextBlock.Text;

                session.Save(product);
                string query = "select * from Product";
                productsList = session.CreateSQLQuery(query).SetResultTransformer(new AliasToBeanResultTransformer(typeof(Product))).List<Product>().ToList();
                ProductDataGrid.ItemsSource = productsList;
            }
        }

        private void ProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentProduct = ProductDataGrid.SelectedItem as Product;
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            using (var session = NHibernateSessionFactory.OpenSession())
            {
                if (currentProduct != null)
                {
                    var deleteProduct = session.Get<Product>(currentProduct.ID);
                    session.Delete(deleteProduct);
                    session.Flush();

                    string query = "select * from Product";
                    productsList = session.CreateSQLQuery(query).SetResultTransformer(new AliasToBeanResultTransformer(typeof(Product))).List<Product>().ToList();
                    ProductDataGrid.ItemsSource = productsList;
                }
            }
        }

        private void RefreshClientButton_Click(object sender, RoutedEventArgs e)
        {
            using (var session = NHibernateSessionFactory.OpenSession())
            {
                string query = "select * from Client";
                clientsList = session.CreateSQLQuery(query).SetResultTransformer(new AliasToBeanResultTransformer(typeof(Client))).List<Client>().ToList();

                ClientDataGrid.ItemsSource = clientsList;
            }
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            ClientWindow clientWindow = new ClientWindow();
            clientWindow.Show();
        }

        private void ClientDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentClient = ClientDataGrid.SelectedItem as Client;
            if (currentClient != null)
            {
                CarTextBlock.Text = currentClient.Car;
                CarTypeTextBlock.Text = currentClient.CarType;
                WinNoTextBlock.Text = currentClient.WinNo;
                TableNoTextBlock.Text = currentClient.TableNo;
            }
            else
            {
                CarTextBlock.Text = "";
                CarTypeTextBlock.Text = "";
                WinNoTextBlock.Text = "";
                TableNoTextBlock.Text = "";
            }
        }

        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            using (var session = NHibernateSessionFactory.OpenSession())
            {
                if (currentClient != null)
                {
                    var deleteClient = session.Get<Client>(currentClient.ID);
                    session.Delete(deleteClient);
                    session.Flush();

                    string query = "select * from Client";
                    clientsList = session.CreateSQLQuery(query).SetResultTransformer(new AliasToBeanResultTransformer(typeof(Client))).List<Client>().ToList();

                    ClientDataGrid.ItemsSource = clientsList;
                }
            }
        }
    }
}
