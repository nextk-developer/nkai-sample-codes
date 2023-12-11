using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using FlyleafLib;
using CommunityToolkit.Mvvm.DependencyInjection;
using NKAPISample.ViewModels;

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
            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton<MainViewModel>()
                .AddSingleton<ComputingNodeViewModel>()
                .AddSingleton<ChannelViewModel>()
                .AddSingleton<ROIViewModel>()
                .AddSingleton<ScheduleViewModel>()
                .AddSingleton<VAViewModel>()
                .AddSingleton<VideoViewModel>()
                .BuildServiceProvider()
                );
           
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
