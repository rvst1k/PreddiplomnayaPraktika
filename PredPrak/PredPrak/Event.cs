//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PredPrak
{
    using System;
    using System.Collections.Generic;
    
    public partial class Event
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Event()
        {
            this.Activity = new HashSet<Activity>();
        }
    
        public int id { get; set; }
        public string EventName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> Days { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> DirectionId { get; set; }
        public string Photo { get; set; }
    
        public virtual Cities Cities { get; set; }
        public virtual Directions Directions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Activity> Activity { get; set; }
    }
}
