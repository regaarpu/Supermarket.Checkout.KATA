using Kata.Entity;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kata.Checkout.Tests
{
    public class CheckOutTest
    {
        private Checkout checkout;

        public CheckOutTest()
        {
            checkout = new Checkout();
        }

        [Fact]
        public void Checkout_Pricing_AB_Only()
        {
            var productScanList = new List<string>() { "A99", "B15" };

            var totalPrice = checkout.ScanProductNGetTotal(productScanList);


            Assert.Equal(0.8M, totalPrice, 2);
        }
    }
}
