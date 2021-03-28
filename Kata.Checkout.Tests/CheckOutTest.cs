using Kata.Entity;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace Kata.Checkout.Tests
{
    public class CheckOutTest
    {
        private ICheckout checkout;

        public CheckOutTest()
        {
            checkout = new Checkout();
        }

        [Fact]
        public void Checkout_Pricing_AB_Only()
        {
            var productScanList = new List<string>()
            {
                "A99",
                "B15"
            };

            var totalPrice = checkout.ScanProductNGetTotal(productScanList);

            Assert.Equal(0.8M, totalPrice, 2);
        }

        [Fact]
        public void Checkout_Pricing_UNKNOWN_Handle_Exception()
        {
            var productScanList = new List<string>()
            {
                "A99",
                "UNKNOWN",
                "B15"
            };

            var totalPrice = checkout.ScanProductNGetTotal(productScanList);

            Assert.Equal(0.8M, totalPrice, 2);
        }

        [Fact]
        public void Checkout_UNKNOWN_Handle_Exception()
        {
            var productScanList = new List<string>()
            {
                "UNKNOWN"
            };

            var totalPrice = checkout.ScanProductNGetTotal(productScanList);

            Assert.Equal(0.0M, totalPrice, 2);
        }

        [Fact]
        public void Checkout_Pricing_ABAA_Only()
        {
            var productScanList = new List<string>()
            {
                "A99",
                "B15",
                "A99",
                "A99"
            };

            var totalPrice = checkout.ScanProductNGetTotal(productScanList);

            Assert.Equal(1.6M, totalPrice, 2);
        }

        [Fact]
        public void Checkout_Pricing_ABAACBBA_Only()
        {
            var productScanList = new List<string>()
            {
                "A99",
                "B15",
                "A99",
                "A99",
                "C40",
                "B15",
                "B15",
                "A99"
            };

            var totalPrice = checkout.ScanProductNGetTotal(productScanList);

            Assert.Equal(3.15M, totalPrice, 2);
        }

        [Fact]
        public void Checkout_Pricing_CCABCAABCAB_Only()
        {
            var productScanList = new List<string>()
            {
                "C40",
                "C40",
                "A99",
                "B15",
                "C40",
                "A99",
                "A99",
                "B15",
                "C40",
                "A99",
                "B15"                
            };

            var totalPrice = checkout.ScanProductNGetTotal(productScanList);

            Assert.Equal(4.95M, totalPrice, 2);
        }
    }
}
