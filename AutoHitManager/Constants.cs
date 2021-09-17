using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoHitManager
{
    public class Constants
    {
        public static readonly string HTML = "<!DOCTYPE html><html><head><title>HitCounterData</title></head><body><script language='javascript'>let run_data = {0};let splits = {1};let total = {2};parent.DoUpdate(total, splits, run_data);</script></body></html>";
        internal static readonly List<string> DreamerZones = new()
        {
            "Dream_Nailcollection",
            "Dream_Guardian_Hegemol",
            "Dream_Guardian_Lurien",
            "Dream_Guardian_Monomon",
            "Dream_Backer_Shrine",
            "Dream_Abyss",
            "Dream_01_False_Knight",
            "Dream_02_Mage_Lord",
            "Dream_03_Infected_Knight",
            "Dream_04_White_Defender",
            "Dream_Room_Believer_Shrine"
        };
        internal static readonly List<string> ProhibitedZones = new()
        {
            "White_Palace"
        };
        private static string folder = "";

        public static string DirFolder
        {
            get
            {
                if (folder == "")
                {
                    var dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var final_dir = Path.Combine(dir, "AutoHit/");
                    if (!Directory.Exists(final_dir))
                    {
                        Directory.CreateDirectory(final_dir);
                    }
                    folder = final_dir;
                }
                return folder;
            }
        }
    }
}
