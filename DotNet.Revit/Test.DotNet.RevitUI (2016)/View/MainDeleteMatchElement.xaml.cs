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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DotNet.RevitUI.MVVM;
using Test.DotNet.RevitUI.ViewModel;

namespace Test.DotNet.RevitUI.View
{
    /// <summary>
    /// MainSplitWall.xaml 的交互逻辑
    /// </summary>
    public partial class MainDeleteMatchElement : Window
    {
        internal const string ClosedToken = "ClosedToken";

        public MainDeleteMatchElement()
        {
            InitializeComponent();

            Messenger.Default.Register<DeleteElementViewModel>(this, ClosedToken, m => this.Close());
        }
    }
}
