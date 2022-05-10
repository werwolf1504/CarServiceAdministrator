using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceAdministrator.Mapping
{
    public class Login
    {
        public Login()
        {
        }

        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }

    }
}
