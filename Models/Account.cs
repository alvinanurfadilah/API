using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_m_accounts")]
public class Account
{
    [Key]
    [Column("employee_guid")] //untuk pk gunakan employee_guid atau hanya guid biasa
    public Guid EmployeeGuid { get; set; }

    [Column("password", TypeName = "nvarchar(255)")]
    public string Password { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("otp")] //untuk constainr seharusnya null
    public int OTP { get; set; }

    [Column("is_used")] //untuk constraint seharusnya null
    public bool IsUsed { get; set; }

    [Column("expired_time")] //untuk constraint seharunya null
    public DateTime ExpiredTime { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    [Column("modified_date")]
    public DateTime ModifiedDate { get; set; }
}
