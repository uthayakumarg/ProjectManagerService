//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectManagerDL.EntityDataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_USR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_USR()
        {
            this.T_PROJ = new HashSet<T_PROJ>();
            this.T_TASK = new HashSet<T_TASK>();
        }
    
        public int EMP_ID { get; set; }
        public string EMP_FRST_NM { get; set; }
        public string EMP_LST_NM { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PROJ> T_PROJ { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_TASK> T_TASK { get; set; }
    }
}