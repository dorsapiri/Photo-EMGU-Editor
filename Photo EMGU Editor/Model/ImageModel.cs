using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo_EMGU_Editor.Model
{
    public class ImageModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey("GalleryModel")]
        public int GalleryId { get; set; }
        public string FileName { get; set; }
        public byte[] ImageData { get; set; }
        public string FileLocation { get; set; }
    }
}
