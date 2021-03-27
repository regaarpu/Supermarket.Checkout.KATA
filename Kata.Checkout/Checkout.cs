using Kata.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kata.Checkout
{    
    public class Checkout
    {
        private static List<Items> itemList = new List<Items>();
        private static List<SpecialOffer> specialOfferList = new List<SpecialOffer>();

        private List<Items> scannedProductList = new List<Items>();

        private decimal CheckoutTotalPrice { get; set; }
        public Checkout()
        {
            initializeSKUProducts();
        }

        public decimal ScanProductNGetTotal(List<string> currentScanProductList)
        {
            if (currentScanProductList != null)
            {
                foreach(var product in currentScanProductList)
                {
                    //Calculate Product Checkout Total
                    CalculateProductCheckoutTotal(product);
                }
            }
            return CheckoutTotalPrice;
        }

        private void initializeSKUProducts()
        {
            string jsonPath = @"C:\Work\Supermarket.Checkout.KATA\Kata.Checkout";
            if (itemList.Count.Equals(0))
            {
                string itemJson = File.ReadAllText(jsonPath + @"\items.json");
                itemList = JsonConvert.DeserializeObject<List<Items>>(itemJson);

                string specialOfferJson = File.ReadAllText(jsonPath + @"\specialOffer.json");
                specialOfferList = JsonConvert.DeserializeObject<List<SpecialOffer>>(specialOfferJson);
            }
        }

        private void CalculateProductCheckoutTotal(string product)
        {            
            CheckoutTotalPrice += getProductUnitPrice(product);
        }

        private decimal getProductUnitPrice(string product)
        {
            return itemList.Find(x => x.ProductCode == product).UnitPrice;
        }
    }
}
