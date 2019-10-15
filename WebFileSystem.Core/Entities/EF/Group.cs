using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFileSystem.Entities.EF {
    [Table("Group")]
    public class Group : Entity, IHasCreationTime {
        [Required, Index]
        public DateTime CreationTime { get; set; } = DateTime.Now;

    }
}
