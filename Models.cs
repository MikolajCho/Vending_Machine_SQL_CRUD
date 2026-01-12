using System;

namespace Automaty_z_napojami
{
    // Klasa bazowa
    public class Produkt
    {
        public int Id;
        public string Nazwa = "";
        public decimal Cena;
    }

    // Klasa pochodna
    public class Napoj : Produkt
    {
        public int Pojemnosc;
        public string Kategoria = "";
    }
}
