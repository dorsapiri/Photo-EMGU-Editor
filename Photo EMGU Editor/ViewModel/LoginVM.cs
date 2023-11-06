using Emgu.CV;
using Photo_EMGU_Editor.Helper;
using Photo_EMGU_Editor.Model;
using Photo_EMGU_Editor.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public LoginVM()
        {
            loginCommand = new RelayCommand(btnSignIn_Click, canSignIn);
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
            }

        }

    }
}
