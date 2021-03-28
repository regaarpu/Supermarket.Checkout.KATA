using Kata.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Kata.Checkout
{
    public class Checkout : ICheckout
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentScanProductList"></param>
        /// <returns></returns>
        public override decimal ScanProductNGetTotal(List<string> currentScanProductList)
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

        #region PRIVATE METHODS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        private void CalculateProductCheckoutTotal(string product)
        {
            //Add the unit price of product
            decimal productUnitPrice = getProductUnitPrice(product);
            if (productUnitPrice.Equals(0)) return;

            CheckoutTotalPrice += getProductUnitPrice(product);

            //Check & Calculate, if any offer exists for this product
            CalculateProductOffer(product);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private decimal getProductUnitPrice(string product)
        {
            if (itemList.Exists(x => x.ProductCode == product))
            {
                return itemList.Find(x => x.ProductCode == product).UnitPrice;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        private void CalculateProductOffer(string product)
        {
            int currentProductOfferQuantity;

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
                        ApplyDiscount(product);
                    }
                }
                else
                {
                    offerProductCountList.Add(new OfferProductCount { ProductCode = product, Count = 1 });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        private void ApplyDiscount(string product)
        {
            //Apply Discount
            CheckoutTotalPrice += specialOfferList.Find(x => x.ProductCode == product).OfferPrice;

            //Remove the UNIT price for the product after applying discount
            CheckoutTotalPrice -= getProductUnitPrice(product) * specialOfferList.Find(x => x.ProductCode == product).OfferQuantity;
        }

        /// <summary>
        /// 
        /// </summary>
        private void initializeSKUProducts()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var initJsonPath = Convert.ToString(config["initJsonPath"]);

            if (itemList.Count.Equals(0))
            {
                string itemJson = File.ReadAllText(initJsonPath + @"\items.json");
                itemList = JsonConvert.DeserializeObject<List<Items>>(itemJson);

                string specialOfferJson = File.ReadAllText(initJsonPath + @"\specialOffer.json");
                specialOfferList = JsonConvert.DeserializeObject<List<SpecialOffer>>(specialOfferJson);
            }
        }
        #endregion
    }
}
