using LiteDB;
using System.Collections.Generic;

namespace MTGinator.Models
{
    public class Round : IDocument
    {
        [BsonId]
        public int Id { get; set; }

        [BsonRef(DbCollectionName.Event)]
        public Event Event { get; set; }
        public int Number { get; set; }
        public List<Pairing> Pairings { get; set; }
    }
}
