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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NaplampaAdmin.NaplampaService;
using NaplampaAdmin.Util;
using NaplampaAdmin.GlobalResources;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace NaplampaAdmin
{
    public partial class GridWindow : Window
    {
        private Person currentUser = null;
        private int lastOrderId = -1;

        public GridWindow(Person person)
        {
            InitializeComponent();

            currentUser = person;

            statusColumn.ItemsSource = OrderStatuses.ResourceManager.ToIntArray("ORDERSTATUS_");
            paymentMethodColumn.ItemsSource = PaymentMethods.ResourceManager.ToIntArray("PAYMENTMETHOD_");
            countryColumn.ItemsSource = new ObservableCollection<KeyValuePair<string, string>>(Countries.ResourceManager.ToArray(null));
            currencyComboBox.ItemsSource = new ObservableCollection<KeyValuePair<string, string>>(Currencies.ResourceManager.ToArray(null));

            sentDatePicker.SelectedDate = DateTime.Now.Date;

            foreach (Survey survey in ServiceManager.NaplampaService.ListSurveys(true))
            {
                surveyComboBox.Items.Add(survey);
            }

            ReloadOrderList();

            DispatcherTimer emailServiceTimer = new DispatcherTimer();
            emailServiceTimer.Interval = new TimeSpan(0, 0, 10);
            emailServiceTimer.IsEnabled = true;
            emailServiceTimer.Tick += new EventHandler(emailServiceTimer_Tick);
        }

        void emailServiceTimer_Tick(object sender, EventArgs e)
        {
            RefreshEmailServiceState();
        }

        private void SentButton_Click(object sender, RoutedEventArgs e)
        {
            Order selectedOrder = (Order)orderDataGrid.SelectedItem;

            ServiceManager.NaplampaService.OrderPackageSent(selectedOrder.OrderId, currentUser.PersonId, (sentDatePicker.SelectedDate.HasValue ? sentDatePicker.SelectedDate.Value : DateTime.UtcNow), trackingNumberTextBox.Text);

            //ReloadOrderList();
        }

        private void SurveyButton_Click(object sender, RoutedEventArgs e)
        {
            Order selectedOrder = (Order)orderDataGrid.SelectedItem;

            ServiceManager.NaplampaService.SendSurveyRequest(selectedOrder.OrderId, (int)surveyComboBox.SelectedValue);

            //ReloadOrderList();
        }

        private void WarrantyButton_Click(object sender, RoutedEventArgs e)
        {
            Order selectedOrder = (Order)orderDataGrid.SelectedItem;

            WarrantyWindow warrantyWindow = new WarrantyWindow(selectedOrder);
            warrantyWindow.ShowDialog();
            
            //ReloadOrderList();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadOrderList();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void orderDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (orderDataGrid.SelectedItem == null)
            {
                paymentRequestButton.IsEnabled =
                paymentReceivedButton.IsEnabled =
                deleteButton.IsEnabled =
                sentBorder.IsEnabled =
                surveyBorder.IsEnabled = false;

                return;
            }

            if (((Order)orderDataGrid.SelectedItem).OrderId == lastOrderId) return;

            
            Order selectedOrder = ServiceManager.NaplampaService.GetOrder(((Order)orderDataGrid.SelectedItem).OrderId);
            lastOrderId = ((Order)orderDataGrid.SelectedItem).OrderId;

            paymentRequestButton.IsEnabled = ((selectedOrder.OrderStatus & 1) == 1) && ((selectedOrder.OrderStatus & 2) != 2);
            paymentReceivedButton.IsEnabled = ((selectedOrder.OrderStatus & 2) == 2) && ((selectedOrder.OrderStatus & 4) != 4);
            deleteButton.IsEnabled = ((selectedOrder.OrderStatus & 65536) != 65536);
            sentBorder.IsEnabled = ((selectedOrder.OrderStatus & 8) == 8) && ((selectedOrder.OrderStatus & 16) != 16);
            surveyBorder.IsEnabled = true;
            surveyButton.IsEnabled = ((selectedOrder.OrderStatus & 16) == 16) && ((selectedOrder.OrderStatus & 64) != 64);
            resendButton.IsEnabled = ((selectedOrder.OrderStatus & 2) == 2) && ((selectedOrder.OrderStatus & 4) != 4);
            returnedButton.IsEnabled = ((selectedOrder.OrderStatus & 16) == 16) && ((selectedOrder.OrderStatus & 32) != 32);
            warrantyButton.IsEnabled = ((selectedOrder.OrderStatus & 16) == 16);

            excelBorder.DataContext = selectedOrder;

            foreach (KeyValuePair<string, string> c in Countries.ResourceManager.ToArray(null))
            {
                CustomerInfoTextBox.Text = CustomerInfoTextBox.Text.Replace(c.Key, c.Value);
            }

            OrderProductXRef opx = selectedOrder.OrderProductXRefs.FirstOrDefault<OrderProductXRef>(op => op.Product.Code == "ES004");
            napsugarTextBox.Text = (opx == null ? "0" : opx.Quantity.ToString());
            opx = selectedOrder.OrderProductXRefs.FirstOrDefault<OrderProductXRef>(op => op.Product.Code == "ES002");
            kristalysugarTextBox.Text = (opx == null ? "0" : opx.Quantity.ToString());
            opx = selectedOrder.OrderProductXRefs.FirstOrDefault<OrderProductXRef>(op => op.Product.Code == "ES015");
            kristalysugarE14TextBox.Text = (opx == null ? "0" : opx.Quantity.ToString());
            opx = selectedOrder.OrderProductXRefs.FirstOrDefault<OrderProductXRef>(op => op.Product.Code == "ES002W");
            kristalysugarMelegTextBox.Text = (opx == null ? "0" : opx.Quantity.ToString());
            opx = selectedOrder.OrderProductXRefs.FirstOrDefault<OrderProductXRef>(op => op.Product.Code == "ES015W");
            kristalysugarE14MelegTextBox.Text = (opx == null ? "0" : opx.Quantity.ToString());


            if (selectedOrder.Invoices.Length > 0)
            {
                //Invoice invoice = selectedOrder.Invoices.OrderBy(i => i.DateOfInvoice).Last();
                //totalTextBox.Text = invoice.Currency.Prefix + invoice.Total.ToString("0.00") + " " + invoice.Currency.Postfix;
                totalTextBox.Text = selectedOrder.Currency.Prefix + selectedOrder.OrderTotal.ToString("0.00") + " " + selectedOrder.Currency.Postfix;
            }
            else
            {
                totalTextBox.Text = "N/A";
            }
        }

        private void ReloadOrderList()
        {
            orderDataGrid.ItemsSource = ServiceManager.NaplampaService.ListOrders(/*person.Base.PartnerId*/ null, DateTime.Now.AddDays(-70));

            orderDataGrid.Columns[1].SortDirection = System.ComponentModel.ListSortDirection.Ascending;
        }

        private void paymentReceivedButton_Click(object sender, RoutedEventArgs e)
        {
            Order selectedOrder = (Order)orderDataGrid.SelectedItem;

            ServiceManager.NaplampaService.OrderPaymentReceived(selectedOrder.OrderId, selectedOrder.PayPalToken);
        }

        private void ReturnedButton_Click(object sender, RoutedEventArgs e)
        {
            Order selectedOrder = (Order)orderDataGrid.SelectedItem;

            ServiceManager.NaplampaService.OrderReturned(selectedOrder.OrderId);
        }


        private void ResendButton_Click(object sender, RoutedEventArgs e)
        {
            Order selectedOrder = (Order)orderDataGrid.SelectedItem;

            ServiceManager.NaplampaService.OrderResendPaymentRequest(selectedOrder.OrderId);
        }
        
        private void paymentRequestButton_Click(object sender, RoutedEventArgs e)
        {
            Order selectedOrder = (Order)orderDataGrid.SelectedItem;

            ServiceManager.NaplampaService.OrderPaymentRequestSent(selectedOrder.OrderId);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Order selectedOrder = (Order)orderDataGrid.SelectedItem;

            ServiceManager.NaplampaService.DeleteOrder(selectedOrder.OrderId);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            orderDataGrid.ItemsSource = ServiceManager.NaplampaService.SearchOrders(/*person.Base.PartnerId*/ null, searchTextBox.Text, Int32.Parse(rankComboBox.Text), 0, 0, showDeleted.IsChecked.Value, showNewsletter.IsChecked);

            orderDataGrid.Columns[1].SortDirection = System.ComponentModel.ListSortDirection.Ascending;
        }

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Order prefillOrder = null;
            if (orderDataGrid.SelectedItems.Count > 0)
            {
                prefillOrder = (Order)orderDataGrid.SelectedItems[0];
            }

            NewOrder newOrderWindow = new NewOrder(prefillOrder);
            newOrderWindow.ShowDialog();
        }

        private void NewsletterButton_Click(object sender, RoutedEventArgs e)
        {
            Order[] orders = (Order[])orderDataGrid.ItemsSource;

            if (orderDataGrid.SelectedItems.Count > 0)
            {
                orders = (Order[])orderDataGrid.SelectedItems.Cast<Order>().ToArray();
            }

            if (MessageBox.Show("Biztos vagy benne, hogy elkuldod a 'CHRISTMAS2009' hirlevelet ennek a " + orders.Length + " vasarlonak?", "Hirlevel", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            List<int> recipientPersonIds = new List<int>();
            foreach (Order order in orders)
            {
                recipientPersonIds.Add(order.Partner.ContactPerson.PersonId);
            }

            int result = ServiceManager.NaplampaService.SendNewsletter("CHRISTMAS2009", recipientPersonIds.ToArray(), null);

            MessageBox.Show("A 'CHRISTMAS2009' hirlevel " + result + " vasarlonak el lett kuldve.", "Hirlevel", MessageBoxButton.OK);
        }

        private void RefundButton_Click(object sender, RoutedEventArgs e)
        {
            Order refundOrder = null;
            if (orderDataGrid.SelectedItems.Count > 0)
            {
                refundOrder = (Order)orderDataGrid.SelectedItems[0];
            }

            Refund refundWindow = new Refund(refundOrder);
            refundWindow.ShowDialog();
        }

        private void DisableEmailButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceManager.NaplampaService.DisableEmailService();
            RefreshEmailServiceState();
        }

        private void EnableEmailButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceManager.NaplampaService.EnableEmailService();
            RefreshEmailServiceState();
        }

        private void RefreshEmailServiceState()
        {
            if (ServiceManager.NaplampaService.IsEmailServiceDisabled())
            {
                EmailServiceLabel.Content = "disabled";
                disableEmailButton.IsEnabled = false;
                enableEmailButton.IsEnabled = true;
            }
            else
            {
                EmailServiceLabel.Content = "enabled";
                disableEmailButton.IsEnabled = true;
                enableEmailButton.IsEnabled = false;
            }
        }
    }
}
