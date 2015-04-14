using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saga.Specification.Interfaces;
using Saga.Specification.Interfaces.Tags;
using Saga.Specifications.Interfaces;

namespace Saga.BusinessLayer
{
    public partial class TagBusiness : ITagBusiness
    {
        private ITagRepository _TagRepository;

        public TagBusiness(ITagRepository tagRepo)
        {
            _TagRepository = tagRepo;
        }

        public IList<ITag> GetAll()
        {
            return _TagRepository.GetAll();
        }


        public IEqualityComparer<ITag> GetComparer()
        {
            return _TagRepository.GetComparer();
        }


        public IList<ITag> Search(string keywords)
        {
            return _TagRepository.Search(keywords);
        }


        public IList<ITag> GetActiveTags()
        {
            return _TagRepository.GetActiveTags();
        }
    }
}
