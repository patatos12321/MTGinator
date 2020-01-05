using LiteDB;
using System;
using System.Collections.Generic;

namespace MTGinator.Models
{
    public class Pairing : IDocument
    {
        [BsonId]
        public int Id { get; set; }
        [BsonRef(DbCollectionName.Player)]
        public List<Player> Players { get; set; }
        [BsonRef(DbCollectionName.Player)]
        public Player WinningPlayer { get; set; }
    }
}
