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

namespace Translator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TranslationType.SelectionChanged+=TranslationType_SelectionChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextToTranslate.Text)) return;
            var text = TextToTranslate.Text.Replace(".","");
            var trans = TranslationType.SelectedIndex == 0 ? Utilities.Translator.TranslateGoogle(text, "en", "de") : Utilities.Translator.TranslateGoogle(text, "de", "en");
            LastTranslation.Text = text + "  :  " + trans;
            if (SendToGame.IsChecked == true)
            {
                Utilities.Keys.SendToGame(trans);
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void Grid_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TranslationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SendToGame.IsChecked = TranslationType.SelectedIndex == 0 ? true : false;
        }
    }
}
