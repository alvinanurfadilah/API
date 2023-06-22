using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

[Table("tb_tr_account_roles")]
public class AccountRole : BaseEntity
{
    [Column("account_guid")]
    public Guid AccountGuid { get; set; }

    [Column("role_guid")]
    public Guid RoleGuid { get; set; }

    //Cardinality
    public Account? Accounts { get; set; }
    public Role? Roles { get; set; }
}
