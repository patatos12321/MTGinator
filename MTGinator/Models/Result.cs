using LiteDB;

namespace MTGinator.Models
{
    [CollectionName(DbCollectionName.Result)]
    public class Result : IDocument
    {
        [BsonId]
        public int Id { get; set; }
        public int NbWin { get; set; }
        public int Place { get; set; }
        public int Score { get; set; }
        [BsonRef(DbCollectionName.Player)]
        public Player Player { get; set; }
        [BsonRef(DbCollectionName.Event)]
        public Event Event { get; set; }
    }
}
