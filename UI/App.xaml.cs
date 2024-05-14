using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UI.Infrastructure;
using UI.View;
using UI.ViewModel;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Logger
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            // Set localization
            Localizer.Instance.Set(Settings.Current.Locale);

        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var error = e.ExceptionObject as Exception;
            if (error == null)
                return;
        }

    }
}
