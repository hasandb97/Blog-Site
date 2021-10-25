//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyBlog.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_Posts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBL_Posts()
        {
            this.TBL_Comments = new HashSet<TBL_Comments>();
        }
    
        public int Post_ID { get; set; }
        public int Admin_ID { get; set; }
        public string Post_Title { get; set; }
        public string Post_Writer { get; set; }
        public int Category_ID { get; set; }
        public string Post_Summery { get; set; }
        public string Post_Image { get; set; }
        public string Post_Text { get; set; }
        public Nullable<System.DateTime> Post_Date { get; set; }
        public Nullable<int> Post_Likes { get; set; }
        public Nullable<int> Post_Dislike { get; set; }
        public Nullable<int> Post_CommentStatus { get; set; }
        public Nullable<int> Post_SeenCount { get; set; }
        public Nullable<int> Comments_Count { get; set; }
        public Nullable<int> Post_Status { get; set; }
        public string Post_Thumbnail { get; set; }
    
        public virtual TBL_Categories TBL_Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_Comments> TBL_Comments { get; set; }
        public virtual TBL_Admin TBL_Admin { get; set; }
    }
}
