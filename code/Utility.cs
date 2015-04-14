using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saga.Specification;
using Saga.Specification.Interfaces;

namespace Saga.BusinessLayer
{
    public class Utility : IUtility
    {
        public object GetRandom<T>(IList<T> pool)
        {
            if (pool == null)
            {
                throw new Exception("GetRandom:  no pool to pull from.");
            }
            if (pool.Count == 0)
            {
                throw new Exception("GetRandom:  pool is empty");
            }
            int? randomId = null;
            Random randomGenerator = new Random();
            randomId = randomGenerator.Next(0, pool.Count - 1);
            if (randomId == null)
            {
                throw new Exception("GetRandom: random id is still null");
            }
            return (T)pool[(int)randomId];
        }

        // Todo: Swap out for the cached, DB driven users.
        public int GetUserIdFromCode(string code)
        {
            int userId = -1;
            switch (code)
            {
                case Constants.User1.Code:
                    userId = Constants.User1.Id;
                    break;
                case Constants.User2.Code:
                    userId = Constants.User2.Id;
                    break;
                case Constants.User3.Code:
                    userId = Constants.User3.Id;
                    break;
                case Constants.User4.Code:
                    userId = Constants.User4.Id;
                    break;
                case Constants.User5.Code:
                    userId = Constants.User5.Id;
                    break;
                default:
                    userId = -999;
                    break;
            }
            return userId;
        }
        // Todo: Swap out for the cached, DB driven users.
        public string GetUserCodeFromId(int id)
        {
            string code = "notfound";
            switch (id)
            {
                case Constants.User1.Id:
                    code = Constants.User1.Code;
                    break;
                case Constants.User2.Id:
                    code = Constants.User2.Code;
                    break;
                case Constants.User3.Id:
                    code = Constants.User3.Code;
                    break;
                case Constants.User4.Id:
                    code = Constants.User4.Code;
                    break;
                case Constants.User5.Id:
                    code = Constants.User5.Code;
                    break;
                default:
                    code = "notfound";
                    break;
            }
            return code;
        }
    }
}
