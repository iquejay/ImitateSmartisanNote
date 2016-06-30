using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ImitateSmartisanNote
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static System.Windows.Controls.Frame MainFrame = null;
        public static NotesDataProvider DataProvider = null;
        public static NoteManager NoteManager = null;
    }
}
