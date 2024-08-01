using SwiftPos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Dto
{
    public class SetRoleDTO
    {
        public string Username { get; set; }
        public UserRole Role { get; set; }
    }
}
