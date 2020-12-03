using System.Collections.Generic;

namespace EasyFarm.Soul
{
    public interface ICalling
    {
        bool CanDo();
        void Do();
        List<IObjective> Objectives { get; set; }
    }
}