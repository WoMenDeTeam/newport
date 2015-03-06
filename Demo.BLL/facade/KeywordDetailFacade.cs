using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;

namespace Demo.BLL
{
    public class KeywordDetailFacade
    {
        private static readonly KEYWORDDETAILEntity.KEYWORDDETAILDAO dao = new KEYWORDDETAILEntity.KEYWORDDETAILDAO();

        private static void add(KEYWORDDETAILEntity entity) {
            dao.Add(entity);
        }

        public static void add(string accid, string keyword,string ipaddress) {
            KEYWORDDETAILEntity entity = new KEYWORDDETAILEntity();
            entity.ACCID = accid;
            entity.KEYWORD = keyword;
            entity.IPADDRESS = ipaddress;
            entity.SEARCHTIME = DateTime.Now;
            entity.SEARCHTIMESTR = DateTime.Now.ToString("yyyy-MM-dd");
            add(entity);
        }
    }
}
