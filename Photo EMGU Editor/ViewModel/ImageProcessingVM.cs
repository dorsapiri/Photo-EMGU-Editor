﻿using Emgu.CV.Structure;
using Emgu.CV;
using Photo_EMGU_Editor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Emgu.CV.Reg;
using Microsoft.Win32;
using Photo_EMGU_Editor.Helper;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.IO.Pipes;

namespace Photo_EMGU_Editor.ViewModel
{
    public class ImageProcessingVM : INotifyPropertyChanged
    {
        public User currentUser;
        //private byte[] imagedata;
        public ImageModel originalImageModel { get; set; } = new ImageModel();
        private Image<Bgr, byte> originalImage;
        private Image<Bgr, byte> adjustedImage;
        private Image<Gray, Single> adjustedImage2;
        public GalleryModel galleryModel { get; set; }
        public ObservableCollection<ImageModel> images { get; set; }
        public ICommand openDirectoryCommand { get; set; }
        public ICommand saveFileCommand { get; set; }
        public ICommand exiteWindow { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ImageProcessingVM(User user)
        {
            currentUser = user;
            openDirectoryCommand = new RelayCommand(OpenFile, canOpenFile);
            saveFileCommand = new RelayCommand(SaveImage, canSaveImage);
            exiteWindow = new RelayCommand(btnExite_Click, canExite);
            images = new ObservableCollection<ImageModel>();
            if (findGalleries())
            {
                fillListView(galleryModel.Id);
            }
            
        }
        private bool canExite(object sender)
        {
            return true;
        }

        private void btnExite_Click(object sender)
        {
            Application.Current.Shutdown();
        }
        public bool findGalleries()
        {
            bool existGalleryForCurrentUser = false;
            using (DatabaseConnection db = new DatabaseConnection())
            {
                galleryModel = db.IGalleryDataAccess.findGalleryByUserId(currentUser.Id);
            }
            if (galleryModel != null)
            {
                existGalleryForCurrentUser = true;
            }

            return existGalleryForCurrentUser;
        }
        private void fillListView(int galleryid)
        {
            using (DatabaseConnection db = new DatabaseConnection())
            {
                images = db.IImageAccess.readImage(galleryid);
            }
        }

        private ImageModel _selectedImage;
        public ImageModel selectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value;
                OnPropertyChanged(nameof(selectedImage));
                ShowCurrentImage();
                LoadImage();
            }
        }
        private void ShowCurrentImage()
        {
            imageView = selectedImage.FileLocation;
            originalImageModel = selectedImage;
        }
        private string _imageView;
        public string imageView
        {
            get { return _imageView; }
            set
            {
                _imageView = value;
                OnPropertyChanged(nameof(imageView));
            }
        }
        private bool canOpenFile(Object sender) { return _imageView != null; }
        private void OpenFile(object sender)
        {
            OpenFileDialog OpenDialog = new OpenFileDialog();
            OpenDialog.Filter = "Image Files|*.JPG; *.PNG; *.JFIF; *.jpg ; *.jpeg ; *.png ; *.bmp; *.jfif |All Files|*.*";

            bool? result = OpenDialog.ShowDialog();
            if (result == true)
            {
                imageView = OpenDialog.FileName;
                int findgalleryid;
                if (findGalleries())
                {
                    findgalleryid = galleryModel.Id;
                }
                else
                {
                    findgalleryid = makeGallery();
                }
                using (DatabaseConnection db = new DatabaseConnection())
                {
                    originalImageModel = new ImageModel
                    {
                        FileName = OpenDialog.SafeFileName,
                        FileLocation = OpenDialog.FileName,
                        ImageData = File.ReadAllBytes(imageView),
                        GalleryId = findgalleryid
                    };
                    db.IImageAccess.insertImage(originalImageModel);
                    images.Add(originalImageModel);
                }
            }
        }
        private bool canSaveImage(Object sender) { return imageView != null;}
        private void SaveImage(object sender) 
        {
            try
            {  
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Image Files(*.jpg;*.png)|*.jpg ; *.jpeg ; *.png ; *.bmp |All Files|*.*";
                bool? result = saveFileDialog.ShowDialog();
                using(Stream fileStream = saveFileDialog.OpenFile())
                {
                    if (result == true)
                    {
                        string fileName = saveFileDialog.FileName;
                        string extension = Path.GetExtension(fileName).ToLower();
                        Image img = adjustedImage.ToBitmap();
                        switch (extension)
                        {
                            case ".png":
                                img.Save(fileStream, ImageFormat.Png);
                                break;
                            case ".jpg":
                            case ".jpeg":
                                img.Save(fileStream, ImageFormat.Jpeg);
                                break;
                            case ".bmp":
                                img.Save(fileStream, ImageFormat.Bmp);
                                break;
                            default:
                                MessageBox.Show("Unsupported file format");
                                break;
                        }
                    }
                }
                
            }
            catch
            {

            }
        }
        private int makeGallery()
        {
            int Galleryid = 0;
            using (DatabaseConnection db = new DatabaseConnection())
            {
                galleryModel = new GalleryModel()
                {
                    UserId = currentUser.Id,
                };
                db.IGalleryDataAccess.createGallery(galleryModel);
                Galleryid = galleryModel.Id;
            }
            return Galleryid;
        }

        private double _Brightness = 1.0;
        public double Brightness
        {
            get { return _Brightness; }
            set
            {
                _Brightness = value;
                OnPropertyChanged(nameof(Brightness));
                LoadImage();
            }
        }

        private double _Contrust = 1.0;
        public double Contrust
        {
            get { return _Contrust; }
            set
            {
                _Contrust = value;
                OnPropertyChanged(nameof(_Contrust));
                LoadImage();
            }
        }

        private double _Sharpening = 1.0;
        public double Sharprning
        {
            get { return _Sharpening; }
            set
            {
                _Sharpening = value;
                OnPropertyChanged(nameof(Sharprning));
                LoadImage();
            }
        }

        private void LoadImage()
        {
            originalImage = new Image<Bgr, byte>(originalImageModel.FileLocation);
            adjustedImage = new Image<Bgr, byte>(originalImage.Size);
            //adjustedImage2 = new Image<Gray, Single>(originalImage.Size);
            try
            {
                ApplyAdjustment();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ApplyAdjustment()
        {
            
            float brightnessFactor = (float)Brightness;
            float contrustFactor = (float)Contrust;
            float sharpeningFactor = (float)Sharprning;

            //adjustedImage = originalImage.CopyBlank();
            adjustedImage = originalImage;

            adjustedImage = adjustedImage.Mul(brightnessFactor);
            adjustedImage = adjustedImage.Mul(contrustFactor);
            adjustedImage = adjustedImage.SmoothGaussian(5, 5, sharpeningFactor, sharpeningFactor);

            //adjustedImage2 = originalImage.Convert<Gray, Single>(); 
            
            Bitmap bmp = adjustedImage.ToBitmap();
            BitmapImage bitmapImage = new BitmapImage();
            

            using (var memory = new MemoryStream())
            {
                bmp.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                //imagedata = memory.ToArray();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            displayImage = bitmapImage;

            
        }
        
        private ImageSource _displayedImage;
        public ImageSource displayImage
        {
            get { return _displayedImage; }
            set
            {
                _displayedImage = value;
                OnPropertyChanged(nameof(displayImage));
            }
        }
    }
}
