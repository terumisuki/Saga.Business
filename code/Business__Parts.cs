using System.Collections.Generic;
using Saga.Specification.Interfaces;
using Saga.Specification.Interfaces.Musical;
using Saga.Specifications.Interfaces.Parts;

namespace Saga.BusinessLayer
{
    public partial class PartBusiness : IPartBusiness
    {
        private IPartRepository _PartRepository;

        public PartBusiness(IPartRepository partRepo)
        {
            _PartRepository = partRepo;
        }

        public IList<IPartBase> GetAll()
        {
            return _PartRepository.GetAll();
        }

        public IList<IPartBase> Search(string keyword)
        {
            return _PartRepository.GetByName(keyword);
        }

    }
}
