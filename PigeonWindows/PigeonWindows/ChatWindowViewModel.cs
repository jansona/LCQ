using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PigeonWindows
{
    class ChatWindowViewModel : BindableBase
    {
        #region delegates
        //窗口关闭
        public DelegateCommand CloseCommand { get; set; }
        #endregion

        #region constructor
        public ChatWindowViewModel()
        {
            CloseCommand = new DelegateCommand(() => {
                Application.Current.Shutdown();
            });
        }
        #endregion
    }
}
