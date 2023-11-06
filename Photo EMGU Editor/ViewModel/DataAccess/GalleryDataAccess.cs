using Photo_EMGU_Editor.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo_EMGU_Editor.ViewModel.DataAccess
{
    public class GalleryDataAccess : IGalleryDataAccess
    {
        SQLiteConnection db;
        private ObservableCollection<GalleryModel> galleries;
        public GalleryDataAccess(SQLiteConnection connection)
        {
            db = connection;
            galleries = new ObservableCollection<GalleryModel>();
            SelectAllGalleries();
        }

        private bool SelectAllGalleries()
        {
            try
            {
                db.CreateTable<GalleryModel>();
                var galleryTable = db.Table<GalleryModel>().ToList();
                foreach (var gallery in galleryTable)
                {
                    galleries.Add(gallery);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool createGallery(GalleryModel gallery)
        {
            try
            {
                db.Insert(gallery);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public GalleryModel findGalleryByUserId(int userId)
        {
            try
            {
                return galleries.First(gallery => gallery.UserId == userId);
            }
            catch
            {
                return null;
            }
        }
    }
}
