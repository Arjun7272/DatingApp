using Api.Entiities;

namespace Api.interfaces
{
    public interface ITokenService
    {
         string CreateToken(AppUser user);
    }
}