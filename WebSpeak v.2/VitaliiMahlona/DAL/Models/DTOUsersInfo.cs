using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
    public class DTOUsersInfo
    {
        public Users CurrentUser { get; set; }
        public bool IsSignedIn { get; set; }
    }
}
