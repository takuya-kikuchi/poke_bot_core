﻿using LineMessagingAPISDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LineBot.Models
{
    public class MessageHandler
    {
        public static MessageHandler Current { get; } = new MessageHandler();
        ContextManager ContextManager { get; } = new ContextManager();
        public static string MoritaID { get; } = "C2bd1a6e22bb47a6b0cda7bf759519113";

        public string HandleTextMessage(string userID, TextMessage msg)
        {
            if(msg.Text == "わすれて")
            {
                this.ContextManager.Forget(userID);
                return "さようなら";
            }
            if(msg.Text == "BOTさんおやすみなさい")
            {
                this.ContextManager.ForgetAll();
                return "おやすみなさい...";
            }
            if(msg.Text == "@dice")
            {
                return new Random(DateTime.Now.Second).Next(0, 100).ToString();
            }
            if (msg.Text == "@dice99")
            {
                return "99";
            }
            if (msg.Text == "もりた")
            {
                return "(笑)";
            }
            if(userID == MoritaID)
            {
                if(new Random(DateTime.Now.Second).Next(0, 100) > 70)
                {
                    return "はいはいもりたもりた";
                }
            }

            return this.ContextManager.GetContextOf(userID).GetReplyMessage(msg.Text);
        }
    }
}