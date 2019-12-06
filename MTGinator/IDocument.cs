using LiteDB;

namespace MTGinator
{
    public interface IDocument
    {
        [BsonId]
        public int Id { get; set; }
    }
}
