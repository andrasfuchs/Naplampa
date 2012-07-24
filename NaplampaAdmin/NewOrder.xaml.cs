using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NaplampaAdmin.NaplampaService;
using System.Collections.ObjectModel;
using NaplampaAdmin.GlobalResources;
using NaplampaAdmin.Util;

namespace NaplampaAdmin
{
    /// <summary>
    /// Interaction logic for NewOrder.xaml
    /// </summary>
    public partial class NewOrder : Window
    {
        private Product[] products;

        public NewOrder(Order prefillOrder)
        {
            InitializeComponent();

            PaymentMethodComboBox.ItemsSource = PaymentMethods.ResourceManager.ToIntArray("PAYMENTMETHOD_");

            Country[] countries = ServiceManager.NaplampaService.ListCountries();
            Currency[] currencies = ServiceManager.NaplampaService.ListCurrencies();
            products = ServiceManager.NaplampaService.ListProducts();

            // TODO: localize 'countries' and 'currencies'

            CountryComboBox.ItemsSource = countries;
            InvoiceCountryComboBox.ItemsSource = countries;
            CurrencyComboBox.ItemsSource = currencies;

            if (prefillOrder != null)
            {
                prefillOrder = ServiceManager.NaplampaService.GetOrder(prefillOrder.OrderId);

                FirstNameTextBox.Text = prefillOrder.Partner.ContactPerson.FirstName;
                LastNameTextBox.Text = prefillOrder.Partner.ContactPerson.LastName;
                EmailTextBox.Text = prefillOrder.Partner.ContactPerson.Email;
                PhoneTextBox.Text = prefillOrder.Partner.Phone;
                
                CountryComboBox.SelectedValue = prefillOrder.DeliveryAddress.Country.CountryId;
                AddressLineTextBox.Text = prefillOrder.DeliveryAddress.AddressLine;
                TownTextBox.Text = prefillOrder.DeliveryAddress.Town;
                PostalCodeTextBox.Text = prefillOrder.DeliveryAddress.PostalCode;

                Invoice invoice = prefillOrder.Invoices.OrderBy(i => i.DateOfInvoice).Last();
                invoice = ServiceManager.NaplampaService.GetInvoice(invoice.InvoiceId);

                InvoiceNameTextBox.Text = invoice.BuyerBillingAddress.Name;
                InvoiceCountryComboBox.SelectedValue = invoice.BuyerBillingAddress.Country.CountryId;
                InvoiceAddressLineTextBox.Text = invoice.BuyerBillingAddress.AddressLine;
                InvoiceTownTextBox.Text = invoice.BuyerBillingAddress.Town;
                InvoicePostalCodeTextBox.Text = invoice.BuyerBillingAddress.PostalCode;

                RefererTextBox.Text = prefillOrder.Referer;
                NewsletterCheckBox.IsChecked = prefillOrder.Partner.ContactPerson.Newsletter;

                CouponTextBox.Text = prefillOrder.DiscountCode;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<ProductQuantity> basket = new List<ProductQuantity>();
            int tempInt;
            if (Int32.TryParse(SunBeamTextBox.Text, out tempInt))
            {
                basket.Add(new ProductQuantity() { ProductId = products.First(p => p.Code == "ES004").ProductId, Quantity = tempInt });
            }

            if (Int32.TryParse(CrystalBeamTextBox.Text, out tempInt))
            {
                basket.Add(new ProductQuantity() { ProductId = products.First(p => p.Code == "ES002").ProductId, Quantity = tempInt });
            }

            if (Int32.TryParse(CrystalBeamE14TextBox.Text, out tempInt))
            {
                basket.Add(new ProductQuantity() { ProductId = products.First(p => p.Code == "ES015").ProductId, Quantity = tempInt });
            }

            ServiceManager.NaplampaService.CreateNewOrderWithoutLogin(
                "", FirstNameTextBox.Text, LastNameTextBox.Text, EmailTextBox.Text, PhoneTextBox.Text, NewsletterCheckBox.IsChecked.Value, "hu-HU"
                , (int)CountryComboBox.SelectedValue, "", TownTextBox.Text, PostalCodeTextBox.Text, AddressLineTextBox.Text
                , (short)(int)PaymentMethodComboBox.SelectedValue, null, basket.ToArray(), (int)CurrencyComboBox.SelectedValue
                , (int)InvoiceCountryComboBox.SelectedValue, "", InvoiceTownTextBox.Text, InvoicePostalCodeTextBox.Text, InvoiceAddressLineTextBox.Text, InvoiceNameTextBox.Text
                , (PromoCheckBox.IsChecked.Value ? 2 + 4 + 8 + 512 : (int?)null), CouponTextBox.Text);

            this.Close();
        }
    }
}
