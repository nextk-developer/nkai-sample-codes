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

namespace NKAPISample.Controls
{
    /// <summary>
    /// LabelledTextBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LabelledTextBox : UserControl
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(LabelledTextBox));
        public static readonly DependencyProperty HeaderWidthProperty = DependencyProperty.Register("HeaderWidth", typeof(int), typeof(LabelledTextBox), new FrameworkPropertyMetadata(100));
        public static readonly DependencyProperty HeaderSizeProperty = DependencyProperty.Register("HeaderSize", typeof(int), typeof(LabelledTextBox), new FrameworkPropertyMetadata(11));
        public static readonly DependencyProperty HeaderColorProperty = DependencyProperty.Register("HeaderColor", typeof(Brush), typeof(LabelledTextBox), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(66, 66, 66))));
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(int), typeof(LabelledTextBox), new FrameworkPropertyMetadata(24));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LabelledTextBox));

        public string Header { get => (string)GetValue(HeaderProperty); set => SetValue(HeaderProperty, value); }
        public int HeaderWidth { get => (int)GetValue(HeaderWidthProperty); set => SetValue(HeaderWidthProperty, value); }
        public int HeaderSize { get => (int)GetValue(HeaderSizeProperty); set => SetValue(HeaderSizeProperty, value); }
        public Brush HeaderColor { get => (Brush)GetValue(HeaderColorProperty); set => SetValue(HeaderColorProperty, value); }
        public int Height { get => (int)GetValue(HeightProperty); set => SetValue(HeightProperty, value); }
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }


        public LabelledTextBox()
        {
            InitializeComponent();
        }

       
        
    }
}
