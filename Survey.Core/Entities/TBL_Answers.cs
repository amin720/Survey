//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Survey.Core.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_Answers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBL_Answers()
        {
            this.TBL_RSQA = new HashSet<TBL_RSQA>();
        }
    
        public int Id { get; set; }
        public string Text { get; set; }
        public int Question_Id { get; set; }
    
        public virtual TBL_Questions TBL_Questions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_RSQA> TBL_RSQA { get; set; }
    }
}