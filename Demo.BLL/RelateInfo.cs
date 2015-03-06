using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.BLL
{
    public static class RelateInfo
    {
        private static Dictionary<string, string> LeaderInfo = new Dictionary<string, string>();
        private static Dictionary<string, string> CategoryInfo = new Dictionary<string, string>();
        private static Dictionary<string, string> OrgInfo = new Dictionary<string, string>();
        public static Dictionary<string, string> GetInfoByType(int type)
        {
            switch (type) { 
                case 1:
                    if (LeaderInfo.Keys.Count == 0) {
                        InnitLeaderInfo();
                    }
                    return LeaderInfo;
                case 2:
                    return null;
                case 3:
                    if (OrgInfo.Keys.Count == 0)
                    {
                        InnitOrgInfo();
                    }
                    return OrgInfo;
                default:
                    return null;
            }
        }

        private static void InnitLeaderInfo() {            
            string[] leaderlist = new string[] { "胡锦涛", "吴邦国", "温家宝", "贾庆林", "李长春", "习近平", "李克强", "贺国强", "周永康 " };
            foreach (string leader in leaderlist) {
                if (!LeaderInfo.Keys.Contains(leader)) { 
                    string text = "\"" + leader +"\"";
                    LeaderInfo.Add(leader, text);
                }
            }
        }

        private static void InnitCategoryInfo() { 
            
        }

        private static void InnitOrgInfo()
        {
            OrgInfo.Add("中纪委", "\"中纪委\"");
            OrgInfo.Add("信访局", "\"信访局\"");  
        }
    }
}
