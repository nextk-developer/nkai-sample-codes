using System;
using System.Windows;
using System.Windows.Forms;

namespace NKAPISample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetScreenSize();
        }

        /// <summary>
        /// 모니터 해상도에 맞춰 창 크기 세팅
        /// </summary>
        private void SetScreenSize()
        {
            if (Screen.PrimaryScreen == null || Screen.PrimaryScreen.WorkingArea == null)
                return;

            if (Screen.PrimaryScreen.WorkingArea.Width >= 1920)
            {
                Width = 1920;
                Height = 1032; // 윈도우 11기준 시작표시줄 제외 높이
            }
            else if (Screen.PrimaryScreen.WorkingArea.Width >= 1280)
            {
                Width = 1280;
                Height = 1024;
            }
            else if(Screen.PrimaryScreen.WorkingArea.Width >= 1024)
            {
                Width = 1024; 
                Height = 768;
            }

        }
    }
}
