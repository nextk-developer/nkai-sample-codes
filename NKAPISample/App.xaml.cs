using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;
using FlyleafLib;

namespace NKAPISample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ImportDlls();
           
        }

        private void ImportDlls()
        {
            var basePath = $@"{AppDomain.CurrentDomain.BaseDirectory}dlls";
            var ffmpegDlls = $@"{basePath}\ffmpeg\";

            Engine.Start(new EngineConfig()
            {
                PluginsPath = ":Plugins",
                FFmpegPath = $":{ffmpegDlls}",
                DisableAudio = true,
            });
        }
    }
}
