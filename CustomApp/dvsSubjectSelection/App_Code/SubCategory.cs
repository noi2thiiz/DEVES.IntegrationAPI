using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SubCategory
/// </summary>

namespace SubjectSelection
{
    [Serializable]
    public class SubCategory
    {
        public SubCategory()
        { }
        public string SubCategotyId { get; set; }
        public string SubCategotyName { get; set; }
        public string CategotyId { get; set; }
    }
}