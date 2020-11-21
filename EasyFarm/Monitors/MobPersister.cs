using System.Collections.Generic;
using System.Numerics;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.ViewModels;
using MemoryAPI;
using Pathfinder.People;

namespace EasyFarm.Monitors
{
    public class MobPersister
    {
        private IMemoryAPI _fface;
        private IGameContext _context;


        public MobPersister(IMemoryAPI fface, GameContext gameContext)
        {
            _fface = fface;
            _context = gameContext;
        }

        public void RunComponent()
        {
            string mapName = _context.Player.Zone.ToString();

            if (mapName == "Unknown")
                return;
            
            LogViewModel.Write("Starting To record Mobs in zone:" + mapName);
            var peopleOverseer = new PeopleOverseer(mapName, "Mobs");
            _context.Mobs = peopleOverseer.PeopleManager.People;
            
            while (mapName == _context.Player.Zone.ToString())
            {
                AddMobsToGrid(peopleOverseer);
            }
            LogViewModel.Write("Changing Zone! (Mob)");
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

        

        private void AddMobsToGrid(PeopleOverseer peopleOverseer)
        {
            if (_context.Memory.UnitService.MobArray.Count == 0)
            {
                return;
            }

            var units = new List<IUnit>(); //_context.Memory.UnitService.NpcUnits.ToList();
            units.AddRange(_context.Memory.UnitService.MobArray);
            foreach (var unit in units)
            {
                Vector3 pos = ConvertPosition.RoundPositionToVector3(unit.Position);
                var npc = new Person(unit.Id, unit.Name, pos);
                peopleOverseer.PeopleManager.AddPerson(npc);
            }
        }




    }
}