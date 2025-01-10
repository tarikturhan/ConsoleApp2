namespace SuperLigHakemSecimi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        public string Sonuc { get; set; } // Maç sonucu
        public int Takim1Gol { get; set; } // Takım 1'in attığı gol sayısı
        public int Takim2Gol { get; set; } // Takım 2'nin attığı gol sayısı
        public int Takim1Sut { get; set; } // Takım 1'in şut sayısı
        public int Takim2Sut { get; set; } // Takım 2'nin şut sayısı
        public int Takim1ToplaOynama { get; set; } // Takım 1'in topla oynama yüzdesi
        public int Takim2ToplaOynama { get; set; } // Takım 2'nin topla oynama yüzdesi

        public override string ToString()
        {
            return $"{Takim1} vs {Takim2} | Tarih: {Tarih} | Hakem: {(AtananHakem != null ? AtananHakem.Ad : "Belirtilmemiş")}";
        }
    }

    class Program
    {
        static Dictionary<string, int> puanDurumu = new Dictionary<string, int>();
        static Random rand = new Random();

        static void GuncelleHakemPerformansi(Hakem hakem, int performansDegisimi)
        {
            hakem.BasariPuani = Math.Max(0, hakem.BasariPuani + performansDegisimi);
        }

        static void GuncellePuanDurumu(string takim1, string takim2, string sonuc)
        {
            if (!puanDurumu.ContainsKey(takim1)) puanDurumu[takim1] = 0;
            if (!puanDurumu.ContainsKey(takim2)) puanDurumu[takim2] = 0;

            if (sonuc == "galibiyet1") puanDurumu[takim1] += 3;
            else if (sonuc == "galibiyet2") puanDurumu[takim2] += 3;
            else if (sonuc == "beraberlik")
            {
                puanDurumu[takim1] += 1;
                puanDurumu[takim2] += 1;
            }
        }

        static List<(string, string)> RastgeleFiksturOlustur(List<string> takimlar)
        {
            List<(string, string)> yeniFikstur = new List<(string, string)>();
            List<string> kalanTakimlar = new List<string>(takimlar);

            while (kalanTakimlar.Count > 1)
            {
                string takim1 = kalanTakimlar[rand.Next(kalanTakimlar.Count)];
                kalanTakimlar.Remove(takim1);
                string takim2 = kalanTakimlar[rand.Next(kalanTakimlar.Count)];
                kalanTakimlar.Remove(takim2);

                yeniFikstur.Add((takim1, takim2));
            }

            return yeniFikstur;
        }

        static void AnaEkranGoster(List<Mac> maclar6, List<Mac> maclar7, List<Hakem> hakemler)
        {
            Console.WriteLine("\nAna Ekran:");
            Console.WriteLine("====================");
            Console.WriteLine("6. Hafta Maçları:");
            foreach (var mac in maclar6)
            {
                Console.WriteLine(mac);
            }

            Console.WriteLine("\n7. Hafta Maçları:");
            foreach (var mac in maclar7)
            {
                Console.WriteLine(mac);
            }

            Console.WriteLine("\nHakem İstatistikleri:");
            foreach (var hakem in hakemler.OrderByDescending(h => h.BasariPuani))
            {
                Console.WriteLine(hakem);
            }
            Console.WriteLine("====================\n");
        }

        static void MacIstatistikleriniGoster(string takimAdi, int hafta, List<Mac> maclar)
        {
            var secilenMaclar = maclar.Where(m => (m.Takim1 == takimAdi || m.Takim2 == takimAdi)).ToList();

            if (secilenMaclar.Any())
            {
                Console.WriteLine($"\n{hafta}. Hafta için {takimAdi} takımının maç istatistikleri:\n");
                foreach (var mac in secilenMaclar)
                {
                    Console.WriteLine($"Maç: {mac.Takim1} {mac.Takim1Gol} - {mac.Takim2Gol} {mac.Takim2}\nŞutlar: {mac.Takim1Sut} - {mac.Takim2Sut}\nTopla Oynama: {mac.Takim1ToplaOynama}% - {mac.Takim2ToplaOynama}%\nSonuç: {mac.Sonuc}");
                }
            }
            else
            {
                Console.WriteLine($"{hafta}. Haftada {takimAdi} takımına ait maç bulunamadı.");
            }
        }

        static void Main(string[] args)
        {
            // 2024-2025 sezonu, 6. hafta fikstürü
            List<(string Takim1, string Takim2, string Tarih)> fikstur = new List<(string, string, string)>
            {
                ("Hatayspor", "Bodrum FK", "20/09/2024"),
                ("Fenerbahçe", "Galatasaray", "21/09/2024"),
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

            // 6. hafta maç oluşturma ve hakem atama
            List<Mac> maclar6 = new List<Mac>();
            List<Hakem> kalanHakemler = new List<Hakem>(hakemler);

            foreach (var (takim1, takim2, tarih) in fikstur)
            {
                if (kalanHakemler.Count == 0)
                {
                    kalanHakemler = new List<Hakem>(hakemler);
                }

                var uygunHakemler = kalanHakemler.OrderByDescending(h => h.BasariPuani).ToList();
                var atananHakem = uygunHakemler.First();

                int takim1Gol = rand.Next(0, 5);
                int takim2Gol = rand.Next(0, 5);
                int takim1Sut = rand.Next(5, 20);
                int takim2Sut = rand.Next(5, 20);
                int takim1ToplaOynama = rand.Next(40, 61);
                int takim2ToplaOynama = 100 - takim1ToplaOynama;

                string sonuc = takim1Gol > takim2Gol ? "galibiyet1" : takim1Gol < takim2Gol ? "galibiyet2" : "beraberlik";

                maclar6.Add(new Mac
                {
                    Takim1 = takim1,
                    Takim2 = takim2,
                    AtananHakem = atananHakem,
                    Tarih = tarih,
                    Sonuc = sonuc,
                    Takim1Gol = takim1Gol,
                    Takim2Gol = takim2Gol,
                    Takim1Sut = takim1Sut,
                    Takim2Sut = takim2Sut,
                    Takim1ToplaOynama = takim1ToplaOynama,
                    Takim2ToplaOynama = takim2ToplaOynama
                });

                GuncelleHakemPerformansi(atananHakem, sonuc == "beraberlik" ? 1 : 3);
                atananHakem.MacSayisi++;
                kalanHakemler.Remove(atananHakem);
            }

            // 7. hafta için fikstür oluşturma ve maç atama
            List<string> takimlar = new List<string> { "Hatayspor", "Bodrum FK", "Fenerbahçe", "Galatasaray", "Beşiktaş", "Eyüpspor", "Gaziantep FK", "Trabzonspor" };
            List<(string, string)> yeniFikstur = RastgeleFiksturOlustur(takimlar);

            List<Mac> maclar7 = new List<Mac>();
            kalanHakemler = new List<Hakem>(hakemler);

            foreach (var (takim1, takim2) in yeniFikstur)
            {
                if (kalanHakemler.Count == 0)
                {
                    kalanHakemler = new List<Hakem>(hakemler);
                }

                var uygunHakemler = kalanHakemler.OrderByDescending(h => h.BasariPuani).ToList();
                var atananHakem = uygunHakemler.First();

                int takim1Gol = rand.Next(0, 5);
                int takim2Gol = rand.Next(0, 5);
                int takim1Sut = rand.Next(5, 20);
                int takim2Sut = rand.Next(5, 20);
                int takim1ToplaOynama = rand.Next(40, 61);
                int takim2ToplaOynama = 100 - takim1ToplaOynama;

                string sonuc = takim1Gol > takim2Gol ? "galibiyet1" : takim1Gol < takim2Gol ? "galibiyet2" : "beraberlik";

                maclar7.Add(new Mac
                {
                    Takim1 = takim1,
                    Takim2 = takim2,
                    AtananHakem = atananHakem,
                    Tarih = "27/09/2024",
                    Sonuc = sonuc,
                    Takim1Gol = takim1Gol,
                    Takim2Gol = takim2Gol,
                    Takim1Sut = takim1Sut,
                    Takim2Sut = takim2Sut,
                    Takim1ToplaOynama = takim1ToplaOynama,
                    Takim2ToplaOynama = takim2ToplaOynama
                });

                GuncelleHakemPerformansi(atananHakem, sonuc == "beraberlik" ? 1 : 3);
                atananHakem.MacSayisi++;
                kalanHakemler.Remove(atananHakem);
            }

            // Ana ekran gösterimi
            AnaEkranGoster(maclar6, maclar7, hakemler);

            // Kullanıcı seçimine göre istatistik gösterme
            Console.WriteLine("Hangi takımın maç istatistiklerini görmek istiyorsunuz? (örneğin: Fenerbahçe)");
            string takimAdi = Console.ReadLine();

            Console.WriteLine("Hangi haftanın maç istatistiklerini görmek istiyorsunuz? (6 veya 7)");
            if (int.TryParse(Console.ReadLine(), out int hafta))
            {
                if (hafta == 6)
                {
                    MacIstatistikleriniGoster(takimAdi, hafta, maclar6);
                }
                else if (hafta == 7)
                {
                    MacIstatistikleriniGoster(takimAdi, hafta, maclar7);
                }
                else
                {
                    Console.WriteLine("Geçersiz hafta numarası girdiniz. Lütfen 6 veya 7 olarak deneyiniz.");
                }
            }
            else
            {
                Console.WriteLine("Geçersiz giriş! Hafta numarası bir sayı olmalıdır.");
            }

            Console.WriteLine("\nProjeyi kapatmak için bir tuşa basın...");
            Console.ReadKey();
        }
    }
}




















