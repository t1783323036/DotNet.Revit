using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.RevitUI;
using DotNet.RevitUI.MVVM;
using Test.DotNet.RevitUI.Command;

namespace Test.DotNet.RevitUI.ViewModel
{
    public class RibbonViewModel : ObservableObject
    {
        /// <summary>
        /// 删除元素 测试
        /// </summary>

        public RevitCommand DeleteElement { get; set; }

        /// <summary>
        /// 删除匹配元素 测试
        /// </summary>
        public RevitCommand DeleteMatchElement { get; set; }

        /// <summary>
        /// This is command test 
        /// </summary>
        public RevitCommand SplitButton1 { get; set; }

        /// <summary>
        /// This is command test 
        /// </summary>
        public RevitCommand SplitButton2 { get; set; }

        /// <summary>
        /// This is command test 
        /// </summary>
        public RevitCommand FlowButton1 { get; set; }

        /// <summary>
        /// This is command test 
        /// </summary>
        public RevitCommand FlowButton2 { get; set; }


        public RibbonViewModel()
        {
            this.DeleteElement = new DeleteElementCommand();

            this.DeleteMatchElement = new DeleteMatchElementCommand();

            this.SplitButton1 = new SplitButton1Command();

            this.SplitButton2 = new SplitButton1Command();

            this.FlowButton1 = new FlowButton1Command();

            this.FlowButton2 = new FlowButton1Command();
        }
    }
}
