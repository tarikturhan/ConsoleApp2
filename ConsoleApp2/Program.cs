// Türkiye Süper Ligi 6. Hafta Hakem Seçimi Projesi

using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperLigHakemSecimi
{
    // Hakem sınıfı 
    public class Hakem
    {
        public string Ad { get; set; }
        public int TecrubeYili { get; set; }
        public string Uzmanlik { get; set; } // Örneğin Merkez, Yan Hakem
        public int BasariPuani { get; set; } // Hakemin başarı puanı
        public int MacSayisi { get; set; } = 0; // Hakemin yönettiği maç sayısı

        public override string ToString()
        {
            return $"{Ad} ({TecrubeYili} yıl, {Uzmanlik}, Başarı Puanı: {BasariPuani}, Maç Sayısı: {MacSayisi})";
        }
    }

    // Maç sınıfı
    public class Mac
    {
        public string Takim1 { get; set; }
        public string Takim2 { get; set; }
        public Hakem AtananHakem { get; set; }
        public string Tarih { get; set; } // Maç tarihi

        public override string ToString()
        {
            return $"{Takim1} vs {Takim2} - Hakem: {AtananHakem} - Tarih: {Tarih}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 2024-2025 sezonu, 6. hafta fikstürü
            List<(string Takim1, string Takim2, string Tarih)> fikstur = new List<(string, string, string)>
            {
                ("Hatayspor", "Bodrum FK", "20/09/2024"),
                
                ("Fenerbahçe", "Galatasaray", "21/09/2024")
                
               
                ,
                ("Beşiktaş", "Eyüpspor", "22/09/2024"),
                
                ("Gaziantep FK", "Trabzonspor", "23/09/2024")
            };

            // Güncel Süper Lig hakem listesi
            List<Hakem> hakemler = new List<Hakem>
            {
                new Hakem { Ad = "Halil Umut Meler", TecrubeYili = 10, Uzmanlik = "Merkez", BasariPuani = 90 },
                new Hakem { Ad = "Ali Şansalan", TecrubeYili = 8, Uzmanlik = "Merkez", BasariPuani = 85 },
                new Hakem { Ad = "Atilla Karaoğlan", TecrubeYili = 7, Uzmanlik = "Merkez", BasariPuani = 88 },
                new Hakem { Ad = "Zorbay Küçük", TecrubeYili = 5, Uzmanlik = "Merkez", BasariPuani = 80 }
            };

            // Maç oluşturma ve hakem atama
            List<Mac> maclar = new List<Mac>();
            var kalanHakemler = new List<Hakem>(hakemler);

            foreach (var (takim1, takim2, tarih) in fikstur)
            {
                if (kalanHakemler.Count == 0)
                {
                    kalanHakemler = new List<Hakem>(hakemler); // Tüm hakemler yeniden kullanılabilir hale getirilir
                }

                // Başarı puanına göre hakem seçimi
                var uygunHakemler = kalanHakemler.OrderByDescending(h => h.BasariPuani).ToList();
                var atananHakem = uygunHakemler.First(); // En yüksek başarı puanına sahip hakem atanır

                maclar.Add(new Mac
                {
                    Takim1 = takim1,
                    Takim2 = takim2,
                    AtananHakem = atananHakem,
                    Tarih = tarih
                });

                // Atanan hakemin maç sayısı artırılır ve geçici listeden çıkarılır
                atananHakem.MacSayisi++;
                kalanHakemler.Remove(atananHakem);
            }

            // Sonuçları ekrana yazdırma
            Console.WriteLine("Türkiye Süper Ligi 2024-2025 Sezonu 6. Hafta Maçları ve Hakemleri:\n");
            foreach (var mac in maclar)
            {
                Console.WriteLine(mac);
            }

            Console.WriteLine("\nHakem İstatistikleri:");
            foreach (var hakem in hakemler.OrderByDescending(h => h.BasariPuani))
            {
                Console.WriteLine(hakem);
            }

            Console.WriteLine("\n ");
            Console.ReadKey();
        }
    }
}


