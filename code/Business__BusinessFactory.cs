using System;
using Saga.Specification.Interfaces;
using Saga.Specification.Interfaces.Audio;
using Saga.Specification.Interfaces.Genres;
using Saga.Specification.Interfaces.Musical;
using Saga.Specification.Interfaces.Users;


/*
 * TODO: What is this doing here?
 * Delete it if it's not used.
*/

namespace Saga.BusinessLayer
{
    class BusinessFactory : IBusinessFactory
    {
        public IUserBusiness GetUserBusiness()
        {
            throw new NotImplementedException();
        }

        public IPartBusiness GetPartBusiness()
        {
            throw new NotImplementedException();
        }

        public IGenreBusiness GetGenreBusiness()
        {
            throw new NotImplementedException();
        }

        public ITrackBusiness GetTrackBusiness()
        {
            throw new NotImplementedException();
        }
    }
}
