using System;

namespace MTGinator
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionNameAttribute : Attribute
    {
        public string Name { get; private set; }
        public CollectionNameAttribute(string name)
        {
            Name = name;
        }

    }
}
