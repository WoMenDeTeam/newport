using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;

namespace Demo.BLL
{
    public class ClickDetailFacade
    {
        private static readonly CLICKDETAILEntity.CLICKDETAILDAO dao = new CLICKDETAILEntity.CLICKDETAILDAO();

        private static void add(CLICKDETAILEntity entity) {
            dao.Add(entity);
        }

        public static void add(string accid, string rawref, string clickref,string ipaddress) {
            CLICKDETAILEntity entity = new CLICKDETAILEntity();
            entity.ACCID = accid;
            entity.CLICKREFRENCE = clickref;
            entity.RAWREFRENCE = rawref;
            entity.IPADDRESS = ipaddress;
            entity.CLICKTIME = DateTime.Now;
            entity.CLICKTIMESTR = DateTime.Now.ToString("yyyy-MM-dd");
            add(entity);
        }
    }
}
