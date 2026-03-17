using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string UserName { get; }
        string Email { get; }
        string FullName { get; }
        string AccessToken { get; }
    }
}
