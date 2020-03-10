using System;

namespace BogLibrary
{
    public class Bog

    {
        private string _titel;
        private string _forfatter;
        private int _sidetal;
        private string _isbn;

        public Bog(string titel, string forfatter, int sidetal, string isbn)
        {
            Titel = titel;
            Forfatter = forfatter;
            Sidetal = sidetal;
            Isbn = isbn;
        }

        public Bog() { }

        public string Titel
        {
            get => _titel;
            set => _titel = value;
        }

        public string Forfatter
        {
            get => _forfatter;
            set
            {
                if (value.Length >= 2) _forfatter = value;
                else
                {
                    string e = "Forfatternavnet skal være på mindst 2 bogstaver!";
                    throw new ArgumentOutOfRangeException(e);
                }
            }
        }

        public int Sidetal
        {
            get => _sidetal;
            set
            {
                if (value >= 4 && value <= 1000) _sidetal = value;
                else
                {
                    string e = "Bogen skal have mindst 4 og højst 1000 sider!";
                    throw new ArgumentOutOfRangeException(e);
                }
            }
        }

        public string Isbn
        {
            get => _isbn;
            set
            {
                if (value.Length == 13) _isbn = value;
                else
                {
                    string e = "Isbn skal altid være på præcis 13 bogstaver!";
                    throw new ArgumentOutOfRangeException(e);
                }
            }
        }
    }
}
