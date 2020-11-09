using System.IO;
using System.Numerics;
using Castle.Core.Internal;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.ViewModels;
using EasyFarm.Views;
using MemoryAPI;
using MemoryAPI.Navigation;
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
            
            _context.ZoneMapFactory.Persister = NewZoneMapPersister();
            _context.Zone.Map = _context.ZoneMapFactory.LoadGridOrCreateNew(mapName);
            _context.Zone.Name = mapName;

            while (mapName == _context.Player.Zone.ToString())
            {
                var node = _context.Zone.Map.GetNodeFromWorldPoint(new Vector3(_context.API.Player.PosX, _context.API.Player.PosY, _context.API.Player.PosZ));
                if (_context.Zone.Map.UnknownNodes.Contains(node))
                {
                    _context.Zone.Map.AddKnownNode(node.WorldPosition);
                    LogViewModel.Write("in map: " + _context.Zone.Name + " Adding position: " + node.WorldPosition);
                }   
            }
            
            var lastPositionBeforeZone = RoundPositionToVector3(_context.API.Player.Position);
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
                

                _context.Zone.AddBoundary(mapName,lastPositionBeforeZone,_context.Player.Zone.ToString(), RoundPositionToVector3(_context.API.Player.Position));
            }

            _context.ZoneMapFactory.Persister.Save(_context.Zone.Map);
            LogViewModel.Write("Saved map: " + mapName);
        }
        private bool IsZoning(IGameContext context) => context.Player.Str == 0;

        private Vector3 RoundPositionToVector3(Position playerPosition)
        {
            var x = GridMath.ConvertFromFloatToInt(playerPosition.X);
            var z = GridMath.ConvertFromFloatToInt(playerPosition.Z);
            
           return new Vector3(x, 0, z); 
           // return new Vector3(playerPosition.X, playerPosition.Y, playerPosition.Z); 
        }


        public static FilePersister NewZoneMapPersister()
        {
            var persister = new FilePersister();
            var mapsDirectory = GetMapsDirectory();
            persister.DefaultExtension = "json";
            persister.FilePath = mapsDirectory;
            return persister;
        }

        public static string GetMapsDirectory()
        {
            var repoRoot = GetRepoRoot();

            repoRoot = Path.Combine(repoRoot, "Maps");
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