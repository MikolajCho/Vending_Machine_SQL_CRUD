using System;

namespace Automaty_z_napojami
{
    public class Produkt
    {
        public int Id;
        public string Nazwa = "";
        public decimal Cena;
    }

    public class Napoj : Produkt
    {
        public int Pojemnosc;
        public string Kategoria = "";
    }
}
