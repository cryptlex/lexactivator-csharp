using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptlex
{
    public class LicenseMeterAttribute
    {

        public string Name;

        public int AllowedUses;

        public int TotalUses;

        public LicenseMeterAttribute(string name, int allowedUses, int totalUses)
        {
            this.Name = name;
            this.AllowedUses = allowedUses;
            this.TotalUses = totalUses;
        }

    }
}

