using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace FormPluszGyorsbufe
{
    public partial class Form1 : Form
    {
        string[] fogyasztas;
        string[] egysegAr; 
        string[] fogyasztaSplit;        
        string[] arakSplit;
        int kuncsaft = 0;
        int reggel = 0, delelott = 0, delutan = 0;
        int kave = 0; 
        int kaveIvo = 0, kvReggel = 0, kvElott = 0, kvUtan = 0;
        int fizetett = 0, bevetel = 0;
        int reggeliBevetel = 0, elottiBevetel = 0, utaniBevetel = 0;
        string napszak = "";        
        string vasarlo = "";
        int tetel = 0;

        List<string> vevoLista = new List<string>();
        List<string> csakVevo = new List<string>();
        List<int> csakFizet = new List<int>();
        List<int> vasarloTetelek = new List<int>();
        List<string> soherVevo = new List<string>();

        public Form1()
        {
            InitializeComponent();   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label11.Visible = false; 

            // 1.feladat
            egysegAr = File.ReadAllLines("egysegar.csv");
            fogyasztas = File.ReadAllLines("fogyasztas.csv");

            for (int i = 0; i < fogyasztas.Length; i++)
            {
                fogyasztaSplit = fogyasztas[i].Split(';');                
                //listBox1.Items.Add(fogyasztas[i]);

                // 2. és 3. feladat
                if (fogyasztaSplit[0].Contains("Vásárló"))
                {
                    kuncsaft++;
                }
                if (fogyasztaSplit[1].Contains("Reggel"))
                {
                    reggel++;
                }
                if (fogyasztaSplit[1].Contains("Délelőtt"))
                {
                    delelott++;
                }
                if (fogyasztaSplit[1].Contains("Délután"))
                {
                    delutan++;
                }
                // 4. feladat
                if (fogyasztaSplit[1] == "Kávé")
                {
                    kaveIvo ++;
                    kave = kave + Convert.ToInt32(fogyasztaSplit[2]);
                }
            }
            label1.Text = ($"A mai napon {kuncsaft} vendég látogatta meg üzletegységünket");
            listBox1.Items.Add("Reggel   érkezett:\t" + reggel+" fő");
            listBox1.Items.Add("Délelőtt érkezett:\t" + delelott+" fő");
            listBox1.Items.Add("Délután  érkezett:\t" + delutan+" fő");
            
            label2.Text = ($"A mai nap {kaveIvo} fö fogyasztott kávét, és összesen {kave} adag került kiadásra");           

            kave = 0;
            kaveIvo = 0;
           
            for (int i = 0; i < fogyasztas.Length; i++)
            {
                fogyasztaSplit = fogyasztas[i].Split(';');

                // 5. feladat
                if (fogyasztaSplit[1] == "Kávé")
                {
                    kaveIvo++;
                    kave = kave + Convert.ToInt32(fogyasztaSplit[2]);
                    switch (napszak)
                    {
                        case "Reggel":
                            kvReggel = kvReggel + kave;
                            break;
                        case "Délelőtt":
                            kvElott = kvElott + kave;
                            break;
                        case "Délután":
                            kvUtan = kvUtan + kave;
                            break;
                    }
                    kave = 0;
                }
                // 6. feladat
                if (fogyasztaSplit[0].Contains("Vásárló") == false)
                {
                    for (int j = 0; j < egysegAr.Length; j++)
                    {
                        arakSplit = egysegAr[j].Split(';');

                        if (fogyasztaSplit[1] == arakSplit[0])
                        {
                            fizetett = Convert.ToInt32(fogyasztaSplit[2]) * Convert.ToInt32(arakSplit[1]) + fizetett;
                            bevetel = Convert.ToInt32(fogyasztaSplit[2]) * Convert.ToInt32(arakSplit[1]) + bevetel;
                            tetel++;
                        }
                    }
                }
                else
                {
                    if (i > 0)
                    {
                        vevoLista.Add(vasarlo + ";  " + fizetett + "; " + " Forintot költött.");
                        csakVevo.Add(vasarlo);
                        csakFizet.Add(fizetett);
                        vasarloTetelek.Add(tetel);
                        switch (napszak)
                        {
                            case "Reggel":
                                reggeliBevetel = reggeliBevetel + fizetett;                                
                                break;
                            case "Délelőtt":
                                elottiBevetel = elottiBevetel + fizetett;                                
                                break;
                            case "Délután":
                                utaniBevetel = utaniBevetel + fizetett;                                
                                break;
                        }                       
                    }
                    napszak = fogyasztaSplit[1];
                    vasarlo = fogyasztaSplit[0];
                    fizetett = 0;
                    tetel = 0;
                }
            }
            utaniBevetel = utaniBevetel + fizetett;

            vevoLista.Add(vasarlo + ";  " + fizetett + "; " + " Forintot költött.");

            string penzForma = String.Format("{0:c}", bevetel);
            vevoLista.Add("A mai nap össz árbevétele:  " + penzForma);
            label7.Text = ("A mai napi bevétel: "+ penzForma);

            // 6. feladat kiírás képernyőre:
            label4.Text = ("A vásárlők költségei részletesen:");
            foreach (var vevo in vevoLista)
            {
                listBox3.Items.Add(vevo);                
            }

            // Ez az 5. feladat kiíratása a képernyőre
            listBox2.Items.Add("Reggel fogyott:\t   "+ kvReggel+" adag");
            listBox2.Items.Add("Délelőtt fogyott:\t " + kvElott + " adag");
            listBox2.Items.Add("Délután fogyott:\t " + kvUtan + " adag");
            
            
            if (kvReggel > kvElott && kvReggel > kvUtan)
            {
                label3.Text = "Így tehát reggel fogyott a legtöbb kávé";
            }

            if (kvElott > kvReggel && kvElott > kvUtan)
            {
                label3.Text = "Így tehát délelőtt fogyott a legtöbb kávé";
            }

            if (kvUtan > kvReggel && kvUtan > kvElott)
            {
                label3.Text = "Így tehát délután fogyott a legtöbb kávé";
            }

            // 7. feladat
            label5.Text = "Napszakra lebontva:";
            listBox4.Items.Add("A reggeli bevétel:\t "+ String.Format("{0:c}", reggeliBevetel));
            listBox4.Items.Add("A délelőtti bevétel:\t" + String.Format("{0:c}", elottiBevetel));
            listBox4.Items.Add("A délutáni bevétel:\t" + String.Format("{0:c}", utaniBevetel));

            if (reggeliBevetel > elottiBevetel && reggeliBevetel > utaniBevetel)
            {
                label6.Text = "Reggel költöttek a legtöbbet a vásárlók";
            }

            if (elottiBevetel > reggeliBevetel && elottiBevetel > utaniBevetel)
            {
                label6.Text = "Délelőtt költöttek a legtöbbet a vásárlók";
            }

            if (utaniBevetel > reggeliBevetel && utaniBevetel > elottiBevetel)
            {
                label6.Text = "Délután költöttek a legtöbbet a vásárlók";
            }

            //8. feladat
            int legtobbetKoltott = csakFizet[0];
            string gazdagVevo = csakVevo[0];

            for (int i = 1; i < csakFizet.Count; i++)
            {
                if (csakFizet[i] > legtobbetKoltott)
                {
                    legtobbetKoltott = csakFizet[i];
                    gazdagVevo = csakVevo[i];
                }
            }
            label8.Text =$"A kuncsaft, aki a legtöbb pénzt költötte: '{gazdagVevo}' volt, és az elköltött összeg: {Convert.ToString(legtobbetKoltott)}Ft";

            //9. feladat
            int legtobbetVett = vasarloTetelek[0];
            string mohoVevo = csakVevo[0];

            for (int i = 1; i < vasarloTetelek.Count; i++)
            {
                if (vasarloTetelek[i] > legtobbetVett)
                {
                    legtobbetVett = vasarloTetelek[i];
                    mohoVevo = csakVevo[i];
                }
            }
            label9.Text = $"A kuncsaft, aki a legtöbb tételt vásárolta: '{mohoVevo}' volt, és Ő {Convert.ToString(legtobbetVett)} féle terméket vásárolt";

            // 10. feladat
            for (int i = 1; i < vasarloTetelek.Count; i++)
            {
                if (vasarloTetelek[i] == 1)
                {
                    soherVevo.Add(csakVevo[i]);
                }
            }
            label10.Text = "A következő vevők csupán egy tételt vásároltak:";
            foreach (var soher in soherVevo)
            {
                listBox5.Items.Add(soher);
            }

            button1.Enabled = false;
        }
    }
}
