using EasyFarm.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes
{
    class Navigator
    {
        private IGameContext _context;

        public Navigator(IGameContext context)
        {
            _context = context;
        }

        private void AvoidObstacles()
        {
            if (IsStuck())
            {
                //RecordTravelBlock();
                //if (IsEngaged()) Disengage();
                //WiggleCharacter(attempts: 3);
            }
        }

        public bool IsStuck()
        {
            var firstX = _context.Memory.EliteApi.Player.PosX;
            var firstZ = _context.Memory.EliteApi.Player.PosZ;
            TimeWaiter.Pause(2000);
            var dchange = Math.Pow(firstX - _context.Memory.EliteApi.Player.PosX, 2) + Math.Pow(firstZ - _context.Memory.EliteApi.Player.PosZ, 2);
            return Math.Abs(dchange) < 1;
        }
    }
}
