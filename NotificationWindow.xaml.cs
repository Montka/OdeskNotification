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
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Threading;



namespace LoginForm
{
    /// <summary>
    /// Логика взаимодействия для NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        JobParserClass jobParserClass = new JobParserClass();
        int position = 0;
        public NotificationWindow()
        {
            InitializeComponent();

            var t = jobParserClass.timelyJobs;
            Title.Text = t.At(position).Title;
            Description.Text = t.At(position).Description;
            Type.Content = t.At(position).Type;
            Amount.Content = t.At(position).Amount;

            Timer tm = new Timer(Callback, null, TimeSpan.FromSeconds(420),
                TimeSpan.FromSeconds(420));
        }
        private void Callback(object param)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action<string>)DoWork,
            "some string");
        }
        void DoWork(string someArg)
        {
            if (jobParserClass._oauthAccessToken == null)
                return;

            Upate();
        }
        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Icon_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var sb = (Storyboard)this.FindResource("FadeOutStoryboard");
            sb.Begin();
        }

        private void FadeOutStoryboard_Completed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {

            var m = new MainWindow(ref jobParserClass);
            this.Hide();
            m.ShowDialog();
            this.Show();
        }
        void Upate()
        {
            var t = jobParserClass.timelyJobs;
            Title.Text = t.At(position).Title;
            Description.Text = t.At(position).Description;
            Type.Content = t.At(position).Type;
            Amount.Content = t.At(position).Amount;

            position++;
            if (position >= jobParserClass.timelyJobs.Count())
                position = 0;
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Upate();
        }

        private void Timeline_OnCompleted(object sender, EventArgs e)
        {
            ButtonClickRight.Source = new BitmapImage(new Uri(@"R:\WPF\LoginForm\2.png"));
            var sb = (Storyboard)this.FindResource("MouseOverAnimation");
            sb.Begin();
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
