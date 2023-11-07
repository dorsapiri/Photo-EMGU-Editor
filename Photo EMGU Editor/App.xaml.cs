using Photo_EMGU_Editor.Model;
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
            /*SignUpV signUpV = new SignUpV();
            SignUpVM signUpVM = new SignUpVM();
            signUpV.DataContext = signUpVM;
            signUpV.ShowDialog();*/

            LoginV loginV = new LoginV();
            LoginVM loginVM = new LoginVM();
            loginV.DataContext = loginVM;
            loginV.ShowDialog();

            /*ImageProcessingV imageProcessingV = new ImageProcessingV();
            User user = new User() {
                UserName = "dorsapiri",
                Id= 1,
                Name="Dorsa",
                Password="1234",
                Lastname = "Piri"
            };
            ImageProcessingVM imageProcessingVM = new ImageProcessingVM(user);
            imageProcessingV.DataContext = imageProcessingVM;
            imageProcessingV.ShowDialog();*/
        }
    }
}
