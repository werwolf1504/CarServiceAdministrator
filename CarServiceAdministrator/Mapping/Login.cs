using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.Attributes;

namespace CarServiceAdministrator.Mapping
{
    [Class]
    public class Login
    {
        public Login()
        {
        }
        [Id(0, Name = "ID")]
        [Generator(1, Class = "native")]
        public virtual int ID { get; set; }
        [Property]
        public virtual string UserName { get; set; }
        [Property]
        public virtual string Password { get; set; }
        [Property]
        public virtual int RoleID { get; set; }
    }

    //public class LoginMapping : ClassMapping<Login>
    //{
    //    public LoginMapping()
    //    {
    //        Table("Login");
    //        Id(x => x.ID, map => map.Generator(Generators.Native));
    //        Property(x => x.UserName);
    //        Property(x => x.Password);
    //        Property(x => x.RoleID);
    //    }
    //}
}
