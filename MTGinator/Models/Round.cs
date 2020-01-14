using LiteDB;
using System.Collections.Generic;

namespace MTGinator.Models
{
    public class Round : IDocument
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public List<Pairing> Pairings { get; set; }
    }
}
