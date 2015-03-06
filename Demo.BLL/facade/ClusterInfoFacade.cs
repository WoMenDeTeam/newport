using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;
using Demo.Util;

namespace Demo.BLL
{
    public class ClusterInfoFacade
    {
        private static readonly CLUSTERINFOEntity.CLUSTERINFODAO dao = new CLUSTERINFOEntity.CLUSTERINFODAO();

        public static IList<CLUSTERINFOEntity> FindByClusterID(int clusterid) {
            return dao.FindByClusterID(clusterid);
        }
    }
}
