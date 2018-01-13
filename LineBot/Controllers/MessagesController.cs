using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LineBot.Models;
using LineMessagingAPISDK.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LineBot.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
//        // GET api/values
//        [HttpGet]
//        public IEnumerable<string> Get()
//        {
//            return new string[] {"value1", "value2"};
//        }
//        // GET api/messages/5
//        [HttpGet("{id}")]
//        public string Get(int id)
//        {
//            return "msg";
//        }

        // POST api/messages
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string value)
        {
            var activity = JsonConvert.DeserializeObject<Activity>(value);

            // Line may send multiple events in one message, so need to handle them all.
            foreach (Event lineEvent in activity.Events)
            {
                LineMessageHandler handler = new LineMessageHandler(lineEvent);

                Profile profile = await handler.GetProfile(lineEvent.Source.UserId);
                //if(profile == null)
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK);
                //}
                switch (lineEvent.Type)
                {
                    case EventType.Beacon:
                        await handler.HandleBeaconEvent();
                        break;
                    case EventType.Follow:
                        await handler.HandleFollowEvent();
                        break;
                    case EventType.Join:
                        await handler.HandleJoinEvent();
                        break;
                    case EventType.Leave:
                        await handler.HandleLeaveEvent();
                        break;
                    case EventType.Message:
                        Message message = JsonConvert.DeserializeObject<Message>(lineEvent.Message.ToString());
                        switch (message.Type)
                        {
                            case MessageType.Text:
                                await handler.HandleTextMessage(MessageHandler.Current);
                                break;
                            case MessageType.Audio:
                            case MessageType.Image:
                            case MessageType.Video:
                                await handler.HandleMediaMessage();
                                break;
                            case MessageType.Sticker:
                                await handler.HandleStickerMessage();
                                break;
                            case MessageType.Location:
                                await handler.HandleLocationMessage();
                                break;
                        }
                        break;
                    case EventType.Postback:
                        await handler.HandlePostbackEvent();
                        break;
                    case EventType.Unfollow:
                        await handler.HandleUnfollowEvent();
                        break;
                }
            }

            return this.Ok();

        }
    }
}