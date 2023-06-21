using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

[Table("tb_m_account_roles")]
public class AccountRole
{
    [Key]
    [Column("guid")]
    public Guid Guid { get; set; }

    [Column("account_guid")]
    public Guid AccountGuid { get; set; }

    [Column("role_guid")]
    public Guid RoleGuid { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    [Column("modified_date")]
    public DateTime ModifiedDate { get; set; }
}
