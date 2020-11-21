using System.IO;
using System.Numerics;
using Castle.Core.Internal;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.ViewModels;
using MemoryAPI;
using Pathfinder;
using Pathfinder.Persistence;

namespace EasyFarm.Monitors
{
    public class ZoneMapPersister
    {
        private IMemoryAPI _fface;
        private IGameContext _context;
        public ZoneMapPersister(IMemoryAPI fface, IGameContext context)
        {
            _fface = fface;
            _context = context;
        }

        public void RunComponent()
        {
            string mapName = _context.Player.Zone.ToString();
            // string mapName = _fface.Player.Zone.ToString();
            if (mapName == "Unknown" || mapName.IsNullOrEmpty())
                return;
            
            // Load up the Zone
            var zonePersister = NewZonePersister();
            zonePersister.MapName = mapName;


            if (zonePersister.Exists())
            {
                _context.Zone = zonePersister.Load<Pathfinder.Map.Zone>();
                
            }
            else
            {
                _context.Zone = new Pathfinder.Map.Zone(mapName);
            }
            
            _context.ZoneMapFactory.Persister = NewZoneMapPersister();
            LogViewModel.Write("Mapper thread loading up grid for: " + mapName);
            _context.Zone.Map = _context.ZoneMapFactory.LoadGridOrCreateNew(mapName);

            
            LogViewModel.Write("Mapper grid loaded!");
            while (mapName == _context.Player.Zone.ToString())
            {
                var pos = GridMath.RoundVector3(new Vector3(_context.API.Player.PosX, _context.API.Player.PosY,
                    _context.API.Player.PosZ));
                var node = _context.Zone.Map.GetNodeFromWorldPoint(pos);
                if (_context.Zone.Map.UnknownNodes.Contains(node))
                {
                    _context.Zone.Map.AddKnownNode(node.WorldPosition);
                    LogViewModel.Write("in map: " + _context.Zone.Name + " Adding position: " + node.WorldPosition);
                }   
            }
            
            var lastPositionBeforeZone = ConvertPosition.RoundPositionToVector3(_context.API.Player.Position);
            if (lastPositionBeforeZone != Vector3.Zero)
            {
                while (_context.API.Player.Zone == Zone.Unknown)
                {
                    TimeWaiter.Pause(100);
                }

                while (IsZoning(_context))
                {
                    TimeWaiter.Pause(100);
                }
                

                _context.Zone.AddBoundary(mapName,lastPositionBeforeZone,_context.Player.Zone.ToString(), ConvertPosition.RoundPositionToVector3(_context.API.Player.Position));
            }

            zonePersister.Save(_context.Zone);
            _context.ZoneMapFactory.Persister.Save(_context.Zone.Map);
            LogViewModel.Write("Saved map: " + mapName);
        }
        private bool IsZoning(IGameContext context) => context.Player.Str == 0;




        public static FilePersister NewZoneMapPersister()
        {
            var persister = new FilePersister();
            var mapsDirectory = GetDataDirectoryFor("ZoneMaps");
            persister.FilePath = mapsDirectory;
            return persister;
        }

        public static FilePersister NewZonePersister()
        {
            var persister = new FilePersister();
            var mapsDirectory = GetDataDirectoryFor("Zones");
            persister.FilePath = mapsDirectory;
            return persister;
        }
        
        public static string GetDataDirectoryFor(string dir)
        {
            var repoRoot = GetRepoRoot();

            repoRoot = Path.Combine(repoRoot, "Data");
            Directory.CreateDirectory(repoRoot);
            
            repoRoot = Path.Combine(repoRoot, dir);
            Directory.CreateDirectory(repoRoot);
            return repoRoot;
        }

        public static string GetAndCreateDirectorFromRoot(string directory)
        {
            var repoRoot = GetRepoRoot();

            repoRoot = Path.Combine(repoRoot, directory);
            Directory.CreateDirectory(repoRoot);
            return repoRoot;
        }

        public static string GetRepoRoot()
        {
            string repoRoot = Directory.GetCurrentDirectory();
            for (int i = 0; i < 3; i++)
            {
                repoRoot = Directory.GetParent(repoRoot).FullName;
            }

            return repoRoot;
        }
    }
}