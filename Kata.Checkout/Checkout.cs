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
        private List<OfferProductCount> offerProductCountList = new List<OfferProductCount>();

        private decimal CheckoutTotalPrice { get; set; }
        public Checkout()
        {
            initializeSKUProducts();
        }

        public decimal ScanProductNGetTotal(List<string> currentScanProductList)
        {
            if (currentScanProductList != null)
            {
                foreach (var product in currentScanProductList)
                {
                    //Calculate Product Checkout Total
                    CalculateProductCheckoutTotal(product);
                }
            }
            return CheckoutTotalPrice;
        }

        private void initializeSKUProducts()
        {
            string jsonPath = @"C:\Work\KATA\Supermarket.Checkout.KATA\Kata.Checkout";
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
            int currentProductOfferQuantity;
            
            //Add the unit price of product
            CheckoutTotalPrice += getProductUnitPrice(product);

            //Check if any offer exists for this product
            int specialOfferIndex = specialOfferList.FindIndex(item => item.ProductCode == product);
            if (specialOfferIndex >= 0)
            {
                //If Yes - Offer exists, then add into the offer product list 
                int offerProductListIndex = offerProductCountList.FindIndex(item => item.ProductCode == product);
                if (offerProductListIndex >= 0)
                {
                    //If already the current product is in the list, then just increment the count
                    offerProductCountList.Find(x => x.ProductCode == product).Count++;

                    //Check if offer discount applicable in current product scan
                    currentProductOfferQuantity = specialOfferList.Find(x => x.ProductCode == product).OfferQuantity;
                    if (offerProductCountList.Find(x => x.ProductCode == product).Count == currentProductOfferQuantity)
                    {
                        //Apply Discount
                        CheckoutTotalPrice += specialOfferList.Find(x => x.ProductCode == product).OfferPrice;
                        CheckoutTotalPrice -= getProductUnitPrice(product) * specialOfferList.Find(x => x.ProductCode == product).OfferQuantity;
                    }
                }
                else
                {
                    offerProductCountList.Add(new OfferProductCount { ProductCode = product, Count = 1 });
                }
            }
        }

        private decimal getProductUnitPrice(string product)
        {
            return itemList.Find(x => x.ProductCode == product).UnitPrice;
        }
    }
}
