using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Category
/// </summary>
namespace SubjectSelection
{
    [Serializable]
    public class Category
    {

        public Category() { }

        public string CategotyId { get; set; }
        public string CategotyName { get; set; }
        public string CallType { get; set; }
    
    }
}