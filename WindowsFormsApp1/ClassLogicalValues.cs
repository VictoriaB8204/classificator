//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsFormsApp1
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClassLogicalValues
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> Feature { get; set; }
        public string TrueValue { get; set; }
        public string FalseValue { get; set; }
    
        public virtual Feature_description Feature_description { get; set; }
    }
}
