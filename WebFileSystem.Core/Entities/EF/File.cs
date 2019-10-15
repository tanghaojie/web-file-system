using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFileSystem.Entities.EF {

    [Table("File")]
    public class File : Entity<long>, IHasCreationTime {
        [Required, Index]
        public string Filename { get; set; }
        [Required, Index]
        public string LocalFilePath { get; set; }
        [Index]
        public string Extension { get; set; }
        /// <summary>
        /// bytes
        /// </summary>
        [Required]
        public long Length { get; set; } = 0;
        public string ContentType { get; set; }

        [Required, Index]
        public string AccessSymbolic {
            get {
                return Access.AccessString;
            }
            set {
                Access = new Const.FileAccess.Access(value);
            }
        }
        [NotMapped]
        public virtual Const.FileAccess.Access Access { get; set; } = new Const.FileAccess.Access();

        public string Owner { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
        public int? GroupId { get; set; }


        public string Description { get; set; }


        [Required, Index]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        public DateTime? LastUpdationTime { get; set; }
        public DateTime? LastVisitTime { get; set; }
    }


}
