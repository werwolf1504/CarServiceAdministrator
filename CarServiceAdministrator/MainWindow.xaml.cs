using CarServiceAdministrator.Mapping;
using CarServiceAdministrator.Windows;
using NHB;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
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

namespace CarServiceAdministrator
{
    /// <summary>
    /// Interaction logic for LoginWidnow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Login> loginList = new List<Login>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            using (var session = NHibernateSessionFactory.OpenSession())
            {
                string query = "select * from Login";
                loginList = session.CreateSQLQuery(query).SetResultTransformer(new AliasToBeanResultTransformer(typeof(Login))).List<Login>().ToList();

                if (loginList.Count == 0)
                    MessageBox.Show("Do not found any logins");
            }

            string userName = UserNameBox.Text;
            string password = PasswordBox.Password;

            if(loginList.Any(x=> x.UserName == userName && x.Password ==  password))
            {
                WorkerWindow workerWindow = new WorkerWindow(loginList.First(x => x.UserName == userName && x.Password == password));
                if (loginList.First(x => x.UserName == userName && x.Password == password).RoleID == 2)
                {
                    workerWindow.AddUserButton.Visibility = Visibility.Hidden;
                    workerWindow.DeleteUserButton.Visibility = Visibility.Hidden;
                }
                workerWindow.Show();
                this.Visibility = Visibility.Collapsed;
            }
            else if(loginList.Any(x => x.UserName != userName || x.Password != password))
                ErrorTextBlock.Visibility = Visibility.Visible;
        }
    }
}

namespace NHB
{
    public class NHibernateSessionFactory
    {

        private static ISessionFactory sessionFactory = null;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    Configuration configuration = new Configuration();
                    string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "hibernate.cfg.xml");
                    configuration.Configure(path);
                    configuration.AddAssembly(Assembly.GetCallingAssembly());
                    sessionFactory = configuration.BuildSessionFactory();
                }
                return sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
        public static IStatelessSession OpenStatelessSession()
        {
            return SessionFactory.OpenStatelessSession();
        }
    }
    public static class NHibernateTransformers
    {
        public static readonly IResultTransformer ExpandoObject;

        static NHibernateTransformers()
        {
            ExpandoObject = new ExpandoObjectResultSetTransformer();
        }

        private class ExpandoObjectResultSetTransformer : IResultTransformer
        {
            public IList TransformList(IList collection)
            {
                return collection;
            }

            public object TransformTuple(object[] tuple, string[] aliases)
            {
                var expando = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expando;
                for (int i = 0; i < tuple.Length; i++)
                {
                    string alias = aliases[i];
                    if (alias != null)
                    {
                        dictionary[alias] = tuple[i];
                    }
                }
                return expando;
            }
        }
    }

    public static class NHibernateExtensions
    {
        public static IList<dynamic> DynamicList(this NHibernate.IQuery query)
        {
            return query.SetResultTransformer(NHibernateTransformers.ExpandoObject)
                        .List<dynamic>();
        }
    }
}
