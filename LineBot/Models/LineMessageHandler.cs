using System.Threading.Tasks;
using LineMessagingAPISDK;
using LineMessagingAPISDK.Models;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;

namespace LineBot.Models
{
    public class LineMessageHandler
    {
        private Event lineEvent;
        private LineClient lineClient = new LineClient("DUMMY");

        public LineMessageHandler(Event lineEvent)
        {
            this.lineEvent = lineEvent;
        }

        public async Task HandleBeaconEvent()
        {
        }

        public async Task HandleFollowEvent()
        {
        }

        public async Task HandleJoinEvent()
        {
        }

        public async Task HandleLeaveEvent()
        {
        }

        public async Task HandlePostbackEvent()
        {
            var replyMessage = new TextMessage(lineEvent.Postback.Data);
            await Reply(replyMessage);
        }

        public async Task HandleUnfollowEvent()
        {
        }

        public async Task<Profile> GetProfile(string mid)
        {
            return await lineClient.GetProfile(mid);
        }

        public async Task HandleTextMessage(MessageHandler handler)
        {
            var textMessage = JsonConvert.DeserializeObject<TextMessage>(lineEvent.Message.ToString());

            var reply = handler.HandleTextMessage(lineEvent.Source.UserId + lineEvent.Source.GroupId, textMessage);

            await Reply(new TextMessage(reply));
        }



        public async Task HandleMediaMessage()
        {
            Message message = JsonConvert.DeserializeObject<Message>(lineEvent.Message.ToString());
            // Get media from Line server.
            Media media = await lineClient.GetContent(message.Id);
            Message replyMessage = null;

            // Reply Image 
            switch (message.Type)
            {
                case MessageType.Image:
                case MessageType.Video:
                case MessageType.Audio:
                    replyMessage = new ImageMessage("https://github.com/apple-touch-icon.png", "https://github.com/apple-touch-icon.png");
                    break;
            }

            await Reply(replyMessage);
        }

        public async Task HandleStickerMessage()
        {
            //https://devdocs.line.me/files/sticker_list.pdf
            //var stickerMessage = JsonConvert.DeserializeObject<StickerMessage>(lineEvent.Message.ToString());
            //var replyMessage = new StickerMessage("1", "1");
            //await Reply(replyMessage);
        }

        public async Task HandleLocationMessage()
        {
            var locationMessage = JsonConvert.DeserializeObject<LocationMessage>(lineEvent.Message.ToString());
            LocationMessage replyMessage = new LocationMessage(
                locationMessage.Title,
                locationMessage.Address,
                locationMessage.Latitude,
                locationMessage.Longitude);
            await Reply(replyMessage);
        }

        private async Task Reply(Message replyMessage)
        {
            try
            {
                await lineClient.ReplyToActivityAsync(lineEvent.CreateReply(message: replyMessage));
            }
            catch
            {
                await lineClient.PushAsync(lineEvent.CreatePush(message: replyMessage));
            }
        }
    }
}