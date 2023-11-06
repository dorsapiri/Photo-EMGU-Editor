using Photo_EMGU_Editor.ViewModel.DataAccess;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo_EMGU_Editor.Model
{
    public class DatabaseConnection : IDisposable
    {
        public static string databaseName = "picture_Editor.db";
        public static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string databasePath = System.IO.Path.Combine(folderPath, databaseName);
        SQLiteConnection connection = new SQLiteConnection(databasePath);
        public DatabaseConnection()
        {
            connection.CreateTable<User>();
            connection.CreateTable<ImageModel>();
            connection.CreateTable<GalleryModel>();
        }

        private IImageDataAccess _imageDataAccess;
        public IImageDataAccess IImageAccess
        {
            get
            {
                if (_imageDataAccess == null)
                {
                    _imageDataAccess = new ImageDataAccess(connection);
                }
                return _imageDataAccess;
            }
        }

        private IUserDataAccess _userDataAccess;

        public IUserDataAccess IUserDataAccess
        {
            get
            {
                if (_userDataAccess == null)
                {
                    _userDataAccess = new UserDataAccess(connection);
                }
                return _userDataAccess;
            }

        }

        private IGalleryDataAccess _galleryDataAccess;

        public IGalleryDataAccess IGalleryDataAccess
        {
            get
            {
                if (_galleryDataAccess == null)
                {
                    _galleryDataAccess = new GalleryDataAccess(connection);
                }
                return _galleryDataAccess;
            }
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
