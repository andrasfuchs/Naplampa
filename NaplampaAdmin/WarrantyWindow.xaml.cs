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
using NaplampaAdmin.Util;

namespace NaplampaAdmin
{
    /// <summary>
    /// Interaction logic for WarrantyWindow.xaml
    /// </summary>
    public partial class WarrantyWindow : Window
    {
        private Order order;
        private ProductQuantity[] basket
        {
            get
            {
                List<ProductQuantity> result = new List<ProductQuantity>();

                if (napsugarSlider.Value > 0) result.Add(new ProductQuantity() { ProductId = (int)napsugarSlider.Tag, Quantity = (int)napsugarSlider.Value });
                if (kristalysugarSlider.Value > 0) result.Add(new ProductQuantity() { ProductId = (int)kristalysugarSlider.Tag, Quantity = (int)kristalysugarSlider.Value });
                if (kristalysugarE14Slider.Value > 0) result.Add(new ProductQuantity() { ProductId = (int)kristalysugarE14Slider.Tag, Quantity = (int)kristalysugarE14Slider.Value });

                return result.ToArray();
            }
        }

        public WarrantyWindow(Order order)
        {
            InitializeComponent();

            this.order = order;

            orderIdTextBox.Text = order.OrderId.ToString();
            nameTextBox.Text = order.Partner.ContactPerson.FirstName + " " + order.Partner.ContactPerson.LastName;

            OrderProductXRef quantity = order.OrderProductXRefs.FirstOrDefault<OrderProductXRef>(opx => opx.Product.Code == "ES004");
            napsugarSlider.IsEnabled = (quantity != null);
            if (quantity != null) 
            {
                napsugarSlider.Maximum = quantity.Quantity;
                napsugarSlider.Tag = quantity.ProductId;
            }

            quantity = order.OrderProductXRefs.FirstOrDefault<OrderProductXRef>(opx => opx.Product.Code == "ES002");
            kristalysugarSlider.IsEnabled = (quantity != null);
            if (quantity != null)
            {
                kristalysugarSlider.Maximum = quantity.Quantity;
                kristalysugarSlider.Tag = quantity.ProductId;
            }
            
            quantity = order.OrderProductXRefs.FirstOrDefault<OrderProductXRef>(opx => opx.Product.Code == "ES015");
            kristalysugarE14Slider.IsEnabled = (quantity != null);
            if (quantity != null)
            {
                kristalysugarE14Slider.Maximum = quantity.Quantity;
                kristalysugarE14Slider.Tag = quantity.ProductId;
            }
        }

        protected void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (basket.Count() == 0) return;

            ServiceManager.NaplampaService.CreateWarrantyOrder(order.OrderId, basket);

            this.Close();
        }
    }
}
