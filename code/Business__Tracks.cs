using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saga.Specification.Interfaces;
using Saga.Specification.Interfaces.Artists;
using Saga.Specification.Interfaces.Audio;
using Saga.Specification.Interfaces.Genres;
using Saga.Specification.Interfaces.Musical;

namespace Saga.BusinessLayer
{
    public class TrackBusiness : ITrackBusiness
    {
        private readonly ITrackRepository _TrackRepository;

        public TrackBusiness(ITrackRepository repo)
        {
            _TrackRepository = repo;
        }

        public ITrack Get(string trackFilePath)
        {
            return _TrackRepository.GetByFilePath(trackFilePath);
        }

        public ITrack Get(int trackId)
        {
            return _TrackRepository.GetById(trackId);
        }

        public void Genre_Add(ITrack track, IGenre genre)
        {
            _TrackRepository.Genre_Add(track, genre);
        }

        public void Genre_Remove(ITrack track, IGenre genre)
        {
            _TrackRepository.Genre_Remove(track, genre);
        }


        public void PerformanceAdd(ITrack track, IArtist artist, IPartBase part)
        {
            _TrackRepository.PerformanceAdd(track, artist, part);
        }
    }
}
