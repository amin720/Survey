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
    
    public partial class TBL_RSQA
    {
        public int Survey_Id { get; set; }
        public int Section_Id { get; set; }
        public int Question_Id { get; set; }
        public int Answer_Id { get; set; }
        public int Respondent_Id { get; set; }
    
        public virtual TBL_Answers TBL_Answers { get; set; }
        public virtual TBL_Questions TBL_Questions { get; set; }
        public virtual TBL_Respondents TBL_Respondents { get; set; }
        public virtual TBL_Sections TBL_Sections { get; set; }
        public virtual TBL_Surveys TBL_Surveys { get; set; }
    }
}
