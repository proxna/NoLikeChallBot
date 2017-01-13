using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;

namespace AntiLikeChallenge
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        const string REPLY_MESSAGE_TEXT = "Zarób se jebany żebraku";

        string[] challengeList = {"wyzwanie", "challenge", "challengi", "wyzwaniach", "wyzwania", "wyzywam" };
        string[] likeList = {"like", "lajk", "lajków", "lajkow", "łapek", "łapki", "polubień", "polubienia", "likow"};
        string[] comList = {"komentarzy","kom","komentarzów", "komentarzow"};

        int messageIsChall = 0;
        bool moveNext = false;

        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                string[] words = message.Text.Split(' ');
                foreach(string word in words)
                {
                    foreach(string wordFromList in challengeList)
                    {
                        if (wordFromList == word)
                        {
                            messageIsChall += 2;
                            moveNext = true;
                            break;
                        }
                    }
                    if (moveNext)
                    {
                        moveNext = false;
                        continue;
                    }
                    foreach (string wordFromList in likeList)
                    {
                        if (wordFromList == word)
                        {
                            messageIsChall += 3;
                            moveNext = true;
                            break;
                        }
                    }
                    if (moveNext)
                    {
                        moveNext = false;
                        continue;
                    }
                    foreach (string wordFromList in comList)
                    {
                        if (wordFromList == word)
                        {
                            messageIsChall += 4;
                            break;
                        }
                    }
                }
                if (messageIsChall >= 5)
                {
                    messageIsChall = 0;
                    return message.CreateReplyMessage(REPLY_MESSAGE_TEXT);
                }
                
                return null;                
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}