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

namespace NaplampaAdmin
{
    /// <summary>
    /// Interaction logic for Refund.xaml
    /// </summary>
    public partial class Refund : Window
    {
        public Refund(Order refundOrder)
        {
            InitializeComponent();
        }
    }
}
