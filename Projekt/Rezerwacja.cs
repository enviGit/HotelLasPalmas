//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Projekt
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rezerwacja
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rezerwacja()
        {
            this.Pobyt = new HashSet<Pobyt>();
        }
    
        public int ID { get; set; }
        public System.DateTime DataZameldowania { get; set; }
        public System.DateTime DataWymeldowania { get; set; }
        public int PokojID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pobyt> Pobyt { get; set; }
        public virtual Pokoj Pokoj { get; set; }
    }
}
