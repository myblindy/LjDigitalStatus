using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LjDigitalStatus
{
    /// <summary>
    /// Interaction logic for BitControl.xaml
    /// </summary>
    public partial class BitControl : UserControl
    {
        public bool SelfEditable
        {
            get { return (bool)GetValue(SelfEditableProperty); }
            set { SetValue(SelfEditableProperty, value); }
        }

        public static readonly DependencyProperty SelfEditableProperty =
            DependencyProperty.Register("SelfEditable", typeof(bool), typeof(BitControl));

        public BitControl()
        {
            InitializeComponent();
        }

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelfEditable)
            {
                var dimodel = ((DigitalInputModel)DataContext);
                dimodel.IsOn = !dimodel.IsOn;
            }
        }
    }
}
