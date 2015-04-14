using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saga.Specification.Interfaces.Images;
using Saga.Specification.Interfaces.PhotoAlbums;

namespace Saga.BusinessLayer
{
    public class PhotoAlbumsBusiness : IPhotoAlbumBusiness
    {
        #region properties / fields
        private IPhotoAlbumRepository _PhotoAlbumRepository;
        private IImageBusiness _ImageBusiness;
        #endregion


        #region constructors
        public PhotoAlbumsBusiness(IPhotoAlbumRepository repo, IImageBusiness imageBusiness)
        {
            _PhotoAlbumRepository = repo;
            _ImageBusiness = imageBusiness;
        }
        #endregion


        #region methods
        public int Save(IPhotoAlbum album)
        {
            int newAlbumId = _PhotoAlbumRepository.Save(album);
            return newAlbumId;
        }

        public List<IPhotoAlbum> GetAllAndFullyInflate()
        {
            List<IPhotoAlbum> albums = _PhotoAlbumRepository.GetAll();
            foreach (var album in albums)
            {
                album.Photos = GetPhotos(album.AlbumId);
            }
            return albums;
        }

        public List<IPhoto> GetPhotos(int albumId)
        {
            List<IPhoto> photos = _PhotoAlbumRepository.GetPhotos(albumId);
            return photos;
        }

        public void Delete(int albumId)
        {
            _PhotoAlbumRepository.Delete(albumId);
        }

        public void ShowPhotoOnAlbum(IPhotoAlbum _PhotoAlbum, IPhoto photo)
        {
            _PhotoAlbumRepository.ShowPhotoOnAlbum(_PhotoAlbum.AlbumId, photo.MediaId, photo.EnglishCaption, photo.JapaneseCaption);
        }

        public void RemovePhoto(IPhotoAlbum _PhotoAlbum, IImage photo)
        {
            _PhotoAlbumRepository.RemovePhoto(_PhotoAlbum.AlbumId, photo.MediaId);
        }


        public IPhoto GetNextPhoto(string albumCode, int userId)
        {
            IPhoto photo = _PhotoAlbumRepository.GetNextPhoto(albumCode, userId);
            return photo;
        }
        #endregion
    }
}
