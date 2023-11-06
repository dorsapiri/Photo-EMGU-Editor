using Photo_EMGU_Editor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo_EMGU_Editor.ViewModel.DataAccess
{
    public interface IGalleryDataAccess
    {
        bool createGallery(GalleryModel gallery);
        GalleryModel findGalleryByUserId(int userId);
    }
}
