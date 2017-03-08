using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerETLibrary
{
    class DataLayer:IDisposable
    {
        public DataLayer()
        {

        }
        public bool Authentificate (string user,string password)
        {
            bool return_value = false;
            using (LibraryDatabaseEntities context = new LibraryDatabaseEntities())
            {
                foreach (var index in context.Angajatis)
                {
                    string data;
                    data = index.Nume + " " + index.Prenume;
                    if (data==user&&password==index.Parola)
                    {
                        return_value = true;
                        break;
                    }
                }
            }
            return return_value;
        }
        public string GetUsers()
        {
            string data="";
            using (LibraryDatabaseEntities context = new LibraryDatabaseEntities())
            {
                foreach (var index in context.Clientis)
                {
                    data += index.Id.ToString() + " | " + index.Nume.ToString() + " " + index.Prenume.ToString() + " | " + index.CNP.ToString() + " / ";
                }
            }
            return data;
        }
        public string GetBooks()
        {
            string data = "";
            using (LibraryDatabaseEntities context = new LibraryDatabaseEntities())
            {
                foreach (var index in context.Cartis)
                {
                    data += index.IBAN.ToString() + " | " + index.Autor.ToString()+ " | " + index.Titlu.ToString() + " / ";
                }
            }
            return data;
        }
        public string GetUserIndex (string data)
        {
            string index = "0";
            using (LibraryDatabaseEntities context = new LibraryDatabaseEntities())
            {
                foreach (var i in context.Clientis)
                {
                    string cmp = i.Nume.ToString()+ i.Prenume.ToString();
                    if (cmp==data)
                    {
                        index = i.Id.ToString();
                        break;
                    }
                }
            }
            return index;
        }
        public string GetIndexByEmail (string email)
        {
            string data = "";
            using (LibraryDatabaseEntities context = new LibraryDatabaseEntities())
            {
                foreach (var x in context.Clientis)
                    if (email.ToString()==x.email.ToString())
                    {
                        data = x.Id.ToString();
                        break;
                    }
            }
            return data;
        }
        private void GetBook(string IBAN,out string st1,out string st2)
        {
            st1 = "";
            st2 = "";
            using (LibraryDatabaseEntities context = new LibraryDatabaseEntities())
            {
                foreach (var i in context.Cartis)
                {
                    if (i.IBAN.ToString()==IBAN)
                    {
                        st1 = i.Titlu.ToString();
                        st2 = i.Autor.ToString();
                        break;
                    }
                }
            }
        }
        public bool AuthentificateAspUser (string user,string CNP)
        {
            using (LibraryDatabaseEntities context = new LibraryDatabaseEntities())
            {
                foreach (var i in context.Clientis)
                {
                    if (i.CNP.ToString() == CNP && i.email.ToString() == user)
                        return true;
                }
            }
            return false;
        }
        public string GetLoans (string index)
        {
            string data = "";
            using (LibraryDatabaseEntities context = new LibraryDatabaseEntities())
            {
                foreach (var i in context.Imprumuturis)
                {
                    if (index==(i.Id_client.ToString()))
                    {
                        string st1, st2;
                        GetBook(i.IBAN.ToString(), out st1, out st2);
                        data += st1 + " | " + st2 + " | " + i.Data_Imprumut.ToString() + " / ";

                    }
                }
            }
            return data;
        }
        public string GetBook(string author)
        {
            string IBAN = "";
            using (LibraryDatabaseEntities context = new LibraryDatabaseEntities())
            {
                foreach (var i in context.Cartis)
                {
                    string x = i.Titlu.ToString() + " ";
                    if (x==author)
                    {
                        int var = int.Parse(i.InStoc.ToString());
                        var++;
                        i.InStoc = var;
                        //context.SaveChanges();
                        IBAN = i.IBAN.ToString();
                        break;
                    }

                }
                context.SaveChanges();
            }
            return IBAN;
        }
        public void RegisterUser(string[] date)
        {
            using (LibraryDatabaseEntities context = new LibraryDatabaseEntities())
            {
                int index = 1;
                foreach(var i in context.Clientis)
                {
                    index++;
                }
                //var row=context.Clientis.
                var row = new Clienti();
                row.Id = index;
                row.Nume = date[0].ToString();
                row.Prenume = date[1].ToString();
                row.email = date[2].ToString();
                row.CNP = date[3].ToString();
                row.penalizari = "0";
                context.Clientis.Add(row);
                context.SaveChanges();
            }
        }
        public void DeleteImprumut(string index,string IBAN)
        {
            using (LibraryDatabaseEntities context = new LibraryDatabaseEntities())
            {
                foreach (var i in context.Imprumuturis)
                {
                    if (i.Id_client.ToString()==index && i.IBAN.ToString()==IBAN)
                    {
                        context.Imprumuturis.Remove(i);
                    }

                }
                context.SaveChanges();
            }
        }
        public bool AddLoan(string index,string IBAN)
        {
            using (LibraryDatabaseEntities context = new LibraryDatabaseEntities())
            {
                int ind = 0;
                foreach (var i in context.Imprumuturis)
                {
                    ind = int.Parse(i.Id.ToString());
                }
                ind++;
                var x = new Imprumuturi();
                x.Id = ind;
                x.IBAN = int.Parse(IBAN);
                x.Id_client = int.Parse(index);
                ind = 1;
                x.Id_angajat =ind.ToString();
                DateTime localDate = DateTime.Now;
                x.Data_Imprumut = localDate;
                context.Imprumuturis.Add(x);
                context.SaveChanges();
                return true;
            }
                return false;
        }
        public void AddAngajat(string nume,string prenume,string pass)
        {
            using (LibraryDatabaseEntities context = new LibraryDatabaseEntities())
            {
                var x = new Angajati();
                int index = 0;
                foreach (var i in context.Angajatis)
                {
                    index = int.Parse(i.Id.ToString());
                }
                index++;
                x.Id = index;
                x.Nume = nume;
                x.Prenume = prenume;
                x.Parola = pass;
                context.Angajatis.Add(x);
                context.SaveChanges();
            }
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DataLayer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
