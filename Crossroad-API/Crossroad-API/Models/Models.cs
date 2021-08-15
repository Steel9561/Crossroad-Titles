using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossroad_API.Models
{
    public class Title
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TitleId { get; set; }

        [StringLength(100)]
        public string TitleName { get; set; }

        [StringLength(100)]
        public string TitleNameSortable { get; set; }

        public int? TitleTypeId { get; set; }

        public int? ReleaseYear { get; set; }

        public DateTime? ProcessedDateTimeUTC { get; set; }
        
        public virtual ICollection<Award> Awards { get; set; }

        public virtual ICollection<OtherName> OtherNames { get; set; }

        public virtual ICollection<StoryLine> StoryLines { get; set; }

        public virtual ICollection<TitleGenre> TitleGenres { get; set; }

        public virtual ICollection<TitleParticipant> TitleParticipants { get; set; }
    }

    public class OtherName
    {
        public int? TitleId { get; set; }

        [StringLength(100)]
        public string TitleNameLanguage { get; set; }

        [StringLength(100)]
        public string TitleNameType { get; set; }

        [StringLength(100)]
        public string TitleNameSortable { get; set; }

        [StringLength(100)]
        public string TitleName { get; set; }

        public int Id { get; set; }

        public virtual Title Title { get; set; }
    }

    public class TitleParticipant
    {
        public int Id { get; set; }

        public int TitleId { get; set; }

        public int ParticipantId { get; set; }

        public bool IsKey { get; set; }

        [StringLength(100)]
        public string RoleType { get; set; }

        public bool IsOnScreen { get; set; }

        public virtual Participant Participant { get; set; }

        public virtual Title Title { get; set; }
    }

    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
        
        public virtual ICollection<TitleGenre> TitleGenres { get; set; }
    }

    public class Participant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string ParticipantType { get; set; }
        
        public virtual ICollection<TitleParticipant> TitleParticipants { get; set; }
    }

    public class TitleGenre
    {
        public int Id { get; set; }

        public int TitleId { get; set; }

        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual Title Title { get; set; }
    }

    public class StoryLine
    {
        public int TitleId { get; set; }

        [StringLength(100)]
        public string Type { get; set; }

        [StringLength(100)]
        public string Language { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        public int Id { get; set; }

        public virtual Title Title { get; set; }
    }

    public class Award
    {
        public int TitleId { get; set; }

        public bool? AwardWon { get; set; }

        public int? AwardYear { get; set; }

        [Column("Award")]
        [StringLength(100)]
        public string Award1 { get; set; }

        [StringLength(100)]
        public string AwardCompany { get; set; }

        public int Id { get; set; }

        public virtual Title Title { get; set; }
    }


}
