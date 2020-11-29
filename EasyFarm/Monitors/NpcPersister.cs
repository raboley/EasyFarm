using System.Collections.Generic;
using System.Numerics;
using System.Windows.Controls.Primitives;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.ViewModels;
using MemoryAPI;
using Pathfinder.People;

namespace EasyFarm.Monitors
{
    public class NpcPersister
    {
        private IMemoryAPI _fface;
        private IGameContext _context;


        public NpcPersister(IMemoryAPI fface, GameContext gameContext)
        {
            _fface = fface;
            _context = gameContext;
        }

        public void RunComponent()
        {
            string mapName = _context.Player.Zone.ToString();

            if (mapName == "Unknown")
                return;
            
            LogViewModel.Write("Starting To record NPCs in zone:" + mapName);
            var peopleOverseer = new PeopleOverseer(mapName);
            _context.NpcOverseer = peopleOverseer;
            _context.Npcs = peopleOverseer.PeopleManager.People;
            
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
                
                // Some weird things stored with this name... on points that shouldn't exist.
                if (unit.Name == "")
                    continue;
                
                Vector3 pos = ConvertPosition.RoundPositionToVector3(unit.Position);
                var npc = new Person(unit.Id, unit.Name, pos);
                peopleOverseer.PeopleManager.AddPerson(npc);
            }
        }
    }
}