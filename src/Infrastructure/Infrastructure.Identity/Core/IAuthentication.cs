using System;
using System.Threading.Tasks;
using Infrastructure.Identity.Model;

namespace Infrastructure.Identity.Core
{
    public interface IAuthentication
    {
        Task<string> GenerateToken(Guid aggregateId, string role);
        Task<UserModel> DecodeToken(string accessToken = null);
    }
}