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

namespace PaintV3
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) //kdoa bi naj delala mam nareto sam idk zakaj ne....
        {
            if(comboBoxTheme.SelectedItem != null)
            {
                Properties.Settings.Default.Theme = comboBoxTheme.SelectedItem.ToString();
                Properties.Settings.Default.Save();

                string dictionary = "";
                if (Properties.Settings.Default.Theme == "Dark")
                    dictionary = "DarkTheme.xaml";
                else
                    dictionary = "Default.xaml";

                var dict = new Uri(dictionary, UriKind.RelativeOrAbsolute);
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = dict
                });
            }
        }
    }
}
