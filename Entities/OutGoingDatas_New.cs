﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QcSupplier.Entities{
    public class OutGoingDatas_New
    {
        public int Id { set; get; }
        public string VendorAbbr { set; get; }
        public string VdDocdt { set; get; }
        public string DsSheetno { get; set; }
        public string VdDocno { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string Status { get; set; }
        public string Filename { get; set; }
        public string FlgJudge { get; set; }
        public int? ReviewId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual User CreatorId { get; set; }

    }
}
