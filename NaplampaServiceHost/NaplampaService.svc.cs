using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NaplampaService;
using NaplampaService.DataContract;
using NaplampaWcfHost;
using NaplampaWcfHost.DataContract;
using System.ServiceModel.Activation;
using System.Configuration;
using NaplampaWcfHost.Util;
using System.IO;
using System.Web.Hosting;
using System.Security.Cryptography;
using NaplampaWcfHost.PayPalService;
using System.Globalization;
using System.ComponentModel;
using System.Data.Objects;
using System.Text.RegularExpressions;
using System.Net;

namespace NaplampaService
{
    public class NaplampaService : INaplampaService
    {
        private NaplampaEntities context = new NaplampaEntities();
        private Random random = new Random((int)DateTime.Now.Ticks);
        private TimeSpan currencyRateTimeOut = new TimeSpan(5, 0, 0, 0);
        private bool emailServiceDisabled = false;

        #region INaplampaService Members

        public void EnableEmailService()
        {
            emailServiceDisabled = false;
        }

        public void DisableEmailService()
        {
            emailServiceDisabled = true;
        }

        public bool IsEmailServiceDisabled()
        {
            return emailServiceDisabled;
        }

        public Person Login(int personId, string password)
        {
            Person result = GetPersonById(personId);


            if (result != null)
            {
                byte[] buffer = Encoding.Default.GetBytes(password);
                SHA512CryptoServiceProvider cryptoTransformSHA512 = new SHA512CryptoServiceProvider();
                string hash = BitConverter.ToString(cryptoTransformSHA512.ComputeHash(buffer)).Replace("-", "");

                if (hash != result.PasswordHash) return null;
            }
            else
            {
                Person person = context.Person.Include("Base").FirstOrDefault(p => p.PersonId == personId);

                if (person != null)
                {
                    LogWarning(null, person.Base.PartnerId, "Login", "User '{0}' needs to confirm his/her e-mail. The confirmation e-mail is being sent.", personId);
                    context.SaveChanges();
                    SendEmail("CONFIRMEMAIL", person.CultureName, person.Email, new object[] { person.FirstName, person.Email, person.EmailConfirmationCode, person.PersonId });
                }
                else
                {
                    LogError(null, null, "Login", null, "User '{0}' was not found.", personId);
                    context.SaveChanges();
                }
            }

            return result;
        }

        public void ResetPassword(int personId)
        {
            Person person = GetPersonById(personId);

            if (person != null)
            {
                string password = GenerateRandomString(8, false);

                byte[] buffer = Encoding.Default.GetBytes(password);
                SHA512CryptoServiceProvider cryptoTransformSHA512 = new SHA512CryptoServiceProvider();
                string hash = BitConverter.ToString(cryptoTransformSHA512.ComputeHash(buffer)).Replace("-", "");

                person.PasswordHash = hash;
                person.PasswordExpiresOn = DateTime.UtcNow.Date;

                LogInformation(null, person.Base.PartnerId, "ResetPassword", "User '{0}' had his/her password reset.", personId);
                context.SaveChanges();

                SendEmail("PASSWORDRESET", person.CultureName, person.Email, new object[] { person.FirstName, password });
            }
            else
            {
                LogError(null, null, "ResetPassword", null, "User '{0}' was not found.", personId);
                context.SaveChanges();
            }
        }

        public int ConfirmEmail(string email, string confirmationCode)
        {
            Person person = context.Person.Include("Base").Where<Person>(p => p.Email == email).OrderByDescending<Person, int>(p => p.PersonId).FirstOrDefault<Person>();

            if (person == null) return 1;

            if (person.EmailConfirmed.HasValue) return 2;

            if (person.EmailConfirmationCode == confirmationCode)
            {
                person.EmailConfirmed = DateTime.UtcNow;
                LogInformation(null, person.Base.PartnerId, "ConfirmEmail", "User '{0}' confirmed his/her e-mail.", email);
                context.SaveChanges();
            }
            else
            {
                LogWarning(null, person.Base.PartnerId, "ConfirmEmail", "User '{0}' tried to confirm his/her e-mail, but the confirmation code was incorrect.", email);
                context.SaveChanges();
                return 3;
            }

            return 0;
        }

        public static Func<NaplampaEntities, string, Person> GetPersonQuery
            = CompiledQuery.Compile((NaplampaEntities context, string username)
                =>
                context.Person.Include("Base")
                .FirstOrDefault<Person>(p => p.Email == username && p.EmailConfirmed.HasValue)
        );


        private Person GetPerson(string username)
        {
            //Person result = context.Person.Include("Base").FirstOrDefault<Person>(p => p.Email == username && p.EmailConfirmed.HasValue);
            Person result = GetPersonQuery(context, username);

            //Person result = context.Person.FirstOrDefault<Person>(p => p.Email == username && p.EmailConfirmed.HasValue);
            //if (!result.BaseReference.IsLoaded) result.BaseReference.Load();

            return result;
        }

        public Person GetPersonById(int personId)
        {
            Person result = context.Person.Include("Base").FirstOrDefault<Person>(p => p.PersonId == personId);

            return result;
        }

        public void DeleteOrder(int orderId)
        {
            SetOrderStatus(orderId, 65536);
        }

        public void OrderPackageSent(int orderId, int sentByPersonId, DateTime sentOn, string trackingNumber)
        {
            Order order = GetOrder(orderId);

            Person sentBy = context.Person.First<Person>(p => p.PersonId == sentByPersonId);
            order.SentByReference.Value = sentBy;

            order.SentOn = sentOn;
            order.TrackingNumber = trackingNumber;

            SetOrderStatusInt(orderId, 2 + 16);

            StockItem stockItem = null;
            Partner naplampaPartner = context.Partner.Include("DeliveryAddress").First(p => p.RefererCode == "NAPLAMPA");

            foreach (OrderProductXRef opx in order.OrderProductXRefs)
            {
                stockItem = StockItem.CreateStockItem(0, opx.Quantity, DateTime.UtcNow);
                stockItem.Owner = order.Partner;
                stockItem.Address = order.DeliveryAddress;
                stockItem.Product = opx.Product;
                context.AddToStockItem(stockItem);

                stockItem = StockItem.CreateStockItem(0, -opx.Quantity, DateTime.UtcNow);
                stockItem.Owner = naplampaPartner;
                stockItem.Address = naplampaPartner.DeliveryAddress;
                stockItem.Product = opx.Product;
                context.AddToStockItem(stockItem);
            }
            context.SaveChanges();

            object[] parameters = new object[] 
            { 
                order.OrderId,
                order.Partner.ContactPerson.FirstName, 
                order.Currency.Prefix + order.OrderTotal.ToString("0.00") + " " + order.Currency.Postfix,
                order.PassCode,
                order.PayPalToken
            };

            SendEmail("PACKAGESENT", order.Partner.ContactPerson.CultureName, order.Partner.ContactPerson.Email, parameters);
        }

        public void OrderPaymentRequestSent(int orderId)
        {
            SetOrderStatus(orderId, 2);
        }

        public void OrderPaymentReceived(int orderId, string payPalToken)
        {
            Order order = context.Order.First(o => o.OrderId == orderId);

            if (order.PayPalToken != payPalToken) return;

            SetOrderStatus(orderId, 4 + 8);
        }

        public void OrderDelivered(int orderId, DateTime deliveredOn)
        {
            Order order = context.Order.First(o => o.OrderId == orderId);

            order.DeliveredOn = deliveredOn;
            SetOrderStatusInt(orderId, 32);

            context.SaveChanges();
        }

        public void OrderReturned(int orderId)
        {
            SetOrderStatus(orderId, (int)OrderStatus.Returned);
        }

        public void OrderResendPaymentRequest(int orderId)
        {
            Order order = GetOrder(orderId);

            object[] parameters = new object[] 
            { 
                order.OrderId,
                order.Partner.ContactPerson.FirstName, 
                order.Currency.Prefix + order.OrderTotal.ToString("0.00") + " " + order.Currency.Postfix,
                order.PassCode,
                order.PayPalToken
            };

            switch (order.PaymentMethod)
            {
                case 1:
                    SendEmail("ORDERNOTPAID_BANKTRANSFER", order.Partner.ContactPerson.CultureName, order.Partner.ContactPerson.Email, parameters);
                    break;

                case 2:
                case 3:
                    SendEmail("ORDERNOTPAID_PAYPAL", order.Partner.ContactPerson.CultureName, order.Partner.ContactPerson.Email, parameters);
                    break;

                case 4:
                    break;

                default:
                    break;
            }
        }

        public void SetOrderSellerComment(int orderId, string comment)
        {
            Order order = context.Order.First(o => o.OrderId == orderId);

            order.CommentBySeller = comment;
            context.SaveChanges();
        }

        public void SetOrderBuyerComment(int orderId, string comment)
        {
            Order order = context.Order.First(o => o.OrderId == orderId);

            order.CommentByBuyer = comment;
            context.SaveChanges();
        }

        public void SetOrderMigrationValues(int orderId, DateTime createdOn, decimal sum, int statusModifiedByPersonId, DateTime statusModifiedOn)
        {
            Order order = context.Order.Include("Partner").Include("Invoices").First(o => o.OrderId == orderId);

            order.Created = createdOn;
            order.Invoices.First().Subtotal = sum;
            order.Invoices.First().Total = sum;

            LogInformation(order.OrderId, order.Partner.PartnerId, "SetOrderMigrationValues", "The status of order {0} was modified by {1} on {2}. This information is lost during the migration process.", new object[] { orderId, statusModifiedByPersonId, statusModifiedOn });

            context.SaveChanges();
        }

        public int CreateNewPartner(string partnerName, string partnerPhone, string contactPersonTitle, string contactPersonFirstName, string contactPersonLastName, string contactPersonEmail, bool contactPersonNewsletter, string contactPersonCultureName)
        {
            Partner createdPartner = InsertPartner(partnerName, partnerPhone);
            Person contactPerson = InsertPerson(contactPersonTitle, contactPersonFirstName, contactPersonLastName, contactPersonEmail, partnerPhone, contactPersonNewsletter, contactPersonCultureName);

            try
            {
                createdPartner.ContactPersonReference.Value = contactPerson;
                LogInformation(null, createdPartner.PartnerId, "CreateNewPartner", "Partner named '{0}' now has the contact person '{1}'.", createdPartner.FullName, createdPartner.ContactPerson.LastName);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(null, null, "CreateNewPartner", ex, "Partner named '{0}' coundn't have the contact person '{1}'.", createdPartner.FullName, createdPartner.ContactPerson.LastName);
                context.SaveChanges();
            }

            return createdPartner.PartnerId;
        }

        public int CreateNewAddress(int parnerId, int countryId, string province, string town, string postalCode, string addressLine, string name)
        {
            Address address = InsertAddress(parnerId, countryId, province, town, postalCode, addressLine, name);
            context.SaveChanges();

            return address.AddressId;
        }

        public int CreateWarrantyOrder(int originalOrderId, List<ProductQuantity> basket)
        {
            if (basket.Count == 0) return -1;

            Order originalOrder = GetOrder(originalOrderId);

            Order newOrder = InsertOrder(originalOrder.Partner.PartnerId, originalOrder.DeliveryAddress.AddressId, 0, "", basket, originalOrder.Currency.CurrencyId, (int)(OrderStatus.WarrantyOrder | OrderStatus.OrderPlaced | OrderStatus.PaymentRequestSent | OrderStatus.PaymentReceived | OrderStatus.ReadyToPost), null);

            SetOrderStatusInt(originalOrderId, (int)OrderStatus.WarrantyRequest);
            context.SaveChanges();

            return newOrder.OrderId;

        }

        public int CreateNewOrderWithoutLogin(string title, string firstName, string lastName, string email, string phone, bool newsletter, string cultureName, int countryId, string province, string town, string postalCode, string addressLine, short paymentMethod, string referer, List<ProductQuantity> basket, int invoiceCurrencyId, int? invoiceCountryId, string invoiceProvince, string invoiceTown, string invoicePostalCode, string invoiceAddressLine, string invoiceFullName, int? initialStatus, string couponCode)
        {
            if (basket.Count == 0) return -1;

            Person newPerson = GetPerson(email);

            if (newPerson == null)
            {
                firstName = firstName.ToCamel();
                lastName = lastName.ToCamel();
                email = email.ToLower();
                //cultureName = cultureName.ToUpper(); // this one caused some trouble

                newPerson = InsertPerson(title, firstName, lastName, email, phone, newsletter, cultureName);
            }
            else
            {
                UpdateUserPreferences(newPerson.PersonId, title, firstName, lastName, email, phone, null, newsletter, cultureName);
            }

            if (!String.IsNullOrEmpty(phone))
            {
                newPerson.Base.Phone = phone;
            }

            if (newsletter)
            {
                newPerson.Newsletter = newsletter;
            }

            province = province.ToCamel();
            town = town.ToCamel();
            Address newAddress = InsertAddress(newPerson.Base.PartnerId, countryId, province, town, postalCode, addressLine, newPerson.Base.FullName);

            if (invoiceAddressLine != null)
            {
                Address billingAddress = InsertAddress(newPerson.Base.PartnerId, invoiceCountryId.Value, invoiceProvince, invoiceTown, invoicePostalCode, invoiceAddressLine, invoiceFullName);
                newPerson.Base.BillingAddressReference.Value = billingAddress;
            }
            else
            {
                newPerson.Base.BillingAddressReference.Value = newAddress;
            }

            newPerson.Base.DeliveryAddressReference.Value = newAddress;

            Order newOrder = InsertOrder(newPerson.Base.PartnerId, newAddress.AddressId, paymentMethod, referer, basket, invoiceCurrencyId, initialStatus, couponCode);
            context.SaveChanges();

            // process paypal and credit/debit card payments
            if ((paymentMethod == 2) || (paymentMethod == 3))
            {
                Invoice lastInvoice = newOrder.Invoices.OrderBy(i => i.DateOfInvoice).Last();
                lastInvoice = GetInvoice(lastInvoice.InvoiceId);

                PayPalAPIAAInterfaceClient payPalAPIAAInterfaceClient = new PayPalAPIAAInterfaceClient();


                CustomSecurityHeaderType requesterCredentials = new CustomSecurityHeaderType();
                SetExpressCheckoutReq req = new SetExpressCheckoutReq();
                req.SetExpressCheckoutRequest = new SetExpressCheckoutRequestType();
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails = new SetExpressCheckoutRequestDetailsType();
                SetExpressCheckoutResponseType res = null;


                // PayPal expects numeric values with US formatting
                CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                requesterCredentials.Credentials = new UserIdPasswordType();
                requesterCredentials.Credentials.Username = ConfigurationManager.AppSettings["PayPalUsername"];
                requesterCredentials.Credentials.Password = ConfigurationManager.AppSettings["PayPalPassword"];
                requesterCredentials.Credentials.Signature = ConfigurationManager.AppSettings["PayPalSignature"];

                req.SetExpressCheckoutRequest.Version = "74.0";
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.BuyerEmail = newOrder.Partner.ContactPerson.Email;
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.LocaleCode = newOrder.Partner.ContactPerson.CultureName.Substring(newOrder.Partner.ContactPerson.CultureName.IndexOf('-') + 1);
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.ReturnURL = ConfigurationManager.AppSettings["PayPalReturnURL"] + "?OrderID=" + newOrder.OrderId;
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.CancelURL = ConfigurationManager.AppSettings["PayPalCancelURL"] + "?PayPalSuccess=false&OrderID=" + newOrder.OrderId;

                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.BillingAgreementDetails = new BillingAgreementDetailsType[1];
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.BillingAgreementDetails[0] = new BillingAgreementDetailsType();
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.BillingAgreementDetails[0].BillingType = BillingCodeType.MerchantInitiatedBillingSingleAgreement;

                object orderCurreny = Enum.Parse(typeof(CurrencyCodeType), newOrder.Currency.ISO);
                object invoiceCurreny = Enum.Parse(typeof(CurrencyCodeType), lastInvoice.Currency.ISO);

                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails = new PaymentDetailsType[1];
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0] = new PaymentDetailsType();

                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].TaxTotal = new BasicAmountType();
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].TaxTotal.currencyID = (CurrencyCodeType)invoiceCurreny;
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].TaxTotal.Value = newOrder.VATTotal.ToString("0.00");

                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].OrderTotal = new BasicAmountType();
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].OrderTotal.currencyID = (CurrencyCodeType)invoiceCurreny;
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].OrderTotal.Value = newOrder.OrderTotal.ToString("0.00");

                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem = new PaymentDetailsItemType[lastInvoice.InvoiceLines.Count];

                int j = 0;
                foreach (InvoiceLine invoiceLine in lastInvoice.InvoiceLines.OrderBy<InvoiceLine, int>(il => il.LineNumber))
                {
                    req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j] = new PaymentDetailsItemType();
                    req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].Quantity = invoiceLine.Quantity.ToString();
                    req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].Name = Resources.Products.ResourceManager.GetString(invoiceLine.Description);

                    if (invoiceLine.Product != null)
                    {
                        req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].Number = invoiceLine.Product.Code;
                        req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].ItemWeight = new MeasureType();
                        req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].ItemWeight.unit = "g";
                        req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].ItemWeight.Value = invoiceLine.Product.Weight;

                        if (invoiceLine.Product.BoxWidth.HasValue)
                        {
                            req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].ItemWidth = new MeasureType();
                            req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].ItemWidth.unit = "mm";
                            req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].ItemWidth.Value = invoiceLine.Product.BoxWidth.Value;
                        }
                        if (invoiceLine.Product.BoxHeight.HasValue)
                        {
                            req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].ItemHeight = new MeasureType();
                            req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].ItemHeight.unit = "mm";
                            req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].ItemHeight.Value = invoiceLine.Product.BoxHeight.Value;
                        }
                        if (invoiceLine.Product.BoxLength.HasValue)
                        {
                            req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].ItemLength = new MeasureType();
                            req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].ItemLength.unit = "mm";
                            req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].ItemLength.Value = invoiceLine.Product.BoxLength.Value;
                        }
                    }

                    req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].Amount = new BasicAmountType();
                    req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].Amount.currencyID = (CurrencyCodeType)invoiceCurreny;
                    req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].Amount.Value = invoiceLine.UnitPrice.ToString("0.00");

                    req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].Tax = new BasicAmountType();
                    req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].Tax.currencyID = (CurrencyCodeType)invoiceCurreny;
                    req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].PaymentDetailsItem[j].Tax.Value = "0.00";

                    j++;
                }

                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].ShipToAddress = new AddressType();
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].ShipToAddress.Name = newOrder.DeliveryAddress.Name;
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].ShipToAddress.Street1 = newOrder.DeliveryAddress.AddressLine;

                object countryCode = Enum.Parse(typeof(CountryCodeType), newOrder.DeliveryAddress.Country.ISO);
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].ShipToAddress.Country = (CountryCodeType)countryCode;
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].ShipToAddress.PostalCode = newOrder.DeliveryAddress.PostalCode;
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].ShipToAddress.CityName = newOrder.DeliveryAddress.Town;
                req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails.PaymentDetails[0].ShipToAddress.StateOrProvince = newOrder.DeliveryAddress.Province;

                res = payPalAPIAAInterfaceClient.SetExpressCheckout(ref requesterCredentials, req);
                payPalAPIAAInterfaceClient.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;

                //if (res.Any != null)
                //{
                //    newOrder.PayPalToken = res.Any.InnerText;
                //}

                if (!String.IsNullOrEmpty(res.Token))
                {
                    newOrder.PayPalToken = res.Token;
                }

                context.SaveChanges();
            }

            // e-mail
            if ((newOrder.OrderStatus & (int)OrderStatus.Promotional) != (int)OrderStatus.Promotional)
            {

                object[] parameters = new object[] 
                { 
                    newOrder.OrderId,
                    newOrder.Partner.ContactPerson.FirstName, 
                    newOrder.Currency.Prefix + newOrder.OrderTotal.ToString("0.00") + " " + newOrder.Currency.Postfix,
                    newOrder.PassCode,
                    newOrder.PayPalToken
                };

                switch (paymentMethod)
                {
                    case 1:
                        SendEmail("ORDERPLACED_BANKTRANSFER", newOrder.Partner.ContactPerson.CultureName, newOrder.Partner.ContactPerson.Email, parameters);
                        break;

                    case 2:
                    case 3:
                        SendEmail("ORDERPLACED_PAYPAL", newOrder.Partner.ContactPerson.CultureName, newOrder.Partner.ContactPerson.Email, parameters);
                        break;

                    case 4:
                        SendEmail("ORDERPLACED_POST", newOrder.Partner.ContactPerson.CultureName, newOrder.Partner.ContactPerson.Email, parameters);
                        break;

                    default:
                        break;
                }
            }

            return newOrder.OrderId;
        }

        public int CreateNewOrderForPartner(int partnerId, int deliveryAddressId, short paymentMethod, string referer, List<ProductQuantity> basket, int invoiceCurrencyId, string couponCode)
        {
            Order order = InsertOrder(partnerId, deliveryAddressId, paymentMethod, referer, basket, invoiceCurrencyId, null, couponCode);
            context.SaveChanges();

            return order.OrderId;
        }

        public ProductDiscountPrice[] GetEffectiveProductPrices(Product[] products, int currencyId, int? partnerId, int countryId, DateTime dateTime)
        {
            List<ProductDiscountPrice> result = new List<ProductDiscountPrice>();

            IOrderedEnumerable<Discount> discounts = GetApplicableDiscounts(partnerId, countryId, dateTime, DiscountType.General).OrderBy(d => d.Multiplier);

            foreach (Product product in products)
            {
                decimal msrp = (decimal)ConvertCurrencies(product.Currency.CurrencyId, currencyId) * product.MSRP;
                Discount discount = discounts.Where<Discount>(d => (d.Product == null) || (d.Product.ProductId == product.ProductId)).OrderBy<Discount, double>(d => d.Multiplier).FirstOrDefault<Discount>();
                decimal effectiveProductCost = msrp;
                if (discount != null)
                {
                    effectiveProductCost *= (decimal)discount.Multiplier;
                }
                effectiveProductCost = RoundUpMoney(effectiveProductCost, currencyId);
                msrp = RoundUpMoney(msrp, currencyId);

                result.Add(new ProductDiscountPrice() { Discount = discount, Product = product, CurrencyId = currencyId, Price = effectiveProductCost, MSRP = msrp });
            }

            return result.ToArray();
        }

        private ProductDiscountCalculationResult ProductDiscountCalculation(ProductDiscountPrice price, Discount quantityDiscount, Discount couponDiscount)
        {
            ProductDiscountCalculationResult result = new ProductDiscountCalculationResult();

            result.OriginalPrice = RoundUpMoney(price.Price, price.CurrencyId);

            result.PriceAfterQuantityDiscount = result.OriginalPrice;
            if (quantityDiscount != null)
            {
                result.PriceAfterQuantityDiscount = RoundUpMoney(result.OriginalPrice * (decimal)quantityDiscount.Multiplier, price.CurrencyId);
            }

            result.PriceAfterCouponDiscount = result.PriceAfterQuantityDiscount;
            if (couponDiscount != null)
            {
                result.PriceAfterCouponDiscount = RoundUpMoney(result.PriceAfterQuantityDiscount * (decimal)couponDiscount.Multiplier, price.CurrencyId);
            }

            result.FinalPrice = result.PriceAfterCouponDiscount;

            return result;
        }

        public OrderCosts CalculateOrderCosts(int? partnerId, int countryId, short paymentMethod, List<ProductQuantity> basket, int invoiceCurrencyId, string couponCode)
        {
            OrderCosts orderCosts = new OrderCosts();
            orderCosts.CurrencyId = invoiceCurrencyId;
            int hufCurrencyId = context.Currency.First(c => c.ISO == "HUF").CurrencyId;

            ProductDiscountPrice[] prices = GetEffectiveProductPrices(ListProducts(), invoiceCurrencyId, partnerId, countryId, DateTime.UtcNow);
            IOrderedEnumerable<Discount> quantityDiscounts = GetApplicableDiscounts(partnerId, countryId, DateTime.UtcNow, DiscountType.Quantity).OrderBy(d => d.Multiplier);
            IOrderedEnumerable<Discount> couponDiscounts = GetApplicableDiscounts(partnerId, countryId, DateTime.UtcNow, DiscountType.Coupon).OrderBy(d => d.Multiplier);

            orderCosts.TotalWeight = 0;
            foreach (ProductQuantity pq in basket)
            {
                Product product = context.Product.Include("Currency").First<Product>(p => p.ProductId == pq.ProductId);

                orderCosts.TotalWeight += product.Weight * pq.Quantity;

                ProductDiscountPrice price = prices.First(pdp => pdp.Product.ProductId == product.ProductId);
                Discount quantityDiscount = quantityDiscounts.FirstOrDefault<Discount>(d => (d.MinimumQuantity <= pq.Quantity) && ((d.Product == null) || ((d.Product.ProductId == pq.ProductId))));
                Discount couponDiscount = couponDiscounts.FirstOrDefault<Discount>(d => (d.Code == couponCode) && ((d.Product == null) || ((d.Product.ProductId == pq.ProductId))));

                ProductDiscountCalculationResult pdcr = ProductDiscountCalculation(price, quantityDiscount, couponDiscount);

                orderCosts.ProductCost += pdcr.OriginalPrice * pq.Quantity;
                orderCosts.QuantityDiscount += (pdcr.OriginalPrice - pdcr.PriceAfterQuantityDiscount) * pq.Quantity;
                orderCosts.CouponDiscount += (pdcr.PriceAfterQuantityDiscount - pdcr.PriceAfterCouponDiscount) * pq.Quantity;
            }

            orderCosts.TotalWeight += 96 + 42; // box + fillings
            if (orderCosts.ProductCost == 0) return orderCosts;

            orderCosts.PackageCost = 100.0M * (decimal)ConvertCurrencies(hufCurrencyId, invoiceCurrencyId);

            Country country = context.Country.First(c => c.CountryId == countryId);

            if (country.ISO == "HU")
            {
                if (paymentMethod != 4)
                {
                    if (orderCosts.TotalWeight <= 500)
                    {
                        // levelkent
                        orderCosts.SendingCost = 500;
                    }
                    else if (orderCosts.TotalWeight <= 750)
                    {
                        // levelkent
                        orderCosts.SendingCost = 700;
                    }
                    else
                    {
                        // csomagkent
                        orderCosts.SendingCost = 875;
                    }
                }
                else
                {
                    if (orderCosts.TotalWeight <= 500)
                    {
                        // levelkent
                        orderCosts.SendingCost = 500;
                    }
                    else
                    {
                        // csomagkent
                        orderCosts.SendingCost = 875;
                    }
                }
            }
            else
            {
                if (orderCosts.TotalWeight <= 500)
                {
                    orderCosts.SendingCost = 1500;
                }
                else if (orderCosts.TotalWeight <= 1000)
                {
                    orderCosts.SendingCost = 3000;
                }
                else
                {
                    orderCosts.SendingCost = 4000;
                }
            }

            orderCosts.SendingCost *= (decimal)ConvertCurrencies(hufCurrencyId, invoiceCurrencyId);

            switch (paymentMethod)
            {
                case 1: // Bank transfer
                    orderCosts.TransactionCost = 0;
                    break;

                case 2: // Paypal
                    orderCosts.TransactionCost = (60 * (decimal)ConvertCurrencies(hufCurrencyId, invoiceCurrencyId)) + (orderCosts.ProductCost * 0.029M);
                    break;

                case 3: // Debit/Credit card
                    orderCosts.TransactionCost = (60 * (decimal)ConvertCurrencies(hufCurrencyId, invoiceCurrencyId)) + (orderCosts.ProductCost * 0.029M);
                    break;

                case 4: // By post
                    if (country.ISO == "HU")
                    {
                        if (orderCosts.TotalWeight <= 500)
                        {
                            orderCosts.TransactionCost = 550;
                        }
                        else
                        {
                            orderCosts.TransactionCost = 400;
                        }
                    }
                    else
                    {
                        if (orderCosts.TotalWeight <= 500)
                        {
                            orderCosts.TransactionCost = 550;
                        }
                        else if (orderCosts.TotalWeight <= 750)
                        {
                            orderCosts.TransactionCost = 700;
                        }
                        else
                        {
                            orderCosts.TransactionCost = 700;
                        }
                    }

                    orderCosts.TransactionCost *= (decimal)ConvertCurrencies(hufCurrencyId, invoiceCurrencyId);
                    break;

                case 5: // Cash
                    orderCosts.TransactionCost = 0;
                    break;

                default:
                    orderCosts.TransactionCost = 0;
                    break;
            }

            orderCosts.PackageCost = RoundUpMoney(orderCosts.PackageCost, invoiceCurrencyId);
            orderCosts.ProductCost = RoundUpMoney(orderCosts.ProductCost, invoiceCurrencyId);
            orderCosts.SendingCost = RoundUpMoney(orderCosts.SendingCost, invoiceCurrencyId);
            orderCosts.TransactionCost = RoundUpMoney(orderCosts.TransactionCost, invoiceCurrencyId);
            orderCosts.InsuranceCost = 0.0M;
            orderCosts.Total = orderCosts.PackageCost + orderCosts.ProductCost + orderCosts.SendingCost + orderCosts.TransactionCost + orderCosts.InsuranceCost - orderCosts.QuantityDiscount - orderCosts.CouponDiscount;

            return orderCosts;
        }

        public void SetOrderStatus(int orderId, int status)
        {
            SetOrderStatusInt(orderId, status);
            context.SaveChanges();
        }

        private void SetOrderStatusInt(int orderId, int status)
        {
            string[] commonChanges = "1>3,1>271,3>15,15>31,31>63,31>159,9>27,27>31,27>59,59>63,31>95,63>191,95>223,271>287,27>2075,31>2079,527>543".Split(new char[] { ',' });

            Order order = context.Order.Include("Partner").First(o => o.OrderId == orderId);

            int oldStatus = order.OrderStatus;
            order.OrderStatus |= status;

            if (oldStatus == order.OrderStatus) return;

            bool commonChange = false;
            foreach (string change in commonChanges)
            {
                string[] ch = change.Split(new char[] { '>' });

                if ((ch[0] == oldStatus.ToString()) && (ch[1] == order.OrderStatus.ToString()))
                {
                    commonChange = true;
                    break;
                }
            }

            if (!commonChange)
            {
                LogWarning(orderId, order.Partner.PartnerId, "SetOrderStatus", "The status of order '{0}' was changed from {1} to {2}. This is not a usual change.", orderId, oldStatus, order.OrderStatus);
                SendEmail("UNKNOWNSTATUSCHANGE", "hu-HU", null, new object[] { orderId, oldStatus, order.OrderStatus, order.PassCode });
            }
            else
            {
                LogInformation(orderId, order.Partner.PartnerId, "SetOrderStatus", "The status of order '{0}' was changed from {1} to {2}.", orderId, oldStatus, order.OrderStatus);
            }
        }

        public void SendEmail(string templateName, string cultureName, string recipientEmail, object[] parameters)
        {
            if (emailServiceDisabled) return;

            BackgroundWorker bw = new BackgroundWorker();

            bw.DoWork += new DoWorkEventHandler(delegate(Object sender, DoWorkEventArgs e) { Util.TemplateEmailSender.Send(templateName, cultureName, recipientEmail, null, parameters); });

            bw.RunWorkerAsync();
        }

        public Country[] ListCountries()
        {
            return context.Country.Include("Currency").ToArray();
        }

        public Currency[] ListCurrencies()
        {
            return context.Currency.ToArray();
        }

        public Product[] ListProducts()
        {
            return context.Product.Include("Currency").ToArray();
        }

        public Address InsertAddress(int partnerId, int countryId, string province, string town, string postalCode, string addressLine, string name)
        {
            Address newAddress = Address.CreateAddress(0, town, postalCode, addressLine, province);
            try
            {
                Country country = context.Country.First<Country>(c => c.CountryId == countryId);
                newAddress.CountryReference = new System.Data.Objects.DataClasses.EntityReference<Country>() { EntityKey = country.EntityKey };

                Partner partner = context.Partner.First<Partner>(p => p.PartnerId == partnerId);
                newAddress.PartnerReference = new System.Data.Objects.DataClasses.EntityReference<Partner>() { EntityKey = partner.EntityKey };

                newAddress.Name = name;

                context.AddToAddress(newAddress);

                LogInformation(null, partnerId, "InsertAddress", "Address with postal code '{0}' was created successfully.", postalCode);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError(null, partnerId, "InsertAddress", ex, "Address with postal code '{0}' coundn't be created.", postalCode);
                context.SaveChanges();
            }

            return newAddress;

        }


        private static Func<NaplampaEntities, int, Order> GetOrderQuery
                = CompiledQuery.Compile((NaplampaEntities context, int orderId)
                    =>
                    context.Order.Include("Currency").Include("Partner").Include("Partner.ContactPerson").Include("DeliveryAddress").Include("DeliveryAddress.Country").Include("OrderProductXRefs").Include("OrderProductXRefs.Product").Include("Invoices")
                    .FirstOrDefault<Order>(o => o.OrderId == orderId)
            );


        public Order GetOrder(int orderId)
        {
            //Order result = context.Order.Include("Currency").Include("Partner").Include("Partner.ContactPerson").Include("DeliveryAddress").Include("DeliveryAddress.Country").Include("OrderProductXRefs").Include("OrderProductXRefs.Product").Include("Invoices")
            //    .FirstOrDefault<Order>(o => o.OrderId == orderId);

            return GetOrderQuery(context, orderId);
        }


        private static Func<NaplampaEntities, int?, DateTime, IQueryable<Order>> ListOrdersQuery
            = CompiledQuery.Compile((NaplampaEntities context, int? partnerId, DateTime from)
                =>
                context.Order.Include("Currency").Include("Partner").Include("Partner.ContactPerson").Include("DeliveryAddress").Include("DeliveryAddress.Country").Include("OrderProductXRefs").Include("OrderProductXRefs.Product")
                .Where<Order>(o => (partnerId.HasValue ? o.Partner.PartnerId == partnerId : true) && (o.Created >= from) && ((o.OrderStatus & 65536) != 65536))
        );


        public Order[] ListOrders(int? partnerId, DateTime from)
        {
            return ListOrdersQuery(context, partnerId, from).ToArray();
        }

        public Order[] SearchOrders(int? partnerId, string keyword, int minimumRank, decimal minimumOrder, int minimumOrderCurrencyId, bool showDeleted, bool? newsletter)
        {
            IQueryable<Order> result = context.Order.Include("Currency").Include("Partner").Include("Partner.ContactPerson").Include("DeliveryAddress").Include("DeliveryAddress.Country").Include("OrderProductXRefs").Include("OrderProductXRefs.Product")
                .Where<Order>(o => (partnerId.HasValue ? o.Partner.PartnerId == partnerId : true) && ((showDeleted) || ((o.OrderStatus & (int)OrderStatus.Deleted) != (int)OrderStatus.Deleted)) && ((newsletter == null) || ((o.Partner.ContactPerson.Newsletter == newsletter))))
                .Where<Order>(o => String.IsNullOrEmpty(keyword) || (((o.CommentByBuyer != null) && (o.CommentByBuyer.Contains(keyword))) || ((o.CommentBySeller != null) && (o.CommentBySeller.Contains(keyword))) || ((o.TrackingNumber != null) && (o.TrackingNumber.Contains(keyword))) || (o.Partner.FullName.Contains(keyword)) || ((o.Partner.Phone != null) && (o.Partner.Phone.Contains(keyword))) || (o.Partner.ContactPerson.Email.Contains(keyword)) || (o.Partner.ContactPerson.FirstName.Contains(keyword)) || (o.Partner.ContactPerson.LastName.Contains(keyword))));

            if (minimumOrder > 0)
            {
                result = result.Where(o => (o.Currency.CurrencyId == minimumOrderCurrencyId) && (o.OrderTotal >= minimumOrder));
            }

            return result.ToArray();
        }

        public Invoice GetInvoice(int invoiceId)
        {
            Invoice result = context.Invoice.Include("Currency").Include("BuyerBillingAddress").Include("BuyerBillingAddress.Country").Include("BuyerPartner")
                .Include("IssuerBillingAddress").Include("IssuerBillingAddress.Country").Include("IssuerPartner")
                .Include("InvoiceLines").Include("InvoiceLines.Product").Include("Order").Include("Order.OrderProductXRefs").Include("Order.OrderProductXRefs.Product")
                .FirstOrDefault<Invoice>(i => i.InvoiceId == invoiceId);

            return result;
        }

        public Survey GetSurvey(int surveyId)
        {
            return context.Survey.Include("SurveyResults").Include("SurveyResults.Order").FirstOrDefault<Survey>(s => s.SurveyId == surveyId);
        }

        public void SendSurveyRequest(int orderId, int surveyId)
        {
            Order order = context.Order.Include("Partner").Include("Partner.ContactPerson").First(o => o.OrderId == orderId);
            if ((order.OrderStatus & (int)OrderStatus.SurveySent) == (int)OrderStatus.SurveySent) return;

            Survey survey = context.Survey.First<Survey>(s => s.SurveyId == surveyId);
            if ((survey.Expires.HasValue) && (survey.Expires.Value < DateTime.Now)) return;

            SendEmail("SURVEYREQUEST", order.Partner.ContactPerson.CultureName, order.Partner.ContactPerson.Email, new object[] { orderId, order.Partner.ContactPerson.FirstName, order.Partner.ContactPerson.EmailConfirmationCode, survey.SurveyId, survey.Name });

            if (order.Survey == null)
            {
                order.Survey = survey;
                order.SurveySentOn = DateTime.UtcNow;
            }

            SetOrderStatus(order.OrderId, (int)OrderStatus.SurveySent);
        }

        public void SaveSurveyResult(int surveyId, int orderId, string cultureName, string surveyQuestions
            , int resultValue1, int resultValue2, int resultValue3, int resultValue4, int resultValue5, int resultValue6, int resultValue7
            , string resultText1, string resultText2, string resultText3, string resultText4, string resultText5, string resultText6, string resultText7
            , int resultFlags1, int resultFlags2, int resultFlags3, int resultFlags4, int resultFlags5, int resultFlags6, int resultFlags7)
        {
            Survey survey = GetSurvey(surveyId);
            if (survey == null) return;

            Order order = GetOrder(orderId);
            if (order == null) return;

            if (survey == order.Survey)
            {
                order.SurveyCompletedOn = DateTime.UtcNow;
            }

            SurveyResult surveyResult = new SurveyResult();
            surveyResult.SurveyReference = new System.Data.Objects.DataClasses.EntityReference<Survey>() { EntityKey = survey.EntityKey }; ;
            surveyResult.OrderReference = new System.Data.Objects.DataClasses.EntityReference<Order>() { EntityKey = order.EntityKey };

            surveyResult.CultureName = cultureName;
            surveyResult.SurveyQuestions = surveyQuestions;
            surveyResult.ResultValue1 = resultValue1;
            surveyResult.ResultValue2 = resultValue2;
            surveyResult.ResultValue3 = resultValue3;
            surveyResult.ResultValue4 = resultValue4;
            surveyResult.ResultValue5 = resultValue5;
            surveyResult.ResultValue6 = resultValue6;
            surveyResult.ResultValue7 = resultValue7;
            surveyResult.ResultText1 = resultText1;
            surveyResult.ResultText2 = resultText2;
            surveyResult.ResultText3 = resultText3;
            surveyResult.ResultText4 = resultText4;
            surveyResult.ResultText5 = resultText5;
            surveyResult.ResultText6 = resultText6;
            surveyResult.ResultText7 = resultText7;
            surveyResult.ResultFlags1 = resultFlags1;
            surveyResult.ResultFlags2 = resultFlags2;
            surveyResult.ResultFlags3 = resultFlags3;
            surveyResult.ResultFlags4 = resultFlags4;
            surveyResult.ResultFlags5 = resultFlags5;
            surveyResult.ResultFlags6 = resultFlags6;
            surveyResult.ResultFlags7 = resultFlags7;

            context.AddToSurveyResult(surveyResult);
            context.SaveChanges();
        }

        public Survey[] ListSurveys(bool validOnly)
        {
            if (validOnly)
            {
                return context.Survey.Where(s => (s.Expires.HasValue == false) || (s.Expires.Value > DateTime.Now)).ToArray();
            }
            else
            {
                return context.Survey.ToArray();
            }
        }

        public byte[] GetInvoiceImage(int invoiceId, string cultureName)
        {
            Invoice invoice = GetInvoice(invoiceId);

            return InvoiceImageCreator.GetInvoiceImage(invoice, cultureName);
        }

        public void SendRecommendation(int? orderId, string[] recipients, string message)
        {
            string recipientsStr = "";
            foreach (string recipient in recipients)
            {
                recipientsStr += recipient + "|";
            }
            if (String.IsNullOrEmpty(recipientsStr)) return;

            SendEmail("RECOMMENDATION", "hu-HU", null, new object[] { orderId == null ? "ismeretlen" : orderId.ToString(), recipientsStr, message });
        }

        public OrderReview GetOrderReview(int orderId, string payPalToken)
        {
            OrderReview result = null;

            GetExpressCheckoutDetailsResponseType payPalResult = null;

            CustomSecurityHeaderType requesterCredentials = new CustomSecurityHeaderType();
            requesterCredentials.Credentials = new UserIdPasswordType();
            requesterCredentials.Credentials.Username = ConfigurationManager.AppSettings["PayPalUsername"];
            requesterCredentials.Credentials.Password = ConfigurationManager.AppSettings["PayPalPassword"];
            requesterCredentials.Credentials.Signature = ConfigurationManager.AppSettings["PayPalSignature"];

            PayPalAPIAAInterfaceClient payPalAPIAAInterfaceClient = new PayPalAPIAAInterfaceClient();

            GetExpressCheckoutDetailsReq getExpressCheckoutDetailsReq = new GetExpressCheckoutDetailsReq();
            getExpressCheckoutDetailsReq.GetExpressCheckoutDetailsRequest = new GetExpressCheckoutDetailsRequestType();
            getExpressCheckoutDetailsReq.GetExpressCheckoutDetailsRequest.Version = "74.0";
            getExpressCheckoutDetailsReq.GetExpressCheckoutDetailsRequest.Token = payPalToken;
            payPalResult = payPalAPIAAInterfaceClient.GetExpressCheckoutDetails(ref requesterCredentials, getExpressCheckoutDetailsReq);

            payPalAPIAAInterfaceClient.Close();

            return result;
        }

        public bool ConfirmOrder(int orderId, string payPalToken, string payPalPayerId)
        {
            Order order = GetOrder(orderId);

            DoExpressCheckoutPaymentResponseType payPalResult = null;

            CustomSecurityHeaderType requesterCredentials = new CustomSecurityHeaderType();
            requesterCredentials.Credentials = new UserIdPasswordType();
            requesterCredentials.Credentials.Username = ConfigurationManager.AppSettings["PayPalUsername"];
            requesterCredentials.Credentials.Password = ConfigurationManager.AppSettings["PayPalPassword"];
            requesterCredentials.Credentials.Signature = ConfigurationManager.AppSettings["PayPalSignature"];

            PayPalAPIAAInterfaceClient payPalAPIAAInterfaceClient = new PayPalAPIAAInterfaceClient();

            DoExpressCheckoutPaymentReq doExpressCheckoutPaymentReq = new DoExpressCheckoutPaymentReq();
            doExpressCheckoutPaymentReq.DoExpressCheckoutPaymentRequest = new DoExpressCheckoutPaymentRequestType();
            doExpressCheckoutPaymentReq.DoExpressCheckoutPaymentRequest.Version = "74.0";
            doExpressCheckoutPaymentReq.DoExpressCheckoutPaymentRequest.DoExpressCheckoutPaymentRequestDetails = new DoExpressCheckoutPaymentRequestDetailsType();
            doExpressCheckoutPaymentReq.DoExpressCheckoutPaymentRequest.DoExpressCheckoutPaymentRequestDetails.Token = payPalToken;
            doExpressCheckoutPaymentReq.DoExpressCheckoutPaymentRequest.DoExpressCheckoutPaymentRequestDetails.PaymentAction = PaymentActionCodeType.Sale;
            doExpressCheckoutPaymentReq.DoExpressCheckoutPaymentRequest.DoExpressCheckoutPaymentRequestDetails.PayerID = payPalPayerId;
            doExpressCheckoutPaymentReq.DoExpressCheckoutPaymentRequest.DoExpressCheckoutPaymentRequestDetails.PaymentDetails = new PaymentDetailsType[1];
            doExpressCheckoutPaymentReq.DoExpressCheckoutPaymentRequest.DoExpressCheckoutPaymentRequestDetails.PaymentDetails[0] = new PaymentDetailsType();
            doExpressCheckoutPaymentReq.DoExpressCheckoutPaymentRequest.DoExpressCheckoutPaymentRequestDetails.PaymentDetails[0].OrderTotal = new BasicAmountType();
            doExpressCheckoutPaymentReq.DoExpressCheckoutPaymentRequest.DoExpressCheckoutPaymentRequestDetails.PaymentDetails[0].OrderTotal.currencyID = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), order.Currency.ISO);
            doExpressCheckoutPaymentReq.DoExpressCheckoutPaymentRequest.DoExpressCheckoutPaymentRequestDetails.PaymentDetails[0].OrderTotal.Value = order.OrderTotal.ToString("0.00");

            payPalResult = payPalAPIAAInterfaceClient.DoExpressCheckoutPayment(ref requesterCredentials, doExpressCheckoutPaymentReq);

            payPalAPIAAInterfaceClient.Close();

            bool result = ((payPalResult.Ack == AckCodeType.Success) || (payPalResult.Ack == AckCodeType.SuccessWithWarning));

            if (result)
            {
                SetOrderStatus(order.OrderId, (int)(OrderStatus.PaymentReceived | OrderStatus.ReadyToPost));
            }

            return result;
        }
        #endregion

        private decimal RoundUpMoney(decimal value, int currencyId, params bool[] down)
        {
            Currency currency = context.Currency.First<Currency>(c => c.CurrencyId == currencyId);

            decimal roundTo = 0.5M * (decimal)Math.Pow(10.0, 2 - currency.DefaultDecimalPlaces);
            decimal result = 0;

            while (result < value) result += roundTo;
            if ((down != null) && (down.Length >= 1) && (down[0]) && (result != value)) result -= roundTo;

            return result;
        }

        private decimal RoundDownMoney(decimal value, int currencyId)
        {
            return RoundUpMoney(value, currencyId, true);
        }

        private List<Discount> GetApplicableDiscounts(int? partnerId, int countryId, DateTime utcTime, DiscountType discountType)
        {
            List<Discount> result = new List<Discount>();

            int rank = 1;

            if (partnerId.HasValue)
            {
                Partner partner = context.Partner.First<Partner>(p => p.PartnerId == partnerId.Value);
                if (partner != null)
                {
                    rank = partner.Rank;
                }

            }

            IQueryable<Discount> discounts = context.Discount.Include("Country").Include("Product")
                .Where(d => ((!d.MinimumPartnerRank.HasValue) || (d.MinimumPartnerRank.Value <= rank))
                && (d.ValidFrom < DateTime.UtcNow)
                && ((!d.ValidUntil.HasValue) || (d.ValidUntil.Value >= DateTime.UtcNow))
                && ((d.Country == null) || (d.Country.CountryId == countryId)));

            switch (discountType)
            {
                case DiscountType.General:
                    discounts = discounts.Where(d => (!d.MinimumQuantity.HasValue) && (String.IsNullOrEmpty(d.Code)));
                    break;
                case DiscountType.Quantity:
                    discounts = discounts.Where(d => (d.MinimumQuantity.HasValue));
                    break;
                case DiscountType.Coupon:
                    discounts = discounts.Where(d => (!String.IsNullOrEmpty(d.Code)));
                    break;

                case DiscountType.All:
                default:
                    break;
            }

            return discounts.ToList();
        }

        private Partner InsertPartner(string name, string phone)
        {
            Partner newPartner = Partner.CreatePartner(0, name, 1);
            newPartner.Phone = phone;
            context.AddToPartner(newPartner);
            LogInformation(null, newPartner.PartnerId, "InsertPartner", "Partner named '{0}' was created successfully.", name);

            return newPartner;
        }

        private Person InsertPerson(string title, string firstName, string lastName, string email, string phone, bool newsletter, string cultureName)
        {
            Partner basePartner = InsertPartner(firstName + " " + lastName, phone);
            Person newPerson = Person.CreatePerson(0, title, firstName, lastName, email, true, cultureName);

            context.SaveChanges();
            newPerson.Base = basePartner;

            newPerson.EmailConfirmationCode = GenerateRandomString(32, false);
            newPerson.Newsletter = newsletter;
            context.AddToPerson(newPerson);

            context.SaveChanges();
            basePartner.ContactPerson = newPerson;

            SendEmail("CONFIRMEMAIL", cultureName, email, new object[] { newPerson.FirstName, newPerson.Email, newPerson.EmailConfirmationCode, newPerson.PersonId });

            return newPerson;
        }

        private Order InsertOrder(int partnerId, int deliveryAddressId, short paymentMethod, string referer, List<ProductQuantity> basket, int invoiceCurrencyId, int? initialStatus, string couponCode)
        {
            Partner issuer = context.Partner.Include("BillingAddress").First<Partner>(a => a.RefererCode == "NAPLAMPA");
            Partner partner = context.Partner.Include("BillingAddress").First<Partner>(a => a.PartnerId == partnerId);
            Address address = context.Address.Include("Country").First<Address>(a => a.AddressId == deliveryAddressId);
            Currency currency = context.Currency.First<Currency>(c => c.CurrencyId == invoiceCurrencyId);

            OrderCosts costs = CalculateOrderCosts(partnerId, address.Country.CountryId, paymentMethod, basket, invoiceCurrencyId, couponCode);

            Order newOrder = Order.CreateOrder(0, DateTime.UtcNow, (int)OrderStatus.OrderPlaced, paymentMethod, costs.TotalWeight, costs.TransactionCost, costs.PackageCost, costs.SendingCost, costs.ProductCost, costs.InsuranceCost, costs.Total, 0, costs.QuantityDiscount, costs.CouponDiscount);
            newOrder.PartnerReference.Value = partner;
            newOrder.DeliveryAddressReference.Value = address;
            newOrder.CurrencyReference.Value = currency;
            newOrder.Referer = referer;
            newOrder.PassCode = GenerateRandomString(32, false);
            if (!String.IsNullOrEmpty(couponCode)) newOrder.DiscountCode = couponCode;

            if ((paymentMethod == 1) || (paymentMethod == 2) || (paymentMethod == 3))
            {
                newOrder.OrderStatus |= (int)OrderStatus.PaymentRequestSent;
            }

            if (paymentMethod == 4)
            {
                newOrder.OrderStatus |= (int)OrderStatus.ReadyToPost;
            }

            if (initialStatus.HasValue)
            {
                newOrder.OrderStatus |= (int)initialStatus.Value;
            }

            context.AddToOrder(newOrder);

            string invoiceGen;
            do
            {
                invoiceGen = GenerateRandomString(5, true);
            } while (context.Invoice.FirstOrDefault<Invoice>(i => i.InvoiceNo == "IS36" + invoiceGen) != null);

            Invoice newInvoice = Invoice.CreateInvoice(0, "IS36" + invoiceGen, DateTime.UtcNow.Date.AddDays(3), DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(14), newOrder.OrderTotal + newOrder.VATTotal, newOrder.VATTotal, newOrder.OrderTotal, 0);
            newInvoice.OrderReference.Value = newOrder;
            newInvoice.CurrencyReference.Value = currency;
            newInvoice.IssuerBillingAddressReference.Value = issuer.BillingAddress;
            newInvoice.IssuerPartnerReference.Value = issuer;
            newInvoice.BuyerBillingAddressReference.Value = partner.BillingAddress;
            newInvoice.BuyerPartnerReference.Value = partner;
            newInvoice.PassCode = GenerateRandomString(32, false);

            int line = 0;
            IOrderedEnumerable<Discount> quantityDiscounts = GetApplicableDiscounts(partnerId, address.Country.CountryId, DateTime.UtcNow, DiscountType.Quantity).OrderBy(d => d.Multiplier);
            IOrderedEnumerable<Discount> couponDiscounts = GetApplicableDiscounts(partnerId, address.Country.CountryId, DateTime.UtcNow, DiscountType.Coupon).OrderBy(d => d.Multiplier);

            InvoiceLine invoiceLine = null;
            foreach (ProductQuantity pq in basket)
            {
                if (pq.Quantity == 0) continue;
                Product product = context.Product.First<Product>(p => p.ProductId == pq.ProductId);

                OrderProductXRef orderProductXRef = OrderProductXRef.CreateOrderProductXRef(newOrder.OrderId, pq.ProductId, pq.Quantity);
                context.AddToOrderProductXRef(orderProductXRef);
                orderProductXRef.OrderReference.Value = newOrder;
                orderProductXRef.ProductReference.Value = product;

                ProductDiscountPrice effectivePrice = GetEffectiveProductPrices(new Product[] { product }, invoiceCurrencyId, partnerId, address.Country.CountryId, DateTime.UtcNow).ToArray()[0];
                Discount quantityDiscount = quantityDiscounts.FirstOrDefault<Discount>(d => (d.MinimumQuantity <= pq.Quantity) && ((d.Product == null) || ((d.Product.ProductId == pq.ProductId))));
                Discount couponDiscount = couponDiscounts.FirstOrDefault<Discount>(d => (d.Code == couponCode) && ((d.Product == null) || ((d.Product.ProductId == pq.ProductId))));

                ProductDiscountCalculationResult pdcr = ProductDiscountCalculation(effectivePrice, quantityDiscount, couponDiscount);


                invoiceLine = InvoiceLine.CreateInvoiceLine(0, line, product.Description, pq.Quantity, pdcr.FinalPrice, pdcr.FinalPrice * (decimal)address.Country.VATMultiplier);
                context.AddToInvoiceLine(invoiceLine);
                invoiceLine.InvoiceReference.Value = newInvoice;
                invoiceLine.ProductReference.Value = product;

                line++;
            }

            if (costs.PackageCost != 0)
            {
                invoiceLine = InvoiceLine.CreateInvoiceLine(0, line, "LOC_PACKAGECOST", 1, costs.PackageCost, costs.PackageCost * (decimal)address.Country.VATMultiplier);
                context.AddToInvoiceLine(invoiceLine);
                invoiceLine.InvoiceReference.Value = newInvoice;
                line++;
            }


            if (costs.SendingCost != 0)
            {
                invoiceLine = InvoiceLine.CreateInvoiceLine(0, line, "LOC_SENDINGCOST", 1, costs.SendingCost, costs.SendingCost * (decimal)address.Country.VATMultiplier);
                context.AddToInvoiceLine(invoiceLine);
                invoiceLine.InvoiceReference.Value = newInvoice;
                line++;
            }

            if (costs.TransactionCost != 0)
            {
                invoiceLine = InvoiceLine.CreateInvoiceLine(0, line, "LOC_TRANSACTIONCOST", 1, costs.TransactionCost, costs.TransactionCost * (decimal)address.Country.VATMultiplier);
                context.AddToInvoiceLine(invoiceLine);
                invoiceLine.InvoiceReference.Value = newInvoice;
                line++;
            }

            if (costs.InsuranceCost != 0)
            {
                invoiceLine = InvoiceLine.CreateInvoiceLine(0, line, "LOC_INSURANCECOST", 1, costs.InsuranceCost, costs.InsuranceCost * (decimal)address.Country.VATMultiplier);
                context.AddToInvoiceLine(invoiceLine);
                invoiceLine.InvoiceReference.Value = newInvoice;
                line++;
            }

            return newOrder;
        }

        private Order UpdateOrder(Order order)
        {
            context.ApplyPropertyChanges("Order", order);
            context.SaveChanges();
            context.Refresh(System.Data.Objects.RefreshMode.StoreWins, order);

            // TODO: check if the status id was changed and send a request to the e-mail service if it was

            return order;
        }

        private List<Order> ListOrders(OrderStatus? orderStatus, DateTime? createdAfter, DateTime? createdBefore)
        {
            int orderStatusInt32 = orderStatus.HasValue ? (int)orderStatus.Value : 0;

            var query = from o in context.Order
                        //where ((!orderStatus.HasValue) || (o.OrderStatusId == orderStatusInt32))
                        //        && ((!createdAfter.HasValue) || (o.CreatedOn >= createdAfter))
                        //        && ((!createdBefore.HasValue) || (o.CreatedOn <= createdBefore))
                        select o;

            return query.ToList<Order>();
        }

        private List<OrderStatus> ListAllowedStatusChangesFrom(OrderStatus orderStatus)
        {
            // TODO: implement this

            return new List<OrderStatus>();
        }

        private string GenerateRandomString(int length, bool numbersOnly)
        {
            string validCharacters = "qazwsxedcrfvtgbyhnujmikolpQAZWSXEDCRFVTGBYHNUJMIKOLP0123456789";
            string validNumbers = "0123456789";

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(numbersOnly ? validNumbers[random.Next(validNumbers.Length)] : validCharacters[random.Next(validCharacters.Length)]);
            }
            return sb.ToString();
        }

        private void LogInformation(int? orderId, int? partnerId, string procedure, string formatString, params object[] args)
        {
            int loggedInPersonId = System.ServiceModel.ServiceSecurityContext.Current == null ? 19000012 : Int32.Parse(System.ServiceModel.ServiceSecurityContext.Current.PrimaryIdentity.Name);

            Log newLog = Log.CreateLog(0, 1, DateTime.UtcNow, String.Format(formatString, args), loggedInPersonId);

            if (partnerId.HasValue)
            {
                Partner partner = context.Partner.FirstOrDefault<Partner>(p => p.PartnerId == partnerId.Value);
                if (partner != null)
                {
                    newLog.PartnerReference = new System.Data.Objects.DataClasses.EntityReference<Partner>() { EntityKey = partner.EntityKey };
                }
            }
            if (orderId.HasValue)
            {
                Order order = context.Order.FirstOrDefault<Order>(p => p.OrderId == orderId.Value);

                if (order != null)
                {
                    newLog.OrderReference = new System.Data.Objects.DataClasses.EntityReference<Order>() { EntityKey = order.EntityKey };
                }
            }

            context.AddToLog(newLog);
        }

        private void LogWarning(int? orderId, int? partnerId, string procedure, string formatString, params object[] args)
        {
            int loggedInPersonId = System.ServiceModel.ServiceSecurityContext.Current == null ? 19000012 : Int32.Parse(System.ServiceModel.ServiceSecurityContext.Current.PrimaryIdentity.Name);

            Log newLog = Log.CreateLog(0, 2, DateTime.UtcNow, String.Format(formatString, args), loggedInPersonId);


            if (partnerId.HasValue)
            {
                Partner partner = context.Partner.FirstOrDefault<Partner>(p => p.PartnerId == partnerId.Value);
                if (partner != null)
                {
                    newLog.PartnerReference = new System.Data.Objects.DataClasses.EntityReference<Partner>() { EntityKey = partner.EntityKey };
                }
            }
            if (orderId.HasValue)
            {
                Order order = context.Order.FirstOrDefault<Order>(p => p.OrderId == orderId.Value);

                if (order != null)
                {
                    newLog.OrderReference = new System.Data.Objects.DataClasses.EntityReference<Order>() { EntityKey = order.EntityKey };
                }
            }

            context.AddToLog(newLog);
        }

        private void LogError(int? orderId, int? partnerId, string procedure, Exception ex, string formatString, params object[] args)
        {
            int loggedInPersonId = System.ServiceModel.ServiceSecurityContext.Current == null ? 19000012 : Int32.Parse(System.ServiceModel.ServiceSecurityContext.Current.PrimaryIdentity.Name);

            Log newLog = Log.CreateLog(0, 4, DateTime.UtcNow, "(" + procedure + ")" + String.Format(formatString, args) + (ex == null ? "" : "[Exception: " + ex.ToString() + "]"), loggedInPersonId);


            if (partnerId.HasValue)
            {
                Partner partner = context.Partner.FirstOrDefault<Partner>(p => p.PartnerId == partnerId.Value);
                if (partner != null)
                {
                    newLog.PartnerReference = new System.Data.Objects.DataClasses.EntityReference<Partner>() { EntityKey = partner.EntityKey };
                }
            }
            if (orderId.HasValue)
            {
                Order order = context.Order.FirstOrDefault<Order>(p => p.OrderId == orderId.Value);

                if (order != null)
                {
                    newLog.OrderReference = new System.Data.Objects.DataClasses.EntityReference<Order>() { EntityKey = order.EntityKey };
                }
            }

            context.AddToLog(newLog);
        }

        private double ConvertCurrencies(int sourceCurrencyId, int destinationCurrencyId)
        {
            Currency sourceCurrency = context.Currency.First<Currency>(c => c.CurrencyId == sourceCurrencyId);
            if (sourceCurrency.ConversionRatioDate < DateTime.UtcNow.Add(-currencyRateTimeOut))
            {
                LogWarning(null, null, "ConvertCurrencies", "The conversion rate of currency '{0}' is out of date. Updating...", sourceCurrency.ISO);
                UpdateCurrenyRate(sourceCurrency);
                sourceCurrency = context.Currency.First<Currency>(c => c.CurrencyId == sourceCurrencyId);
            }

            Currency destinationCurrency = context.Currency.First<Currency>(c => c.CurrencyId == destinationCurrencyId);
            if (destinationCurrency.ConversionRatioDate < DateTime.UtcNow.Add(-currencyRateTimeOut))
            {
                LogWarning(null, null, "ConvertCurrencies", "The conversion rate of currency '{0}' is out of date. Updating...", destinationCurrency.ISO);
                UpdateCurrenyRate(destinationCurrency);
                destinationCurrency = context.Currency.First<Currency>(c => c.CurrencyId == destinationCurrencyId);
            }

            return sourceCurrency.ConversionToEur / destinationCurrency.ConversionToEur;
        }

        private void UpdateCurrenyRate(Currency currency)
        {
            HttpWebRequest req = null;
            HttpWebResponse resp = null;

            try
            {
                string requestUrl = "http://www.webservicex.net/CurrencyConvertor.asmx/ConversionRate?FromCurrency=EUR&ToCurrency=" + currency.ISO;
                // get the IP's country
                req = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                resp = (HttpWebResponse)req.GetResponse();
                StreamReader strmReader = new StreamReader(resp.GetResponseStream());
                string rateResponse = strmReader.ReadToEnd().Trim();
                rateResponse = rateResponse.Substring(rateResponse.IndexOf("<double"));
                rateResponse = rateResponse.Substring(rateResponse.IndexOf(">") + 1, rateResponse.IndexOf("</double>") - rateResponse.IndexOf(">") - 1);

                currency.ConversionToEur = 1.00 / Double.Parse(rateResponse, new CultureInfo("en-US"));
                currency.ConversionRatioDate = DateTime.Now;

                context.SaveChanges();
            }
            catch { }
            finally
            {
                if (req != null)
                {
                    req = null;
                }

                if (resp != null)
                {
                    resp.Close();
                    resp = null;
                }
            }
        }

        public ValidationResult[] ValidateOrder(string title, string firstName, string lastName, string email, string phone, bool newsletter, string cultureName, int countryId, string province, string town, string postalCode, string addressLine, short paymentMethod, string referer, List<ProductQuantity> basket, int invoiceCurrencyId, int? invoiceCountryId, string invoiceProvince, string invoiceTown, string invoicePostalCode, string invoiceAddressLine, string invoiceFullName)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            result.Add(new ValidationResult() { PropertyName = "FirstName", Type = ValidationType.RequiredField, ResultType = String.IsNullOrEmpty(firstName) ? ValidationResultType.Error : ValidationResultType.Pass });
            result.Add(new ValidationResult() { PropertyName = "LastName", Type = ValidationType.RequiredField, ResultType = String.IsNullOrEmpty(lastName) ? ValidationResultType.Error : ValidationResultType.Pass });

            result.Add(new ValidationResult() { PropertyName = "Email", Type = ValidationType.RequiredField, ResultType = String.IsNullOrEmpty(email) ? ValidationResultType.Error : ValidationResultType.Pass });
            result.Add(new ValidationResult() { PropertyName = "Email", Type = ValidationType.RegularExpression, ResultType = (!String.IsNullOrEmpty(email) && !Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")) ? ValidationResultType.Error : ValidationResultType.Pass });

            Country c = context.Country.FirstOrDefault(country => country.CountryId == countryId);
            result.Add(new ValidationResult() { PropertyName = "Country", Type = ValidationType.Custom, ResultType = (c == null) ? ValidationResultType.Error : ValidationResultType.Pass });

            if (c != null)
            {
                result.Add(new ValidationResult() { PropertyName = "PostalCode", Type = ValidationType.RequiredField, ResultType = String.IsNullOrEmpty(postalCode) ? ValidationResultType.Error : ValidationResultType.Pass });
                result.Add(new ValidationResult() { PropertyName = "PostalCode", Type = ValidationType.RegularExpression, ResultType = (!String.IsNullOrEmpty(postalCode) && !String.IsNullOrEmpty(c.PostalCodeValidatorRegularExpression) && !Regex.IsMatch(postalCode, c.PostalCodeValidatorRegularExpression)) ? ValidationResultType.Error : ValidationResultType.Pass, Parameters = new object[] { c.PostalCodeSample } });
            }

            result.Add(new ValidationResult() { PropertyName = "Town", Type = ValidationType.RequiredField, ResultType = String.IsNullOrEmpty(town) ? ValidationResultType.Error : ValidationResultType.Pass });
            result.Add(new ValidationResult() { PropertyName = "AddressLine", Type = ValidationType.RequiredField, ResultType = String.IsNullOrEmpty(addressLine) ? ValidationResultType.Error : ValidationResultType.Pass });


            if (invoiceCountryId.HasValue)
            {
                c = context.Country.FirstOrDefault(country => country.CountryId == invoiceCountryId);
                result.Add(new ValidationResult() { PropertyName = "InvoiceCountry", Type = ValidationType.Custom, ResultType = (c == null) ? ValidationResultType.Error : ValidationResultType.Pass });

                if (c != null)
                {
                    result.Add(new ValidationResult() { PropertyName = "InvoicePostalCode", Type = ValidationType.RequiredField, ResultType = String.IsNullOrEmpty(invoicePostalCode) ? ValidationResultType.Error : ValidationResultType.Pass });
                    result.Add(new ValidationResult() { PropertyName = "InvoicePostalCode", Type = ValidationType.RegularExpression, ResultType = (!String.IsNullOrEmpty(invoicePostalCode) && !String.IsNullOrEmpty(c.PostalCodeValidatorRegularExpression) && !Regex.IsMatch(invoicePostalCode, c.PostalCodeValidatorRegularExpression)) ? ValidationResultType.Error : ValidationResultType.Pass, Parameters = new object[] { c.PostalCodeSample } });
                }

                result.Add(new ValidationResult() { PropertyName = "InvoiceTown", Type = ValidationType.RequiredField, ResultType = String.IsNullOrEmpty(invoiceTown) ? ValidationResultType.Error : ValidationResultType.Pass });
                result.Add(new ValidationResult() { PropertyName = "InvoiceAddressLine", Type = ValidationType.RequiredField, ResultType = String.IsNullOrEmpty(invoiceAddressLine) ? ValidationResultType.Error : ValidationResultType.Pass });
            }
            else
            {
                result.Add(new ValidationResult() { PropertyName = "InvoiceCountry", Type = ValidationType.Custom, ResultType = ValidationResultType.Pass });

                result.Add(new ValidationResult() { PropertyName = "InvoicePostalCode", Type = ValidationType.RequiredField, ResultType = ValidationResultType.Pass });
                result.Add(new ValidationResult() { PropertyName = "InvoicePostalCode", Type = ValidationType.RegularExpression, ResultType = ValidationResultType.Pass });

                result.Add(new ValidationResult() { PropertyName = "InvoiceTown", Type = ValidationType.RequiredField, ResultType = ValidationResultType.Pass });
                result.Add(new ValidationResult() { PropertyName = "InvoiceAddressLine", Type = ValidationType.RequiredField, ResultType = ValidationResultType.Pass });
            }

            return result.OrderBy(vr => vr.ResultType).ToArray();
        }

        public int UpdateUserPreferences(int personId, string title, string firstName, string lastName, string email, string phone, string fax, bool newsletter, string cultureName)
        {
            Person person = GetPersonById(personId);

            if (person == null) return 1;

            if (!String.IsNullOrEmpty(title) && (title != person.Title))
            {
                LogInformation(null, person.Base.PartnerId, "UpdateUserPreferences", "The property '{0}' was changed from '{1}' to '{2}'.", "Title", person.Title, title);
                person.Title = title;
            }

            if (!String.IsNullOrEmpty(firstName) && (firstName != person.FirstName))
            {
                LogInformation(null, person.Base.PartnerId, "UpdateUserPreferences", "The property '{0}' was changed from '{1}' to '{2}'.", "FirstName", person.FirstName, firstName);
                person.FirstName = firstName;
            }

            if (!String.IsNullOrEmpty(lastName) && (lastName != person.LastName))
            {
                LogInformation(null, person.Base.PartnerId, "UpdateUserPreferences", "The property '{0}' was changed from '{1}' to '{2}'.", "LastName", person.LastName, lastName);
                person.LastName = lastName;
            }

            if (!String.IsNullOrEmpty(email) && (email != person.Email))
            {
                LogInformation(null, person.Base.PartnerId, "UpdateUserPreferences", "The property '{0}' was changed from '{1}' to '{2}'.", "Email", person.Email, email);
                person.Email = email;
            }

            if (!String.IsNullOrEmpty(phone) && (phone != person.Base.Phone))
            {
                LogInformation(null, person.Base.PartnerId, "UpdateUserPreferences", "The property '{0}' was changed from '{1}' to '{2}'.", "Phone", person.Base.Phone, phone);
                person.Base.Phone = phone;
            }

            if (!String.IsNullOrEmpty(fax) && (fax != person.Base.Fax))
            {
                LogInformation(null, person.Base.PartnerId, "UpdateUserPreferences", "The property '{0}' was changed from '{1}' to '{2}'.", "Fax", person.Base.Fax, fax);
                person.Base.Fax = fax;
            }

            if (newsletter != person.Newsletter)
            {
                LogInformation(null, person.Base.PartnerId, "UpdateUserPreferences", "The property '{0}' was changed from '{1}' to '{2}'.", "Newsletter", person.Newsletter, newsletter);
                person.Newsletter = newsletter;
            }

            if (!String.IsNullOrEmpty(cultureName) && (cultureName != person.CultureName))
            {
                LogInformation(null, person.Base.PartnerId, "UpdateUserPreferences", "The property '{0}' was changed from '{1}' to '{2}'.", "CultureName", person.CultureName, cultureName);
                person.CultureName = cultureName;
            }

            try
            {
                context.SaveChanges();
            }
            catch
            {
                return 2;
            }

            return 0;
        }

        public int SendNewsletter(string templateName, int[] recipientPersonIds, object[] parameters)
        {
            int result = 0;
            List<string> recipientEmails = new List<string>();

            foreach (int personId in recipientPersonIds)
            {
                Person person = GetPersonById(personId);

                if ((person == null) || (recipientEmails.Contains(person.Email.ToLower())) || (!person.Newsletter)) continue;

                recipientEmails.Add(person.Email.ToLower());
                List<object> param = new List<object>();
                param.Add(person.FirstName);
                param.Add(person.Email);
                param.Add(person.EmailConfirmationCode);
                param.Add(person.PersonId);
                if (parameters != null) param.AddRange(parameters);

                try
                {
                    SendEmail(templateName, person.CultureName, person.Email, param.ToArray());
                    //SendEmail(templateName, person.CultureName, "andras.fuchs@gmail.com", param.ToArray());
                    result++;
                }
                catch { }
            }

            LogInformation(null, null, "SendNewsletter", "{0} newsletters were sent using the template '{1}'.", result, templateName);

            return result;
        }

        public void OrderRefund(int orderId, decimal refundAmount, List<ProductQuantity> productsReturned)
        {
            Order order = GetOrder(orderId);
            SetOrderStatusInt(orderId, 4096);
            order.Refund = refundAmount;

            // TODO: set the inventory changes

            context.SaveChanges();
        }
    }


    public class MyServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            Uri webServiceAddress = new Uri(ConfigurationManager.AppSettings["MyServiceUri"], UriKind.Absolute);

            ServiceHost webServiceHost = new ServiceHost(serviceType, webServiceAddress);
            return webServiceHost;
        }
    }
}
