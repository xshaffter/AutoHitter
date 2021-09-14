using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHitManager.Cat
{
    public class SplitConfig
    {

        public SplitConfig(string Name)
        {
            this.Name = Name;
            this.Id = Global.GlobalSaveData.NextId++;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
