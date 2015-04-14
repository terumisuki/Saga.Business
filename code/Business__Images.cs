using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saga.Specification.Interfaces;
using Saga.Specification.Interfaces.Errors;
using Saga.Specification.Interfaces.Images;

namespace Saga.BusinessLayer
{
    public class ImageBusiness : IImageBusiness
    {
        private readonly IImageRepository _ImageRepository;
        private readonly IErrorRepository _ErrorRepository;

        public ImageBusiness(IImageRepository repo, IErrorRepository errorRepo)
        {
            _ImageRepository = repo;
            _ErrorRepository = errorRepo;
        }

        public IImage Image(IImage image)
        {
            image = _ImageRepository.Get(image);

            // if the image isn't registered in the library, register it.
            if (image.MediaId < 1)
            {
                image = _ImageRepository.Save(image);
                image.Tags = new List<ITag>();
            }
            return image;
        }

        public void ImageSave(IImage image)
        {
            _ImageRepository.Save(image);
        }

        public void Tag(IImage image, ITag tag)
        {
            // add to the database
            _ImageRepository.TagRemove(image, tag);
            _ImageRepository.TagAdd(image, tag);
            
            // add to the object
            foreach (ITag t in image.Tags)
            {
                if (t.TagId == tag.TagId)
                {
                    image.Tags.Remove(t);
                    break;
                }
            }
            image.Tags.Add(tag);
        }

        public void UnTag(IImage image, ITag tag)
        {
            // remove from the database
            _ImageRepository.TagRemove(image, tag);
            
            // remove from the object
            foreach (ITag t in image.Tags)
            {
                if (t.TagId == tag.TagId)
                {
                    image.Tags.Remove(t);
                    break;
                }
            }
        }

        public IImage Get(IImage image)
        {
            return Image(image);
        }

        public IList<IImage> GetImagesForSlideShow(int userId, IList<ITag> includedTags, IList<ITag> excludedTags)
        {
            return _ImageRepository.GetImagesForSlideShow(userId, includedTags, excludedTags);
        }


        public IList<IImage> GetImagesForSlideShow(int userid)
        {
            return _ImageRepository.GetImagesForSlideShow(userid);
        }


        public void ClearNoRepeatList(int userid)
        {
            _ImageRepository.ClearNoRepeatList(userid);
        }


        public IImage Get(int id)
        {
            return _ImageRepository.Get(id);
        }


        public IList<IImage> GetRandomPhotoFromPhotoAlbum(int userid, string albumCode)
        {
            return _ImageRepository.GetRandomPhotoFromPhotoAlbum(userid, albumCode);
        }


        public void LogError(string error)
        {
            _ErrorRepository.LogError(error);
        }

        public void LogError(Exception ex)
        {
            _ErrorRepository.LogError(ex);
        }
    }
}
