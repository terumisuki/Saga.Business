using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saga.Specification.Interfaces;
using Saga.Specification.Interfaces.Artists;
using Saga.Specification.Interfaces.Musical;

namespace Saga.BusinessLayer
{
    public partial class MovementBusiness : IMovementBusiness
    {
        private IMovementDataManager _MovementDataManager;

        public MovementBusiness(IMovementDataManager datamanager)
        {
            _MovementDataManager = datamanager;
        }

        public void MovementsPartComposer_Add(IMovement movement, IPartBase part, IArtist composer)
        {
            _MovementDataManager.PartComposer_Add(movement, part, composer);
        }
    }
}
