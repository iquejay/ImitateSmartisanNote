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
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        private NoteManager noteManager = null;
        NotesDataProvider dataProvider = null;
        public MainPage()
        {
            InitializeComponent();
            noteManager = new NoteManager();
            dataProvider = new NotesDataProvider();
            App.NoteManager = noteManager;
            App.DataProvider = dataProvider;
            dataProvider.Init();
            foreach (var item in dataProvider.LoadData())
            {
                noteManager.Notes.Add(item);
            }

            this.DataContext = noteManager;
        }
        private void Star_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                TextBlock tb = (sender as TextBlock);
                var dt = tb.DataContext as NoteItemModel;

                dt.IsStared = !dt.IsStared;
                App.DataProvider.UpdateData(dt);
                e.Handled = true; //停止向上冒泡
            }
        }

        private void contentGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                NotePage notepae = new NotePage((sender as Grid).DataContext as NoteItemModel);
                App.MainFrame.Navigate(notepae);
            }
        }
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            NoteItemModel noteitem = new NoteItemModel(DateTime.Now, "") { Identity = GetTimeStamp() };
            NotePage notepae = new NotePage(noteitem);
            App.MainFrame.Navigate(notepae);
        }

        private void Delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var item = (sender as Grid).DataContext as NoteItemModel;
                noteManager.Notes.Remove(item);
                dataProvider.DeleteData(item);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            noteManager.Notes.Clear();
            string filter = "";
            if (!string.IsNullOrEmpty(tb.Text))
            {
                
                filter = "where Note like \"%" + tb.Text + "%\"";
            }
            foreach (var item in dataProvider.LoadData(filter))
            {
                noteManager.Notes.Add(item);
            }
        }
    }
}
