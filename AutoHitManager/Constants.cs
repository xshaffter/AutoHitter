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
        internal static List<string> ProhibitedZones = new()
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
