﻿namespace Hawk.Domain.Entities
{
    public sealed class Tag
    {
        public Tag(string name, int total = 0)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public int Total { get; }

        public static implicit operator string(Tag tag)
        {
            return tag.Name;
        }

        public static implicit operator Tag(string name)
        {
            return new Tag(name);
        }
    }
}