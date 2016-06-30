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

namespace ImitateSmartisanNote
{
    /// <summary>
    /// NotePage.xaml 的交互逻辑
    /// </summary>
    public partial class NotePage : Page
    {
        private NoteItemModel Item
        {
            get;set;
        }
        private string srcNote;
        private bool srcStar;
        public NotePage()
        {
            InitializeComponent();
        }
        public NotePage(NoteItemModel dataContext):this()
        {
            Item = dataContext;
            srcNote = Item.Note;
            srcStar = Item.IsStared;
            this.DataContext = Item;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (srcNote == "" && Item.Note.Trim()!="")
            {
                App.NoteManager.Notes.Insert(0, Item);
                App.DataProvider.InsertData(Item);
            }
            if (srcNote != "" && Item.Note.Trim() == "")
            {
                App.NoteManager.Notes.Remove(Item);
                App.DataProvider.DeleteData(Item);
            }
            if (string.Compare(srcNote, Item.Note) != 0)
            {
                Item.DateAndTime = DateTime.Now;
                App.DataProvider.UpdateData(Item, true);
            }
            else
            {
                if (srcStar != Item.IsStared)
                {
                    App.DataProvider.UpdateData(Item);
                }
            }
            if (App.MainFrame.CanGoBack)
                App.MainFrame.GoBack();
        }

        private void Star_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                TextBlock tb = (sender as TextBlock);
                var dt = tb.DataContext as NoteItemModel;
                dt.IsStared = !dt.IsStared;
                e.Handled = true; //停止向上冒泡
            }
        }
    }
}
