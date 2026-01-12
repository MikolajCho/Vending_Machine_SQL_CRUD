using System;
using System.Collections.Generic;

namespace Automaty_z_napojami
{
    class Program
    {
        static DatabaseService ds = new DatabaseService(".\\SQLEXPRESS", "vending_machine_71423");
        static decimal saldo = 0;

        static void Main(string[] args)
        {
            Console.Title = "Aplikacja Automat z napojami - Projekt 71423";
            if (!ds.TestPolaczenia()) { Console.WriteLine("Blad polaczenia z SQL!"); return; }

            bool uruchomiony = true;
            while (uruchomiony)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("   AUTOMATY Z NAPOJAMI - MIKOLAJ CHODUR (71423)");
                Console.WriteLine("==============================================");
                Console.ResetColor();
                Console.WriteLine(" 1. PANEL KLIENTA (ZAKUP)");
                Console.WriteLine(" 2. PANEL ADMINISTRATORA (ZARZADZANIE)");
                Console.WriteLine(" 0. WYJSCIE");
                Console.WriteLine("----------------------------------------------");
                Console.Write(" Wybor: ");

                string menu = Console.ReadLine() ?? "";
                if (menu == "1") MenuUzytkownika();
                else if (menu == "2") MenuAdmina();
                else if (menu == "0") uruchomiony = false;
            }
        }

        static void MenuUzytkownika()
        {
            bool powrot = false;
            while (!powrot)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(">>> PANEL KLIENTA <<<");
                Console.WriteLine("AKTUALNE SRODKI: " + saldo.ToString("0.00") + " zl");
                Console.ResetColor();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine(string.Format("{0,-4} | {1,-20} | {2,-10} | {3,-10}", "ID", "NAZWA", "CENA", "OBJETOSC"));
                Console.WriteLine("---------------------------------------------------------");

                List<Napoj> napoje = ds.PobierzNapoje();
                foreach (Napoj n in napoje)
                {
                    Console.WriteLine(string.Format("{0,-4} | {1,-20} | {2,-10} | {3,-10}ml", n.Id, n.Nazwa, n.Cena + " zl", n.Pojemnosc));
                }

                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine(" [W] - Wrzuc monete | [Numer ID] - Kup napoj | [P] - Powrot");
                Console.Write(" Twoj wybor: ");
                string akcja = (Console.ReadLine() ?? "").ToUpper();

                if (akcja == "P") powrot = true;
                else if (akcja == "W")
                {
                    Console.Write(" Podaj kwote (np. 1, 2, 5): ");
                    string wplata = Console.ReadLine() ?? "0";
                    saldo += Convert.ToDecimal(wplata);
                }
                else
                {
                    try
                    {
                        int id = Convert.ToInt32(akcja);
                        Napoj wybrany = null;
                        foreach (Napoj n in napoje) { if (n.Id == id) wybrany = n; }

                        if (wybrany != null)
                        {
                            if (saldo >= wybrany.Cena)
                            {
                                saldo -= wybrany.Cena;
                                ds.ZapiszTransakcje(wybrany.Id, "Gotowka");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("\n[!] WYDAWANIE: " + wybrany.Nazwa);
                                if (saldo > 0) Console.WriteLine("[!] ZWROT RESZTY: " + saldo.ToString("0.00") + " zl");
                                Console.ResetColor();
                                saldo = 0; // Wydanie reszty i wyzerowanie automatu
                                Console.WriteLine("\nNacisnij dowolny klawisz..."); Console.ReadKey();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[!] BRAK SRODKOW! Brakuje: " + (wybrany.Cena - saldo) + " zl");
                                Console.ResetColor(); Console.ReadKey();
                            }
                        }
                    }
                    catch { /* Ignorujemy bledne wpisy */ }
                }
            }
        }

        static void MenuAdmina()
        {
            bool powrot = false;
            while (!powrot)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(">>> PANEL ADMINISTRACYJNY <<<");
                Console.ResetColor();
                Console.WriteLine(" 1. Dodaj nowy napoj");
                Console.WriteLine(" 2. Usun napoj z oferty");
                Console.WriteLine(" 0. Powrot do menu");
                Console.Write(" Opcja: ");

                string opcja = Console.ReadLine() ?? "";
                if (opcja == "1")
                {
                    Console.Write(" Nazwa: "); string nazwa = Console.ReadLine() ?? "";
                    Console.Write(" ID Kategorii (1-10): "); int kat = Convert.ToInt32(Console.ReadLine() ?? "1");
                    Console.Write(" Cena: "); decimal cena = Convert.ToDecimal(Console.ReadLine() ?? "0");
                    Console.Write(" Pojemnosc (ml): "); int poj = Convert.ToInt32(Console.ReadLine() ?? "0");
                    ds.DodajNapoj(nazwa, kat, cena, poj);
                    Console.WriteLine(" Produkt zostal dodany."); Console.ReadKey();
                }
                else if (opcja == "2")
                {
                    Console.Write(" Podaj ID napoju do usuniecia: ");
                    int id = Convert.ToInt32(Console.ReadLine() ?? "0");
                    ds.UsunNapoj(id);
                    Console.WriteLine(" Produkt zostal usuniety."); Console.ReadKey();
                }
                else if (opcja == "0") powrot = true;
            }
        }
    }
}
