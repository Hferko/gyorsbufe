using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace FileIro
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] fogyasztas;
            string[] egysegAr;
            string[] fogyasztaSplit;
            string[] arakSplit;
            string vasarlo = "";
            int fizetett = 0, bevetel = 0;

            List<string> vevoLista = new List<string>();

            egysegAr = File.ReadAllLines("egysegar.csv");
            fogyasztas = File.ReadAllLines("fogyasztas.csv");

            for (int i = 0; i < fogyasztas.Length; i++)
            {
                fogyasztaSplit = fogyasztas[i].Split(';');
                if (fogyasztaSplit[0].Contains("Vásárló") == false)
                {
                    for (int j = 0; j < egysegAr.Length; j++)
                    {
                        arakSplit = egysegAr[j].Split(';');
                        if (fogyasztaSplit[1] == arakSplit[0])
                        {
                            fizetett = Convert.ToInt32(fogyasztaSplit[2]) * Convert.ToInt32(arakSplit[1]) + fizetett;
                            bevetel = Convert.ToInt32(fogyasztaSplit[2]) * Convert.ToInt32(arakSplit[1]) + bevetel;                            
                        }
                    }
                }
                else
                {
                    if (i > 0)
                    {
                        vevoLista.Add(vasarlo + ";  " + fizetett + "; " + " Forintot költött.");                        
                    }                    
                    vasarlo = fogyasztaSplit[0];
                    fizetett = 0;                    
                }
            }
            vevoLista.Add(vasarlo + ";  " + fizetett + "; " + " Forintot költött.");

            string penzForma = String.Format("{0:0,0.00}", bevetel);
            vevoLista.Add("A mai nap össz árbevétele; " + penzForma + "; Forint");

            // 6. feladat kiírás képernyőre:
            Console.WriteLine("A vásárlók költségei részletesen:\n");
            foreach (var vevo in vevoLista)
            {
                Console.WriteLine(vevo);   
                // Kiírás file-ba:
                File.AppendAllText("napibevetel.csv", $"{vevo}\n", Encoding.UTF8);
            }

            string path = Directory.GetCurrentDirectory();
            Console.WriteLine("\nA 'napibevetel.csv' file mentésre került ide: "+ path);

            Console.ReadKey();
        }
    }
}
