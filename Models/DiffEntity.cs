using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("diffentity")]
public class DiffEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Int32 id { get; set;} 

    [Column("codigo")]
    public Int32 codigo { get; set; }

    [Column("isfrom")]
    public Int32 isfrom { get; set; }

    [Column("jsonuploaded")]
    public String jsonUploaded { get; set; }

}