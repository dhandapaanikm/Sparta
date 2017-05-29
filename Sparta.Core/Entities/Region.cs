using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparta.Core.Entities;

namespace Sparta.Core.Entities
{
   public class Region : EntityBase
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public String Name { get; set; }

        public int order { get; set; }

        public Boolean IsReportingGroup { get; set; }

        public DateTime Created { get; set; }

        public String CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public string ModifiedBy { get; set; }
    }
}
