using Photo_EMGU_Editor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo_EMGU_Editor.ViewModel.DataAccess
{
    public interface IUserDataAccess
    {
        bool insertUser(User user);
        bool updateUser(User user);
        bool deleteUser(User user);
        User selectUser(string username, string password);
        bool selectAll();
    }
}
