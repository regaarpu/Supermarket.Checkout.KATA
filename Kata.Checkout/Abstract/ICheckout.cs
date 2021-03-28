using Kata.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kata.Checkout
{
    public abstract class ICheckout
    {

        public abstract decimal ScanProductNGetTotal(List<string> currentScanProductList);
    }
}
