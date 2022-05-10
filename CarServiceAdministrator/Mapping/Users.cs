using NHibernate.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceAdministrator.Mapping
{
    [Class]
    public class Users
    {
        [Id(0, Name = "ID")]
        [Generator(1, Class = "native")]
        public virtual int ID { get; set; }
        [Property]
        public virtual string FirstName { get; set; }
        [Property]
        public virtual string LastName { get; set; }
        [Property]
        public virtual string Email { get; set; }
        [Property]
        public virtual string Phone { get; set; }
        [Property]
        public virtual int LoginID { get; set; }

    }
}
