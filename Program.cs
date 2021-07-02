using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using System.IO;

namespace Example_941
{
    class Program
    {
        static TelegramBotClient bot;
        static void Main(string[] args)
        {
            string token = File.ReadAllText(@"token.txt");

            WebClient wc = new WebClient() {Encoding = Encoding.UTF8 };

            int update_id = 0;
            string startUrl = $@"https://api.telegram.org/bot{token}/";

            string  url = $"{startUrl}getUpdates?offset={update_id}";

            bot = new TelegramBotClient(token);
            bot.OnMessage += MessageListener;
            bot.StartReceiving();
            Console.ReadKey();



            #region ДЗ
            // Создать бота, позволяющего принимать разные типы файлов, 
            // *Научить бота отправлять выбранный файл в ответ
            // 
            // https://data.mos.ru/
            // https://apidata.mos.ru/
            // 
            // https://vk.com/dev
            // https://vk.com/dev/manuals

            // https://dev.twitch.tv/
            // https://discordapp.com/developers/docs/intro
            // https://discordapp.com/developers/applications/
            // https://discordapp.com/verification
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Вся информация о сообщениях</param>
        private static void MessageListener(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Random rand = new Random();
            string token = File.ReadAllText(@"token.txt");
            WebClient wc = new WebClient() { Encoding = Encoding.UTF8 };
            string text = $"{DateTime.Now.ToLongTimeString()}: {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}";

            int update_id = 0;
            string startUrl = $@"https://api.telegram.org/bot{token}/";

            int user_Id = e.Message.From.Id;

            string url = $"{startUrl}getUpdates?offset={update_id}";

            Console.WriteLine($"{text} TypeMessage: {e.Message.Type.ToString()}");


            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                Console.WriteLine(e.Message.Document.FileId);
                Console.WriteLine(e.Message.Document.FileName);
                Console.WriteLine(e.Message.Document.FileSize);

                DownLoad(e.Message.Document.FileId, e.Message.Document.FileName);
            }

            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Audio)
            {
                Console.WriteLine(e.Message.Audio.FileId);
                Console.WriteLine($"{e.Message.Audio.Performer} - {e.Message.Audio.Title}");
                Console.WriteLine(e.Message.Audio.FileSize);

                DownLoad(e.Message.Audio.FileId, $"{e.Message.Audio.Performer} - {e.Message.Audio.Title}.mp3");
            }

            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
            {
                Console.WriteLine(e.Message.Photo[0].FileId);
                Console.WriteLine(e.Message.Photo[0].FileSize);
                Console.WriteLine($"H: {e.Message.Photo[0].Height}");
                Console.WriteLine($"W: {e.Message.Photo[0].Width}");

                DownLoad(e.Message.Photo[0].FileId, $"{e.Message.Photo[0].FileUniqueId}.jpeg");
            }

            if (e.Message.Text == "Фото" || e.Message.Text == "фото")
            {
                url = $"{startUrl}sendPhoto?chat_id={user_Id}&photo={@"https://memegenerator.net/img/images/9661155.jpg"}";
                int what = rand.Next(1, 4);
                if (what == 1) bot.SendPhotoAsync(e.Message.Chat.Id, $"https://memegenerator.net/img/images/9661155.jpg");
                if (what == 2) bot.SendPhotoAsync(e.Message.Chat.Id, $"https://www.factinate.com/wp-content/uploads/2018/06/7787147587_existe-t-il-differentes-formes-de-schizophrenie.jpg");
                if (what == 3) bot.SendPhotoAsync(e.Message.Chat.Id, $"https://img.fruugo.com/product/6/68/51323686_max.jpg");
            }

            if (e.Message.Text == null) return;

            var messageText = e.Message.Text;


            bot.SendTextMessageAsync(e.Message.Chat.Id,
                $"{ messageText}"
                );
        }

        static async void DownLoad(string fileId, string path)
        {
            var file = await bot.GetFileAsync(fileId);
            FileStream fs = new FileStream("_" + path, FileMode.Create);
            await bot.DownloadFileAsync(file.FilePath, fs);
            fs.Close();

            fs.Dispose();
        }

    }
}

