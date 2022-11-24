using Microsoft.AspNetCore.Identity;

namespace CustomDescriber
{
    public class CustomErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new()
            {
                Code="PasswordTooShort",
                Description="Parola En az 5 karakter olmalıdır"
            };       
        }
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new()
            {
                Code="PasswordRequiresNonAlphanumeric",
                Description ="Parola En az 1 alfabetik karakter içermelidir"
            };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return new()
            {
                Code="DuplicateUserName",
                Description=$"Bu {userName} sistemde zaten kayıtlı"
            };
        }
    }
}