using LiteDB;

namespace MTGinator
{
    public class Player : IDocument
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }
}
