using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StopWatch
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public void winCenter (Window _owner)
        {
           
            //  This call is required by the designer.
            InitializeComponent();
            this.Owner = _owner;
            //  Add any initialization after the InitializeComponent() call.
        }
    }
    
}
