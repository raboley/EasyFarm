using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using Castle.Core.Internal;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.ViewModels;
using MemoryAPI;
using MemoryAPI.Navigation;
using Pathfinder;
using Pathfinder.Persistence;
using Pathfinder.Travel;

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
            var zone = GetOrCreateNewZone(mapName, out var zonePersister);

            _context.Zone = zone;

            _context.ZoneMapFactory.DefaultGridSize = new Vector2(2001f, 2001f);
            _context.ZoneMapFactory.Persister = NewZoneMapPersister();
            LogViewModel.Write("Mapper thread loading up grid for: " + mapName);
            _context.Zone.Map = _context.ZoneMapFactory.LoadGridOrCreateNew(mapName);


            LogViewModel.Write("Mapper grid loaded!");
            while (mapName == _context.Player.Zone.ToString())
            {
                var pos = GridMath.RoundVector3(new Vector3(_context.API.Player.PosX, _context.API.Player.PosY,
                    _context.API.Player.PosZ));
                var node = _context.Zone.Map.GetNodeFromWorldPoint(pos);
                // if (_context.Zone.Map.UnknownNodes.Contains(node))
                // {
                    _context.Zone.Map.AddKnownNode(node.WorldPosition);
                    // Debug.Write("in map: " + _context.Zone.Name + " Adding position: " + node.WorldPosition);
                // }
            }

            var lastPositionBeforeZone = ConvertPosition.RoundPositionToVector3(_context.API.Player.Position);

            if (lastPositionBeforeZone == Vector3.Zero)
            {
                var positionHistoryList = _context.API.Player.PositionHistory.ToList();

                for (int i = 0; i < positionHistoryList.Count; i++)
                {
                    lastPositionBeforeZone =  ConvertPosition.RoundPositionToVector3(positionHistoryList[i]);
                    if (lastPositionBeforeZone != Vector3.Zero)
                        break;
                }
            }

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


                if (!_context.Traveler.IsDead)
                {
                    string newMapName = _context.API.Player.Zone.ToString();
                    Vector3 newMapPosition = ConvertPosition.RoundPositionToVector3(_context.API.Player.Position);

                    _context.Zone.AddBoundary(mapName, lastPositionBeforeZone, newMapName,
                        newMapPosition);

                    // We must have explored this point because we zoned and not due to death.
                    if (_context.Traveler.CurrentZone.NextPointToExplore != null)
                    {
                        _context.Traveler.CurrentZone.Explored.Add( (Vector3) _context.Traveler.CurrentZone.NextPointToExplore);
                        _context.Traveler.CurrentZone.NextPointToExplore = null;
                    }
                        

                    // Can't do new zone stuff because we end up in the middle of the zone.
                    // var newZone = GetOrCreateNewZone(newMapName, out var newZonePersister);
                    // newZone.AddBoundary(newMapName, newMapPosition, mapName, lastPositionBeforeZone);
                    // newZonePersister.Save(newZone);
                }
            }

            zonePersister.Save(_context.Zone);
            _context.ZoneMapFactory.Persister.Save(_context.Zone.Map);
            LogViewModel.Write("Saved map: " + mapName);
            _context.Traveler.IsDead = false;
        }

        private static Pathfinder.Map.Zone GetOrCreateNewZone(string mapName, out FilePersister zonePersister)
        {
            zonePersister = NewZonePersister();
            zonePersister.MapName = mapName;


            Pathfinder.Map.Zone zone;
            if (zonePersister.Exists())
            {
                zone = zonePersister.Load<Pathfinder.Map.Zone>();
            }
            else
            {
                zone = new Pathfinder.Map.Zone(mapName);
            }

            return zone;
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