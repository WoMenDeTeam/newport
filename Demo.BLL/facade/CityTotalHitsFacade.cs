using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;
using System.Data;

namespace Demo.BLL
{
    public static class CityTotalHitsFacade
    {
        private static readonly CITYTOTALHITSEntity.CITYTOTALHITSDAO dao = new CITYTOTALHITSEntity.CITYTOTALHITSDAO();

        public static DataTable GetLastHitDt() {
            return dao.FindNew();
        }

        public static DataTable GetLastHitDtByCategory()
        {
            return dao.FindNewByCategory();
        }
    }
}
