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
    public class ImageDataAccess : IImageDataAccess
    {
        SQLiteConnection db;
        ObservableCollection<ImageModel> images { get; set; }
        public ImageDataAccess(SQLiteConnection connection)
        {
            db = connection;
            images = new ObservableCollection<ImageModel>();
        }

        public bool insertImage(ImageModel image)
        {
            try
            {
                db.Insert(image);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteImage(int id)
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

        public ObservableCollection<ImageModel> readImage(int galleryid)
        {
            try
            {
                db.CreateTable<ImageModel>();
                var AllImages = db.Table<ImageModel>().ToList();
                var findImagesByGalleryId = AllImages.FindAll(image => image.GalleryId == galleryid);
                foreach (var image in findImagesByGalleryId)
                {
                    images.Add(image);
                }
                return images;

            }
            catch
            {
                return null;
            }
        }
    }
}
