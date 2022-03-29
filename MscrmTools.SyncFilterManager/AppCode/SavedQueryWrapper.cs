using Microsoft.Xrm.Sdk;
using System;

namespace MscrmTools.SyncFilterManager.AppCode
{
    internal class SavedQueryWrapper
    {
        private Entity savedQuery;

        public SavedQueryWrapper(Entity savedQuery)
        {
            this.savedQuery = savedQuery;
        }

        public Entity SavedQuery => savedQuery;

        public override string ToString()
        {
            return savedQuery.GetAttributeValue<string>("name");
        }
    }
}