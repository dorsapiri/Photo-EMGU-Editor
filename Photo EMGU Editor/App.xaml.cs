using Photo_EMGU_Editor.View;
using Photo_EMGU_Editor.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Photo_EMGU_Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SignUpV signUpV = new SignUpV();
            SignUpVM signUpVM = new SignUpVM();
            signUpV.DataContext = signUpVM;
            signUpV.ShowDialog();
        }
    }
}
