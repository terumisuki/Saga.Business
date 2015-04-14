using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saga.Specification.Interfaces.Audio;
using Saga.Specification.Interfaces.Genres;
using Saga.Specification.Interfaces.Musical;

namespace Saga.BusinessLayer
{
    public partial class Business
    {
        //public partial class Audio
        //{
            #region tracks
            //public class Tracks
            //{
                //private ITrackRepository _TrackRepository;
                
                //public Tracks(ITrackRepository trackRepository)
                //{
                //    if (trackRepository == null) throw new ArgumentNullException();
                //    _TrackRepository = trackRepository;
                //}

                //public ITrack Tracks__Get(string trackFilePath)
                //{
                //    return _TrackRepository.GetByFilePath(trackFilePath);
                //}

                //public void Tracks__Genre_Add(ITrack track, IGenre genre)
                //{
                //    _TrackRepository.Genres__Add(track, genre);
                //}

                //public void Tracks__Genre_Remove(ITrack track, IGenre genre)
                //{
                //    _TrackRepository.Genres__Remove(track, genre);
                //}
            //}
            #endregion


            #region albums
            //public class Albums
            //{
                //private IAlbumRepository _AlbumRepository;

                //public Albums(IAlbumRepository albumRepository)
                //{
                //    _AlbumRepository = albumRepository;
                //}

                //public IAlbum Albums__Get(int albumId)
                //{
                //    return _AlbumRepository.Get(albumId);
                //}
            //}
            #endregion

        //}
    }
}
