using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Windows;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.Parsing;
using EasyFarm.States;
using EasyFarm.ViewModels;
using MemoryAPI;
using MemoryAPI.Navigation;
using Pathfinder;
using Pathfinder.Map;
using Pathfinder.People;
using Pathfinder.Persistence;
using Zone = Pathfinder.Map.Zone;

namespace EasyFarm.Infrastructure
{
    public class NpcPersister
    {
        private IMemoryAPI _fface;
        private IGameContext _context;

        public NpcPersister(IMemoryAPI fface)
        {
            _fface = fface;
            _context = new GameContext(fface);
        }

        public void RunComponent()
        {
            string mapName = _context.Player.Zone.ToString();

            if (mapName == "Unknown")
                return;
            
            LogViewModel.Write("Starting To record NPCs in zone:" + mapName);
            var peopleOverseer = new PeopleOverseer(mapName);
            while (mapName == _context.Player.Zone.ToString())
            {
                AddNpcsToGrid(peopleOverseer);
            }
            LogViewModel.Write("Changing Zone!");
        }

        // private static FilePersister WatchAndPersistNpcs(string fileName)
        // {
        //     // Setup the PersonActor to save to file
        //     var npcPersister = new Pathfinder.Persistence.FilePersister();
        //     npcPersister.FilePath = GetAndCreateDirectorFromRoot("NPCs");
        //     npcPersister.FileName = fileName;
        //
        //     zone.LoadNpcsOrCreateNew(npcPersister);
        //
        //     var personActor = new PersonActor();
        //     personActor.Persister = npcPersister;
        //
        //     //Watch the NPC collection of the Zone
        //     var peopleWatcher = new CollectionWatcher<Person>(zone.Npcs, personActor);
        //     return npcPersister;
        // }

        

        private void AddNpcsToGrid(PeopleOverseer peopleOverseer)
        {
            if (_context.Memory.UnitService.NpcUnits.Count == 0)
            {
                return;
            }

            var units = new List<IUnit>(); //_context.Memory.UnitService.NpcUnits.ToList();
            units.AddRange(_context.Memory.UnitService.NpcUnits);
            foreach (var unit in units)
            {
                // Skip treasure caskets since they are randomly generated and will clog state.
                if (unit.Name == "Treasure Casket")
                    continue;
                
                Vector3 pos = RoundPositionToVector3(unit.Position);
                var npc = new Person(unit.Id, unit.Name, pos);
                peopleOverseer.PeopleManager.AddPerson(npc);
            }
        }

        private ObservableCollection<MemoryAPI.Navigation.Position> ConvertVectorArrayToObservableCollectionPosition(
            IList<Vector3> path)
        {
            var waypoints = new ObservableCollection<MemoryAPI.Navigation.Position>();
            for (int i = 0; i < path.Count; i++)
            {
                var pos = new MemoryAPI.Navigation.Position();
                pos.X = path[i].X;
                pos.Y = path[i].Y;
                pos.Z = path[i].Z;

                waypoints.Add(pos);
            }

            return waypoints;
        }

        public static Vector3 RoundPositionToVector3(Position position)
        {
            Vector3 gridPosition = new Vector3
            {
                X = GridMath.ConvertFromFloatToInt(position.X),
                Y = 0,
                Z = GridMath.ConvertFromFloatToInt(position.Z)
            };
            return gridPosition;
        }
    }
}