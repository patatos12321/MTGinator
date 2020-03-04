using LiteDB;

namespace MTGinator.Models
{
    [CollectionName(DbCollectionName.Player)]
    public class Player : IDocument
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public decimal WinLossRatio { get; set; }
    }
}
