using System;

namespace AdmInflux.Client.Models
{
    public class Database : IEquatable<Database>
    {
        public string Server { get; set; }
        public string Name { get; set; }

        public bool Equals(Database other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Server, other.Server) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Database) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Server != null ? Server.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Database left, Database right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Database left, Database right)
        {
            return !Equals(left, right);
        }
    }
}