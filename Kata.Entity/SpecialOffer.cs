using System;
using System.Collections.Generic;
using System.Text;

namespace Kata.Entity
{
    public class SpecialOffer : Product
    {
        public int? OfferQuantity { get; set; }
        public decimal? OfferPrice { get; set; }
    }
}
