using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saga.Specification.Interfaces.Genres;
using Saga.Specification.Interfaces.Musical;
using Saga.Specification.Interfaces.Users;
using Saga.Specifications.Interfaces;

namespace Saga.BusinessLayer
{
    public partial class GenreBusiness : IGenreBusiness
    {
        private IGenreRepository _GenreRepository;

        public GenreBusiness(IGenreRepository genreRepo)
        {
            _GenreRepository = genreRepo;
        }

        public IList<IGenre> GetAll()
        {
            return _GenreRepository.GetAll();
        }

        public IList<IGenre> Search(string keyword)
        {   
            IList<IGenre> genres = _GenreRepository.GetByName(keyword);
            return genres;
        }
    }
}
