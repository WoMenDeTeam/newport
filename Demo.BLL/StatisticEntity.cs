using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.BLL
{
    public  class StatisticEntity
    {
        public String PicTitle { 
            set;
            get;
        }
        public String PicPath { 
            set;
            get;
        }

        public IList<ItemEntity> ItemList
        {
            set;
            get;
        }
    }
}
