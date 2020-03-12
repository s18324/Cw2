using System;
using System.Text;

namespace Cw2
{
    public class Student
    {
        private string imie, nazwisko, kierunek, tryb, data, mail, imieRodzica1, imieRodzica2;
        private string numerId;
        public Student(string imie, string nazwisko, string kierunek, string tryb, string numerId, string data, string mail, string imieRodzica1, string imieRodzica2)
        {
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.kierunek = kierunek;
            this.tryb = tryb;
            this.numerId = numerId;
            this.data = data;
            this.mail = mail;
            this.imieRodzica1 = imieRodzica1;
            this.imieRodzica2 = imieRodzica2;

            StringBuilder stringBuilder= new StringBuilder();
            for (int i = 0; i < nazwisko.Length; i++)
            {
                if (nazwisko[i]>=65)
                {
                    stringBuilder.Append(nazwisko[i]);
                }
            }
            
            this.nazwisko = stringBuilder.ToString();
        }

        public Student()
        {
            
        }

        public string getId()
        {
            return numerId;
        }

        public string getImie()
        {
            return imie;
        }
        public string getNazwisko()
        {
            return nazwisko;
        }
        public string getKierunek()
        {
            return kierunek;
        }
        public string getTryb()
        {
            return tryb;
        }
        public string getData()
        {
            return data;
        }
        public string getMail()
        {
            return mail;
        }
        public string getImieRodzica1()
        {
            return imieRodzica1;
        }
        public string getImieRodzica2()
        {
            return imieRodzica2;
        }
    }
}