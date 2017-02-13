using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuckyDraw
{
    class StaticEntity
    {
        private static List<string> awardNameList = new List<string>();

        public static List<string> AwardNameList
        {
            get
            {
                return awardNameList;
            }

            set
            {
                awardNameList = value;
            }
        }

        private static List<string> dispalyNameList = new List<string>();

        public static List<string> DispalyNameList
        {
            get
            {
                return dispalyNameList;
            }

            set
            {
                dispalyNameList = value;
            }
        }

        public static int WinNum = 0;

        public static int TimeSpeed = 10;
    }
}
