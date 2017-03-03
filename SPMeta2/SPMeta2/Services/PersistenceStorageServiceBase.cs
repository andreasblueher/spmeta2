﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Services
{
    public abstract class PersistenceStorageServiceBase
    {
        #region methods

        public abstract byte[] LoadObject(string objectId);
        public abstract void SaveObject(string objectId, byte[] data);

        #endregion
    }
}
