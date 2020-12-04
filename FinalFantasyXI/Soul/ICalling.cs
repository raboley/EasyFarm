using System.Collections.Generic;

namespace FinalFantasyXI.Soul
{
    public interface ICalling
    {
        bool CanDo();
        void Do();
        List<IObjective> Objectives { get; set; }
    }
}