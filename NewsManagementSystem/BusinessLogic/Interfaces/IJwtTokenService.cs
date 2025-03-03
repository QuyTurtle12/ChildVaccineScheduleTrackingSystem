using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IJwtTokenService
    {
        string GetName(string jwtToken);
        string GetEmail(string jwtToken);
        string GetRole(string jwtToken);
        string GetId(string jwtToken);
    }
}
