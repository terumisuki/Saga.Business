using System.Collections.Generic;
using Saga.Specification.Interfaces;
using Saga.Specification.Interfaces.Artists;
using Saga.Specification.Interfaces.Musical;
using Saga.Specification.Interfaces.Users;

namespace Saga.BusinessLayer
{
    public partial class ArtistBusiness : IArtistBusiness
    {
        private IArtistRepository _ArtistRepository;

        public ArtistBusiness(IArtistRepository repo)
        {
            _ArtistRepository = repo;
        }

        public IList<IArtist> GetAll()
        {
            return _ArtistRepository.GetAll();
        }

        public IList<IArtist> Search(string keyword)
        {
            IList<IArtist> artists = _ArtistRepository.GetByName(keyword);
            return artists;
        }

        public IList<IOpus> GetKnownOpuses(int artistId)
        {
            IList<IOpus> knownOpuses = _ArtistRepository.GetKnownOpuses(artistId);
            return knownOpuses;
        }

        public IList<IPiece> GetPieces(int opusId)
        {
            IList<IPiece> pieces = _ArtistRepository.GetPieces(opusId);
            return pieces;
        }

        public IList<IMovement> GetMovements(int pieceId)
        {
            IList<IMovement> movements = _ArtistRepository.GetMovements(pieceId);
            return movements;
        }

        public IList<IPart> GetParts(int movementId)
        {
            IList<IPart> parts = _ArtistRepository.GetParts(movementId);
            return parts;
        }
    }
}
