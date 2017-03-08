
namespace ServerETLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class Carti
    {
        public int IBAN { get; set; }
        public string Titlu { get; set; }
        public string Autor { get; set; }
        public Nullable<int> Numar { get; set; }
        public Nullable<int> InStoc { get; set; }
    }
}
