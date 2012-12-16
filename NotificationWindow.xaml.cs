using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LoginForm
{
    /// <summary>
    /// Логика взаимодействия для NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        public NotificationWindow()
        {
            InitializeComponent();
            
            List.Items.Add(new { ImagePath = "pack://application:,,,/hat.png", Title = "What is next?", Skills = "123,123,123" });
            List.Items.Add(new { ImagePath = "pack://application:,,,/hat.png", Title = "What is next?", Skills = "123,123,123" });

        }
        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void List_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                var view = List.View as GridView;

                if (view != null)
                {
                    double width = List.ActualWidth / view.Columns.Count;

                    view.Columns[0].Width = width;
                }

                //foreach (GridViewColumn col in view.Columns)
            }

           
        }
        private void Icon_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is ListViewItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            var item = List.ItemContainerGenerator.IndexFromContainer(dep);

            
        }

    }
    internal class UrlToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new BitmapImage(new Uri(value.ToString(), UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
