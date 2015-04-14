using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Saga.Specification;
using Saga.Specification.Interfaces;
using Saga.Specification.Interfaces.Artists;
using Saga.Specification.Interfaces.Audio;
using Saga.Specification.Interfaces.Errors;
using Saga.Specification.Interfaces.Genres;
using Saga.Specification.Interfaces.Musical;
using Saga.Specification.Interfaces.Users;

namespace Saga.BusinessLayer
{
    public partial class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository _UserRepository;
        private readonly ITrackRepository _TrackRepository;
        private readonly IUtility _Utility;
        private readonly IErrorRepository _ErrorRepository;

        public UserBusiness(IUserRepository userRepo, ITrackRepository trackRepo, IUtility utility, IErrorRepository errorRepo)
        {
            _UserRepository = userRepo;
            _TrackRepository = trackRepo;
            _Utility = utility;
            _ErrorRepository = errorRepo;
        }


        #region gets
        public IUser Get(int userId)
        {
            try
            {
                return _UserRepository.Get(userId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public IUser Get(string username, string password)
        {
            try
            {
                return _UserRepository.Get(username, password);
            }
            catch (Exception e) 
            {
                throw e;
            }
        }

        public IList<IUser> GetAll()
        {
            IList<IUser> users = _UserRepository.GetAll();
            return users;
        }
        #endregion


        #region methods around media playlists
        public IEnumerable<IMedia> RecentlyPlayedMedia(IUser user)
        {
            return _UserRepository.RecentlyPlayedMedia_Get(user);
        }

        public void RecentlyPlayedMedia_Clear(IUser user)
        {
            _UserRepository.RecentlyPlayedMedia_Clear(user);
        }

        public IEnumerable<IMedia> RemoveRepeats(IUser user, IList<IMedia> mediaList)
        {
            var recentlyPlayedMedia = RecentlyPlayedMedia(user);
            var noRepeatList = mediaList.Except(recentlyPlayedMedia);
            return noRepeatList;
        }
        
        public IList<ITrack> ApplyWeighting(IList<ITrack> allPossibleAudio, ISettings audioPreferences)
        {
            // No weighting algorithm for this version yet.
            // Will be needed when implementing "rotation weight".
            // Heavy rotation, medium rotation, light rotation.
            return allPossibleAudio;
        }

        public ITrack RandomAudioTrack_Get(int userId)
        {
            try
            {
                ITrack track = _UserRepository.RandomAudioTrack_Get(userId);
                return track;
            }
            catch (Exception e)
            {
                _ErrorRepository.LogError(e);
                throw e;
            }
        }
        
        public ITrack RandomAudioTrack_Get(IUser user, ITrack track)
        {
            List<ITrack> usersAllAudio = AvailableTracksFromSettings_Get(user).ToList();
            track = (ITrack)_Utility.GetRandom(usersAllAudio);
            if (track == null)
            {
                track = HandleUnknownNextAudioToPlay(user);
            }
            return track;
        }

        public void Played(IUser user, IMedia media)
        {
            Played(user.UserId, media.MediaId);
        }
        public void Played(int userId, int mediaId)
        {
            _UserRepository.Played(userId, mediaId);
        }

        public double? AudioSkipped(ITrack audioToSkip, bool blessed, double secondsPlayedBeforeSkipping, IUser currentUser)
        {
            try
            {
                double currentDarwinScore = DarwinScore_Get(currentUser, audioToSkip);
                double newDarwinScore = currentDarwinScore + secondsPlayedBeforeSkipping;
                if (blessed)
                {
                    newDarwinScore = newDarwinScore + Math.Abs(Constants.DARWIN__SURVIVAL_SCORE);
                }
                else
                {
                    // Todo: Keep track of how many times in a row atrack is skipped.
                    //      Multiple the penalty by that many times.
                    //          (numberTimesSkipped * DARWIN_PENALTY_SCORE)
                    //
                    //      The next time the track is played all the way through, reset the numberOfTimesSkipped back to 0.
                    //          This will be done in AudioCompleted()
                    newDarwinScore = newDarwinScore - Math.Abs(Constants.DARWIN__PENALTY_SCORE);
                }
                _UserRepository.DarwinScore_Set(currentUser, audioToSkip, newDarwinScore);
                return newDarwinScore;
            }
            catch (Exception e)
            {
                //_ErrorRepository.LogError(e);
            }
            return null;
        }

        public void AudioCompleted(IUser user, int totalSeconds, ITrack audio)
        {
            // Todo: This Track has been played to completion.
            //          Clear the numberOfTimesSkipped for this Track.
            
            double currentDarwinScore = DarwinScore_Get(user, audio);
            double newDarwinScore = currentDarwinScore + Constants.DARWIN__SURVIVAL_SCORE + totalSeconds;
            _UserRepository.DarwinScore_Set(user, audio, newDarwinScore);
        }
        #endregion


        #region darwin related
        public double DarwinScore_Get(IUser user, ITrack audio)
        {
            double currentDarwinScore = _UserRepository.DarwinScore_Get(user, audio);
            return currentDarwinScore;
        }

        public double[] DarwinRange_Get(IUser user)
        {
            return _UserRepository.DarwinRange_Get(user);
        }
        #endregion


        #region user settings management
        public void NoRepeat_Save(ISettings settings)
        {
            _UserRepository.NoRepeat_Save(settings);
        }

        public void DarwinPercentage_Set(ISettings settings, int percentage)
        {
            _UserRepository.DarwinPercentage_Set(settings, percentage);
        }


        #region teach Saga what it doesn't know
        public void PromptForMissingGenres_Save(ISettings settings, bool teachMeGenres)
        {
            _UserRepository.Settings_PromptForMissingGenres_Save(settings, teachMeGenres);
        }

        public void PromptForMissingDarwin_Save(ISettings settings, bool teachMeDarwin)
        {
            _UserRepository.Settings_PromptForMissingDarwin_Save(settings, teachMeDarwin);
        }

        public void PromptForMissingParts_Save(ISettings settings, bool teachMeParts)
        {
            _UserRepository.Settings_PromptForMissingParts_Save(settings, teachMeParts);
        }
        #endregion
        
        
        #region artist settings
        public void IncludedArtistSetting_Add(ISettings settings, IArtist artistToAttach)
        {
            settings.AttachedArtists.Remove(artistToAttach);
            settings.AttachedArtists.Add(artistToAttach);
            _UserRepository.IncludedArtistSetting_Add(settings, artistToAttach);
        }

        public void IncludedArtistSetting_Remove(ISettings settings, IArtist artistToUnattach)
        {
            settings.AttachedArtists.Remove(artistToUnattach);
            _UserRepository.IncludedArtistSetting_Remove(settings, artistToUnattach);
        }

        public void ExcludedArtistSetting_Remove(ISettings settings, IArtist artistToRemove)
        {
            settings.UnattachedArtists.Remove(artistToRemove);
            _UserRepository.ExcludedArtistSetting_Remove(settings, artistToRemove);
        }

        public void ExcludedArtistSetting_Add(ISettings settings, IArtist artistToExclude)
        {
            settings.UnattachedArtists.Remove(artistToExclude);
            settings.UnattachedArtists.Add(artistToExclude);
            _UserRepository.ExcludedArtistSetting_Add(settings, artistToExclude);
        }
        #endregion


        #region genres settings
        public void IncludedGenresSetting_Add(ISettings settings, IGenre genreToAttach)
        {
            settings.AttachedGenres.Remove(genreToAttach);
            settings.AttachedGenres.Add(genreToAttach);
            _UserRepository.IncludedGenresSetting_Add(settings, genreToAttach);
        }

        public void IncludedGenresSetting_Remove(ISettings settings, IGenre genreToUnattach)
        {
            settings.AttachedGenres.Remove(genreToUnattach);
            _UserRepository.IncludedGenresSetting_Remove(settings, genreToUnattach);
        }

        public void ExcludedGenresSetting_Remove(ISettings settings, IGenre genreToRemove)
        {
            settings.UnattachedGenres.Remove(genreToRemove);
            _UserRepository.ExcludedGenresSetting_Remove(settings, genreToRemove);
        }

        public void ExcludedGenresSetting_Add(ISettings settings, IGenre genreToExclude)
        {
            settings.UnattachedGenres.Remove(genreToExclude);
            settings.UnattachedGenres.Add(genreToExclude);
            _UserRepository.ExcludedGenresSetting_Add(settings, genreToExclude);
        }
        #endregion


        #region parts settings
        public void IncludedPartSetting_Remove(ISettings settings, IPartBase partToUnattach)
        {
            settings.IncludedParts.Remove(partToUnattach);
            _UserRepository.IncludedPartsSetting_Remove(settings, partToUnattach);
        }

        public void IncludedPartSetting_Add(ISettings settings, IPartBase partToAttach)
        {
            settings.IncludedParts.Remove(partToAttach);
            settings.IncludedParts.Add(partToAttach);
            _UserRepository.IncludedPartsSetting_Add(settings, partToAttach);
        }

        public void ExcludedPartSetting_Remove(ISettings settings, IPartBase partToRemove)
        {
            settings.ExcludedParts.Remove(partToRemove);
            _UserRepository.ExcludedPartsSetting_Remove(settings, partToRemove);
        }

        public void ExcludedPartSetting_Add(ISettings settings, IPartBase partToExclude)
        {
            settings.ExcludedParts.Remove(partToExclude);
            settings.ExcludedParts.Add(partToExclude);
            _UserRepository.ExcludedPartsSetting_Add(settings, partToExclude);
        }
        #endregion


        #region slide show tag settings
        public void IncludedSlideShowTagSetting_Remove(ISettings settings, ITag tag)
        {
            settings.IncludedSlideShowTags.Remove(tag);
            _UserRepository.IncludedSlideShowTagSetting_Remove(settings, tag);
        }

        public void IncludedSlideShowTagSetting_Add(ISettings settings, ITag tag)
        {
            settings.IncludedSlideShowTags.Remove(tag);
            settings.IncludedSlideShowTags.Add(tag);
            _UserRepository.IncludedSlideShowTagSetting_Add(settings, tag);
        }

        public void ExcludedSlideShowTagSetting_Remove(ISettings settings, ITag tag)
        {
            settings.ExcludedSlideShowTags.Remove(tag);
            _UserRepository.ExcludedSlideShowTagSetting_Remove(settings, tag);
        }

        public void ExcludedSlideShowTagSetting_Add(ISettings settings, ITag tag)
        {
            settings.ExcludedSlideShowTags.Remove(tag);
            settings.ExcludedSlideShowTags.Add(tag);
            _UserRepository.ExcludedSlideShowTagSetting_Add(settings, tag);
        }
        #endregion
        #endregion


        public void LogError(Exception e)
        {
            _ErrorRepository.LogError(e);
        }

        
        // TODO: move to Track Business
        #region move to track business
        private bool contains(ITrack track, IGenre excludedGenre)
        {
            bool itDoes;
            var results = track.Genres.Where(g => g.GenreId == excludedGenre.GenreId);
            if (results.Count() > 0)
            {
                itDoes = true;
            }
            else
            {
                itDoes = false;
            }
            return itDoes;
        }

        private bool containsPart(ITrack track, IPartBase part)
        {
            bool itDoes;
            var results = track.Parts.Where(p => p.Part == part);
            if (results.Count() > 0)
            {
                itDoes = true;
            }
            else
            {
                itDoes = false;
            }
            return itDoes;
        }
        #endregion




        #region revist or remove

        #region revisit
        public ITrack HandleUnknownNextAudioToPlay(IUser user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITrack> AvailableTracksFromSettings_Get(IUser user)
        {
            _UserRepository.RandomAudioTrack_Get(user.UserId);
            return new List<ITrack>();
        }
        #endregion
        #endregion
    }
}
