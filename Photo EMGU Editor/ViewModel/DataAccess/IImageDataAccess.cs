using Photo_EMGU_Editor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo_EMGU_Editor.ViewModel.DataAccess
{
    public interface IImageDataAccess
    {
        bool insertImage(ImageModel image);
        bool deleteImage(int id);
        ObservableCollection<ImageModel> readImage(int galleryid);
    }
}
