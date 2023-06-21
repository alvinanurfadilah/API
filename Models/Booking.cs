using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_tr_bookings")]
public class Booking
{
    [Key]
    [Column("guid")]
    public Guid Guid { get; set; }

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [Column("status")] //untuk type data di erd putih itu enum
    public int Status { get; set; }

    [Column("remarks", TypeName = "nvarchar(255)")] //untuk constraint seharusnya null
    public string Remarks { get; set; }

    [Column("room_guid")]
    public Guid RoomGuid { get; set; }

    [Column("employee_guid")]
    public Guid EmployeeGuid { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    [Column("modified_date")]
    public DateTime ModifiedDate { get; set; }
}
