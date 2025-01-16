using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace SuperLigHakemSecimi
{
    public class Kullanici
    {
        public string Email { get; set; }
        public string Sifre { get; set; }
        public List<string> FavoriTakimlar { get; set; } = new List<string>();

        public Kullanici(string email, string sifre)
        {
            Email = email;
            Sifre = sifre;
        }
    }

    public class Mac
    {
        public string Takim1 { get; set; }
        public string Takim2 { get; set; }
        public string Tarih { get; set; }
        public int Takim1Gol { get; set; }
        public int Takim2Gol { get; set; }
        public int Takim1Sut { get; set; }
        public int Takim2Sut { get; set; }
        public int Takim1ToplaOynama { get; set; }
        public int Takim2ToplaOynama { get; set; }
        public string Sonuc { get; set; }

        public string GenelBilgi()
        {
            return $"{Takim1} vs {Takim2} | Tarih: {Tarih}";
        }

        public override string ToString()
        {
            return $@"
                Maç: {Takim1} {Takim1Gol} - {Takim2Gol} {Takim2}
                Şutlar: {Takim1Sut} - {Takim2Sut}
                Topla Oynama: {Takim1ToplaOynama}% - {Takim2ToplaOynama}%
                Sonuç: {Sonuc}";
        }
    }

    class Program
    {
        static List<Kullanici> kullanicilar = new List<Kullanici>();
        static Kullanici aktifKullanici = null;
        static List<Mac> maclar = new List<Mac>
        {
            new Mac { Takim1 = "Fenerbahçe", Takim2 = "Galatasaray", Tarih = "2025-02-19", Takim1Gol = 1, Takim2Gol = 3, Takim1Sut = 25, Takim2Sut = 14, Takim1ToplaOynama = 56, Takim2ToplaOynama = 44, Sonuc = "Galatasaray Kazandı" },
            new Mac { Takim1 = "Beşiktaş", Takim2 = "Eyüpspor", Tarih = "2025-02-12", Takim1Gol = 2, Takim2Gol = 1, Takim1Sut = 15, Takim2Sut = 11, Takim1ToplaOynama = 62, Takim2ToplaOynama = 38, Sonuc = "Beşiktaş Kazandı" }
        };

        static void MailGonder(string email, string konu, string mesaj)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("futbolmackolik3@gmail.com", "wclajkwvhpjtvvfy"),
                    EnableSsl = true
                };

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("futbolmackolik3@gmail.com"),
                    Subject = konu,
                    Body = mesaj,
                    IsBodyHtml = true
                };
                mail.To.Add(email);

                client.Send(mail);
                Console.WriteLine("Mail başarıyla gönderildi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mail gönderimi başarısız: " + ex.Message);
            }
        }

        static void MacIstatistikleriniGonder(Mac secilenMac, int hafta)
        {
            string mailMesaj = $@"<h1>{hafta}. Hafta Maç İstatistiği</h1>
                <p>Maç: {secilenMac.Takim1} {secilenMac.Takim1Gol} - {secilenMac.Takim2Gol} {secilenMac.Takim2}</p>
                <p>Şutlar: {secilenMac.Takim1Sut} - {secilenMac.Takim2Sut}</p>
                <p>Topla Oynama: {secilenMac.Takim1ToplaOynama}% - {secilenMac.Takim2ToplaOynama}%</p>
                <p>Sonuç: {secilenMac.Sonuc}</p>";
            MailGonder(aktifKullanici.Email, $"{hafta}. Hafta Maç İstatistiği", mailMesaj);
        }

        static void KullaniciKayit()
        {
            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Şifre: ");
            string sifre = Console.ReadLine();

            var yeniKullanici = new Kullanici(email, sifre);
            Console.Write("Favori takımlarınızı virgülle ayırarak girin: ");
            yeniKullanici.FavoriTakimlar = Console.ReadLine().Split(',').Select(t => t.Trim()).ToList();
            kullanicilar.Add(yeniKullanici);

            Console.WriteLine("Kayıt başarılı!");
        }

        static void AnaMenu()
        {
            while (true)
            {
                Console.WriteLine("\nAna Menü");
                Console.WriteLine("1. Kayıt Ol");
                Console.WriteLine("2. Giriş Yap");
                Console.WriteLine("3. Çıkış");
                Console.Write("Seçiminizi yapın: ");

                string secim = Console.ReadLine();

                if (secim == "1")
                {
                    KullaniciKayit();
                }
                else if (secim == "2")
                {
                    Console.Write("Email: ");
                    string email = Console.ReadLine();

                    Console.Write("Şifre: ");
                    string sifre = Console.ReadLine();

                    aktifKullanici = kullanicilar.FirstOrDefault(k => k.Email == email && k.Sifre == sifre);

                    if (aktifKullanici != null)
                    {
                        Console.WriteLine("Giriş başarılı!");
                        KullaniciMenu();
                    }
                    else
                    {
                        Console.WriteLine("Email veya şifre hatalı!");
                    }
                }
                else if (secim == "3")
                {
                    Console.WriteLine("Çıkış yapılıyor. Hoşça kalın!");
                    break;
                }
                else
                {
                    Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                }
            }
        }

        static void KullaniciMenu()
        {
            Console.WriteLine("\nMaç Listesi:");
            for (int i = 0; i < maclar.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {maclar[i].GenelBilgi()}");
            }

            Console.Write("Hangi maçı görmek istiyorsunuz? (Maç numarası): ");
            if (int.TryParse(Console.ReadLine(), out int macNo) && macNo > 0 && macNo <= maclar.Count)
            {
                var secilenMac = maclar[macNo - 1];
                Console.Write("Bu maçın istatistiklerini e-posta ile göndermek istiyor musunuz? (E/H): ");
                if (Console.ReadLine()?.ToUpper() == "E")
                {
                    MacIstatistikleriniGonder(secilenMac, 6);
                }
            }
        }

        static void Main(string[] args)
        {
            AnaMenu();
        }
    }
}
