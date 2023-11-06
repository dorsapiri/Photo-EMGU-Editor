using Photo_EMGU_Editor.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Photo_EMGU_Editor.ViewModel.DataAccess
{
    public class UserDataAccess : IUserDataAccess
    {
        SQLiteConnection db;
        public ObservableCollection<User> users;

        public UserDataAccess(SQLiteConnection connection)
        {
            db = connection;
            users = new ObservableCollection<User>();
            selectAll();
        }
        public bool deleteUser(User user)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool insertUser(User user)
        {
            try
            {
                db.Insert(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool selectAll()
        {
            try
            {
                db.CreateTable<User>();
                var usersTable = db.Table<User>().ToList();
                foreach (var user in usersTable)
                {
                    users.Add(user);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public User selectUser(string username, string password)
        {
            try
            {
                return users.First(user => user.UserName == username && user.Password == password);
                /*db.FindWithQuery<User>
                    ("SELECT * FROM User WHERE UserName = @Username And Password = @Password",
                    new { Username = username, Password = password });*/

            }
            catch
            {
                MessageBox.Show("This user is not valid!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public bool updateUser(User user)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
