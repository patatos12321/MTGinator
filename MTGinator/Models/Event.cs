using LiteDB;
using System;
using System.Collections.Generic;

namespace MTGinator.Models
{
    [CollectionName(DbCollectionName.Event)]
    public class Event : IDocument
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public DateTime Date { get; set; }
        public bool Official { get; set; }
        [BsonRef(DbCollectionName.Player)]
        public List<Player> ParticipatingPlayers { get; set; }
    }
}
