using System;
using NHibernate.Mapping.Attributes;

namespace Leprechaun.BitStamp.DAL
{
    [Class(Table = "BTCRates")]
    public class BTCRate
    {
        [Id(Name = "ID")]
        [Generator(1, Class = "guid")]
        public Guid ID { get; set; }

        [Property(Column = "Date")]
        public DateTime Date { get; set; }

        [Property(Column = "Last")]
        public double Last { get; set; }

        [Property(Column = "Highest")]
        public double Highest { get; set; }

        [Property(Column = "Lowest")]
        public double Lowest { get; set; }

        [Property(Column = "Volume")]
        public double Volume { get; set; }

        [Property(Column = "Bid")]
        public double Bid { get; set; }

        [Property(Column = "Ask")]
        public double Ask { get; set; }
    }
}
