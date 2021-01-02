using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SohbetBaloncuklari.Hubs
{
    public class SohbetHub : Hub
    {
        private static List<Kullanici> kullanicilar = new List<Kullanici>();

        public async Task SohbeteGir(string takmaAd)
        {
            var kullanici = new Kullanici()
            {
                Id = Guid.NewGuid().ToString(),
                ConnectionId = Context.ConnectionId,
                TakmaAd = takmaAd
            };
            kullanicilar.Add(kullanici);

            // önce sohbete girene atanan id'sini gönderelim
            await Clients.Caller.SendAsync("SohbeteGirildi", kullanici.Id, 
                kullanicilar.Select(x => new { x.Id, x.TakmaAd, x.X, x.Y }));

            // sohbete yeni kullanıcı girdiğini insanlığa duyur
            await Clients.Others.SendAsync("KullaniciSohbeteGirdi", kullanici.Id, kullanici.TakmaAd);
        }

        public async Task MesajGonder(string mesaj)
        {
            var kullanici = kullanicilar.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            // gelen mesajı herkese gönder
            await Clients.All.SendAsync("YeniMesajAlindi", kullanici.Id, kullanici.TakmaAd, mesaj);
        }

        public async Task KonumGonder(int x, int y)
        {
            var kullanici = kullanicilar.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            kullanici.X = x;
            kullanici.Y = y;
            await Clients.Others.SendAsync("KullaniciKonumDegistirdi", kullanici.Id, x, y);
        }

        // bir istemcinin bağlantısı koptuğunda..
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var kullanici = kullanicilar.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            kullanicilar.Remove(kullanici);

            // kullanıcının sohbetten çıktığını herkese duyur
            await Clients.All.SendAsync("KullaniciSohbettenCikti", kullanici.Id, kullanici.TakmaAd);
        }
    }
}
