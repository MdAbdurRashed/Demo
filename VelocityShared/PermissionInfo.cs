using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VelocityShared
{
    public class PermissionInfo
    {
        public int UserId { get; set; }
        public int PageId { get; set; }
        public string PageTitle { get; set; } = string.Empty;
        public string PageUrl { get; set; } = string.Empty; 
    }
}
