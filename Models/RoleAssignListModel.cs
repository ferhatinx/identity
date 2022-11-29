using System.Collections.Generic;

namespace Models
{
    public class RoleAssignListModel
    {
        public int RoleId { get; set; }

        public string Name { get; set; }
        public bool Exits { get; set; }
    }
    public class RoleAssignSendModel
    {
        public List<RoleAssignListModel> Roles {get; set;}
        public int UserId { get; set; }
    }
}