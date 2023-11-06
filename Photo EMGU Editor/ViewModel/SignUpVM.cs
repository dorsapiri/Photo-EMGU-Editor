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
    public class SignUpVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ICommand alreadyAmember_login { get; set; }
        public ICommand btnSignUp { get; set; }
        public SignUpVM()
        {
            alreadyAmember_login = new RelayCommand(alreadyAmember_login_Click, canalreadyAmemberExecute);
            btnSignUp = new RelayCommand(btnSignUp_Click, btnSignUpCanExecute);
        }
        public void alreadyAmember_login_Click(Object sender)
        {
            LoginVM loginVM = new LoginVM();
            LoginV loginV = new LoginV();
            loginV.DataContext = loginVM;
            CloseWindow();
            loginV.ShowDialog();
        }
        public bool canalreadyAmemberExecute(object sender)
        {
            return true;
        }
        public void CloseWindow()
        {
            Application.Current.MainWindow.Close();
        }

        private string _tbFirstName;
        public string tbFirstName
        {
            get { return _tbFirstName; }
            set
            {
                _tbFirstName = value;
                OnPropertyChanged(nameof(tbFirstName));

            }
        }

        private string _tbLastName;
        public string tbLastName
        {
            get { return _tbLastName; }
            set
            {
                _tbLastName = value;
                OnPropertyChanged(nameof(tbLastName));

            }
        }

        private string _tbUserName;
        public string tbUserName
        {
            get { return _tbUserName; }
            set
            {
                _tbUserName = value;
                OnPropertyChanged(nameof(tbUserName));

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
        public bool btnSignUpCanExecute(object sender) { return true; }
        public void btnSignUp_Click(Object sender)
        {
            using (DatabaseConnection db = new DatabaseConnection())
            {
                User NewUser = new User()
                {
                    Name = tbFirstName,
                    Lastname = tbLastName,
                    UserName = tbUserName,
                    Password = tbPassword
                };

                db.IUserDataAccess.insertUser(NewUser);
            }
            LoginVM loginVM = new LoginVM();
            LoginV loginV = new LoginV();
            loginV.DataContext = loginVM;
            CloseWindow();
            loginV.ShowDialog();

        }
    }
}
