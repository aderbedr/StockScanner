//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StockScanner
{
    using System;
    using System.Collections.Generic;
    
    public partial class KeywordInfluence
    {
        public string keyword { get; set; }
        public int industry_type { get; set; }
        public double influence { get; set; }
        public int influence_counter { get; set; }
    
        public virtual Industry Industry { get; set; }
    }
}
