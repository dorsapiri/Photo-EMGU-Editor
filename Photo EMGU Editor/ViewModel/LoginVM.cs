using Emgu.CV;
using Photo_EMGU_Editor.Helper;
using Photo_EMGU_Editor.Model;
using Photo_EMGU_Editor.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Photo_EMGU_Editor.ViewModel
{
    public class LoginVM : INotifyPropertyChanged
    {
        public User currentUser;
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ICommand loginCommand { get; set; }
        public ICommand goToSignUp { get; set; }
        public ICommand cancelWindow { get; set; }
        public LoginVM()
        {
            loginCommand = new RelayCommand(btnSignIn_Click, canSignIn);
            goToSignUp = new RelayCommand(lnkGoToSignUp, canGoToSignUo);
            cancelWindow = new RelayCommand(btnCancel_Click, canCancel);
        }

        private bool canCancel(object sender)
        {
            return true;
        }

        private void btnCancel_Click(object sender)
        {
            Application.Current.Shutdown();
        }

        private bool canGoToSignUo(object sender)
        {
            return true;
        }

        private void lnkGoToSignUp(object sender)
        {
            SignUpV signUpV = new SignUpV();
            SignUpVM signUpVM = new SignUpVM();
            signUpV.DataContext = signUpVM;
            if(sender is Window window)
            {
                window.Close();
            }
            signUpV.ShowDialog();
        }

        private string _tbUsername;
        public string tbUsername
        {
            get { return _tbUsername; }
            set
            {
                _tbUsername = value;
                OnPropertyChanged(nameof(tbUsername));
            }
        }

        private string _tbPassword;
        public string tbPassword
        {
            get { return _tbPassword; }
            set
            {
                _tbPassword = value;
                OnPropertyChanged(nameof(tbPassword));
            }
        }
        private string _tbError;

        public string tbError
        {
            get { return _tbError; }
            set 
            { 
                _tbError = value; 
                OnPropertyChanged(nameof(tbError));
            }
        }


        private bool canSignIn(object sender) { return !string.IsNullOrEmpty(tbPassword) && !string.IsNullOrEmpty(tbUsername); }
        public void btnSignIn_Click(object sender)
        {
            using (DatabaseConnection db = new DatabaseConnection())
            {
                User loginUser = db.IUserDataAccess.selectUser(tbUsername, tbPassword);
                if (loginUser != null)
                {
                    currentUser = loginUser;
                    ImageProcessingV mainWindow = new ImageProcessingV();
                    ImageProcessingVM mainVM = new ImageProcessingVM(currentUser);
                    mainWindow.DataContext = mainVM;


                    if (sender is Window window)
                    {
                        window.Close();
                    }
                    mainWindow.ShowDialog();
                }
                else
                {
                    tbError = "This User is not valid!";
                }
            }

        }

        //static string ConvertToUnsecureString(SecureString securePassword)
        //{
        //    IntPtr unmanagedString = IntPtr.Zero;

        //    try
        //    {
        //        unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
        //        return Marshal.PtrToStringUni(unmanagedString);
        //    }
        //    finally
        //    {
        //        Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
        //    }
        //}

    }
}
