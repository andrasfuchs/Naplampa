using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MagmaLightWeb.NaplampaService;
using System.Web.UI;
using MagmaLightWeb.Common;
using System.Threading;
using System.Text;

namespace MagmaLightWeb.Common
{
    public class CacheManager
    {
        private System.Web.Caching.Cache cache = null;
        private System.Web.SessionState.HttpSessionState session = null;

        public CacheManager(System.Web.Caching.Cache cache, System.Web.SessionState.HttpSessionState session)
        {
            this.cache = cache;
            this.session = session;
        }

        public Currency[] CheckAndLoadCurrencies(string defaultCurrencyISO)
        {
            Currency[] currencyList = null;


            lock (cache)
            {
                if (cache["Currencies"] == null)
                {
                    cache.Insert("Currencies", ServiceManager.NaplampaService.ListCurrencies());
                }

                if (session["CurrencyList"] == null)
                {
                    currencyList = Cloner.CloneAll<Currency>((Currency[])cache["Currencies"]);

                    foreach (Currency currency in currencyList)
                    {
                        currency.Name = Resources.Currencies.ResourceManager.GetString(currency.Name);
                    }

                    for (int i = 0; i < currencyList.Length; i++)
                    {
                        if (defaultCurrencyISO == currencyList[i].ISO)
                        {
                            Currency temp = currencyList[0];
                            currencyList[0] = currencyList[i];
                            currencyList[i] = temp;
                        }
                    }

                    session["CurrencyList"] = currencyList;
                }
                else
                {
                    currencyList = (Currency[])session["CurrencyList"];
                }
            }

            return currencyList;
        }

        public Product[] CheckAndLoadProducts()
        {
            Product[] productList = null;

            lock (cache)
            {
                if (cache["Products"] == null)
                {
                    cache.Insert("Products", ServiceManager.NaplampaService.ListProducts());
                }

                Product[] products = (Product[])cache["Products"];

                if (session["ProductList"] == null)
                {
                    productList = Cloner.CloneAll<Product>((Product[])cache["Products"]);

                    foreach (Product product in productList)
                    {
                        product.Name = Resources.Products.ResourceManager.GetString(product.Name);
                        product.Description = Resources.Products.ResourceManager.GetString(product.Description);
                    }
                }
                else
                {
                    productList = (Product[])session["ProductList"];
                }
            }

            return productList;
        }

        public Country[] CheckAndLoadCountries()
        {
            Country[] countryList = null;

            lock (cache)
            {
                if (cache["Countries"] == null)
                {
                    cache.Insert("Countries", ServiceManager.NaplampaService.ListCountries());
                }

                if (session["CountryList"] == null)
                {
                    countryList = Cloner.CloneAll<Country>((Country[])cache["Countries"]);

                    foreach (Country country in countryList)
                    {
                        country.Name = Resources.Countries.ResourceManager.GetString(country.Name);
                    }

                    for (int i = 1; i < countryList.Length; i++)
                    {
                        if (
                            (session["Country"] != null) && (((Country)session["Country"]).CountryId == countryList[i].CountryId)
                            || (session["Country"] == null) && (countryList[i].DefaultCultureName == Thread.CurrentThread.CurrentCulture.Name)
                            )
                        {
                            Country temp = countryList[0];
                            countryList[0] = countryList[i];
                            countryList[i] = temp;
                        }
                    }

                    session["CountryList"] = countryList;
                }
                else
                {
                    countryList = (Country[])session["CountryList"];
                }
            }

            return countryList;
        }

        public ProductDiscountPrice[] CheckAndLoadProductPrices(Product[] products, int currencyId, int countryId)
        {
            string pricesCacheKey = "Prices_" + currencyId + "_" + countryId;
            ProductDiscountPrice[] prices = null;


            lock (cache)
            {
                if ((cache[pricesCacheKey] == null)
                    || (cache[pricesCacheKey + "_TimeInserted"] == null)
                    || (((DateTime)cache[pricesCacheKey + "_TimeInserted"]) < DateTime.UtcNow.AddHours(-1)))
                {
                    prices = ServiceManager.NaplampaService.GetEffectiveProductPrices(products, currencyId, null, countryId, DateTime.UtcNow);

                    if (cache[pricesCacheKey] == null)
                    {
                        cache.Insert(pricesCacheKey, prices);
                        cache.Insert(pricesCacheKey + "_TimeInserted", DateTime.UtcNow);
                    }
                    else
                    {
                        cache[pricesCacheKey] = prices;
                        cache[pricesCacheKey + "_TimeInserted"] = DateTime.UtcNow;
                    }
                }
                else
                {
                    prices = (ProductDiscountPrice[])cache[pricesCacheKey];
                }
            }

            return prices;
        }

        public OrderCosts CheckAndLoadOrderCosts(int? parnerId, int countryId, short paymentMethod, ProductQuantity[] basket, int invoiceCurrencyId, string couponCode)
        {
            OrderCosts result = null;

            StringBuilder key = new StringBuilder();
            key.Append(parnerId);
            key.Append('_');
            key.Append(countryId);
            key.Append('_');
            key.Append(paymentMethod);
            key.Append('_');
            key.Append(invoiceCurrencyId);
            key.Append('|');
            foreach (ProductQuantity pq in basket)
            {
                key.Append('[');
                key.Append(pq.ProductId);
                key.Append('_');
                key.Append(pq.Quantity);
                key.Append(']');
            }
            key.Append('|');
            key.Append(couponCode);

            string orderCostsCacheKey = key.ToString();

            if ((cache[orderCostsCacheKey] == null)
                || (cache[orderCostsCacheKey + "_TimeInserted"] == null)
                || (((DateTime)cache[orderCostsCacheKey + "_TimeInserted"]) < DateTime.UtcNow.AddHours(-1)))
            {                
                result = ServiceManager.NaplampaService.CalculateOrderCosts(null, countryId, paymentMethod, basket, invoiceCurrencyId, couponCode);

                cache[orderCostsCacheKey] = result;
                cache[orderCostsCacheKey + "_TimeInserted"] = DateTime.UtcNow;
            }
            else
            {
                result = (OrderCosts)cache[orderCostsCacheKey];
            }

            return result;
        }

        public void CultureChanged()
        {
            lock (session)
            {
                session.Remove("CurrencyList");
                session.Remove("CountryList");
                session.Remove("ProductList");
            }
        }
    }
}
