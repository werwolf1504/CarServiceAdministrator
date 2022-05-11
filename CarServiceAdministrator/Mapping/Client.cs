using NHibernate.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceAdministrator.Mapping
{
    [Class]
    public class Client
    {
        [Id(0, Name = "ID")]
        [Generator(1, Class = "native")]
        public virtual int ID { get; set; }
        [Property]
        public virtual string Name { get; set; }
        [Property]
        public virtual string Surname { get; set; }
        [Property]
        public virtual string Phone { get; set; }
        [Property]
        public virtual string Email { get; set; }
        [Property]
        public virtual string Car { get; set; }
        [Property]
        public virtual string CarType { get; set; }
        [Property]
        public virtual string WinNo { get; set; }
        [Property]
        public virtual string TableNo { get; set; }
    }
}
