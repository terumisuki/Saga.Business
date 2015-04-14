using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saga.Specification.Interfaces.Audio;

namespace Saga.BusinessLayer
{
    public class AlbumBusiness : IAlbumBusiness
    {
        private readonly IAlbumRepository _AlbumRepository;
        private readonly ITrackRepository _TrackRepository;

        public AlbumBusiness(IAlbumRepository albumRepo, ITrackRepository trackRepo)
        {
            _AlbumRepository = albumRepo;
            _TrackRepository = trackRepo;
        }

        public IAlbum Get()
        {
            return _AlbumRepository.Get();
        }

        public IAlbum Get(int albumId)
        {
            return _AlbumRepository.Get(albumId);
        }

        public IAlbum Save(IAlbum album)
        {
            _AlbumRepository.Save(album);

            foreach (ITrack track in album.Tracks)
            {
                track.AlbumId = album.AlbumId;
                _TrackRepository.Save(track);
            }
            return album;
        }

        public void Track_Attach(ITrack track)
        {
            throw new NotImplementedException();
        }

        public IList<ITrack> GetTracks(IAlbum album)
        {
            IList<ITrack> tracks = _TrackRepository.GetFromAlbum(album);
            return tracks;
        }
    }
}
