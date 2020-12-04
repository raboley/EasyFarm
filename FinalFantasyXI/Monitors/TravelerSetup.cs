using System.Threading;
using Zone = Pathfinder.Map.Zone;

namespace FinalFantasyXI.Monitors
{
    public class TravelerSetup
    {
        private IMemoryAPI _fface;
        private IGameContext _context;


        public TravelerSetup(IMemoryAPI fface, IGameContext gameContext)
        {
            _fface = fface;
            _context = gameContext;
        }

        public void RunComponent()
        {
            if (_context.API.Player == null)
                return;
            string mapName = _context.Player.Zone.ToString();

            if (mapName == "Unknown")
                return;


            if (_context.API.Player.Position == null)
                return;

            var mover = new Mover(_context);

            var world = new World();

            if (_context.Zone == null)
                return;

            if (_context.Zone.Map == null)
                return;

            if (_context.Zone.Map.MapName != mapName)
                return;


            world.ZonePersister = ZoneMapPersister.NewZonePersister();
            world.LoadAllZonesToWorld(_context.Zone);

            if (_context.Npcs == null)
                return;

            world.PeopleManager = _context.NpcOverseer;
            world.Npcs.AddRange(_context.Npcs);

            if (_context.Mobs == null)
                return;
            world.Mobs.AddRange(_context.Mobs);

            LogViewModel.Write("Setting up Traveler for Zone: " + mapName);
            _context.Traveler = new Traveler(_context.Zone.Name, world, mover);
            LogViewModel.Write("Traveler Setup for Zone: " + mapName);

            while (true)
            {
                while (mapName == _context.Player.Zone.ToString())
                {
                    // Gotta be a better way to say only set this up again once zone changes.
                    // Maybe fire a zone change event and let everything subscribe to it?
                    _context.Traveler.IsFighting = _context.API.Player.Status.Equals(Status.Fighting);
                    _context.Traveler.MenuIsOpen = _context.API.Menu.IsMenuOpen;
                    Thread.Sleep(100);
                }

                _context.Traveler.Zoning = true;

                LogViewModel.Write("Changing Zone! (traveler)");

                while (_context.Player.Zone == MemoryAPI.Zone.Unknown)
                    Thread.Sleep(100);

                mapName = _context.Player.Zone.ToString();
                while (_context.Zone?.Map?.MapName != mapName)
                    Thread.Sleep(100);
                
                _context.Traveler.CurrentZone = _context.Zone;
                _context.Traveler.World.LoadAllZonesToWorld(_context.Zone);

                while (_context.Mobs == null)
                    Thread.Sleep(100);

                _context.Traveler.World.Mobs = new List<Person>();
                _context.Traveler.World.Mobs.AddRange(_context.Mobs);

                while (_context.Npcs == null)
                    Thread.Sleep(100);

                _context.Traveler.World.Npcs = new List<Person>();
                _context.Traveler.World.Npcs.AddRange(_context.Npcs);
                LogViewModel.Write("Traveler setup for Zone: " + mapName);
                _context.Traveler.Zoning = false;
            }
        }
    }
}