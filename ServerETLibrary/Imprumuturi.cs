

namespace ServerETLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class Imprumuturi
    {
        public int Id { get; set; }
        public Nullable<int> Id_client { get; set; }
        public Nullable<int> IBAN { get; set; }
        public string Id_angajat { get; set; }
        public Nullable<System.DateTime> Data_Imprumut { get; set; }
    }
}
