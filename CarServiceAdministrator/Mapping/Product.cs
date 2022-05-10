using NHibernate.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceAdministrator.Mapping
{
    public class Product
    {
        [Id(0, Name = "ID")]
        [Generator(1, Class = "native")]
        public virtual int ID { get; set; }
        [Property]
        public virtual string ProductNo { get; set; }
        [Property]
        public virtual int Quantity { get; set; }
        [Property]
        public virtual string Description { get; set; }
    }
}
