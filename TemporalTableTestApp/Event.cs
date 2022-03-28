namespace TemporalTableTestApp
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Event")]
    public class Event
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string Client { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreatingUser { get; set; }

        public DateTime EndDate { get; set; }

        [Key]
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public string State { get; set; }

        public string Topic { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string? UpdatingUser { get; set; }

        public string Zip { get; set; }
    }
}