using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppIdentity.Models
{
    public class KeyRequirement : IAuthorizationRequirement
    {
        public KeyRequirement(string keyNumber)
        {
            this.KeyNumber = keyNumber;
        }
        public string KeyNumber { get; set; }

    }
}
