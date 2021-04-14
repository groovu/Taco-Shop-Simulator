using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSSWpf
{
    public static class Utils
    {
        //turn a list into a dict.  used for making buy menu
        public static Dictionary<string, int> makeDict(List<String> list)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (string s in list)
            {
                dict.Add(s, 0);
            }
            return dict;
        }
        //borked, dont use this.
        public static List<string> makeList(IQueryable q)
        {
            List<string> ret = new List<string>();
            return ret;
        }
        static TacoDBEntity db = new TacoDBEntity();

    }
}
