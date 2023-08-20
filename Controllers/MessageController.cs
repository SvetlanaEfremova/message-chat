using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;
using task6.Models;
using System.Text.RegularExpressions;
using Azure;

namespace task6.Controllers
{
    public class MessageController : Controller
    {
        private ApplicationContext dbcontext;

        public MessageController(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Chat()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewMessage([FromBody] MessageInputModel messageModel)
        {
            CreateNewMessage(messageModel);
            return RedirectToAction("Chat");
        }

        private void CreateNewMessage(MessageInputModel messageModel)
        {
            var tagsFromMessageText = GetTagsFromMessageText(messageModel);
            var allTags = tagsFromMessageText.Concat(messageModel.Tags).Distinct().ToList();
            var messageTextWithoutTags = Regex.Replace(messageModel.Text, @"#\w+", "").Trim();
            var message = new Message { Text = messageTextWithoutTags, MessagesAndTags = new List<MessageTag>() };
            AddNewMessageToDb(message, allTags);
        }

        private List<string> GetTagsFromMessageText(MessageInputModel messageModel)
        {
            return Regex.Matches(messageModel.Text, @"#\w+")
                .Cast<Match>()
                .Select(match => match.Value.Substring(1))
                .ToList();
        }

        private void AddNewMessageToDb(Message message, List<string> tagNames)
        {
            dbcontext.Messages.Add(message);
            UpdateTagsInDb(tagNames, message);
            dbcontext.SaveChanges();
        }

        private void UpdateTagsInDb(List<string> tagNames, Message message)
        {
            foreach (var tagName in tagNames)
            {
                UpdateDataForTag(tagName, message);
            }
        }

        private void UpdateDataForTag(string tagName, Message message)
        {
            var tag = dbcontext.Tags.FirstOrDefault(t => t.Name == tagName);
            if (tag == null)
            {
                tag = new Tag { Name = tagName, MessagesAndTags = new List<MessageTag>() };
                dbcontext.Tags.Add(tag);
            }
            else if (tag.MessagesAndTags == null) tag.MessagesAndTags = new List<MessageTag>();
            AddMessageTagRelation(message, tag);
        }

        private void AddMessageTagRelation(Message message, Tag tag)
        {
            var messageTag = new MessageTag { Message = message, Tag = tag };
            tag.MessagesAndTags.Add(messageTag);
            message.MessagesAndTags.Add(messageTag);
        }

        [HttpPost]
        public IActionResult GetTaggedMessages([FromBody] TagsRequest request)
        {
            var taggedMessages = GetTaggedMessagesFromDb(request);
            var nonTaggedMessages = GetNonTaggedMessagesFromDb();
            var messages = taggedMessages.Concat(nonTaggedMessages).ToList();
            var taggedMessagesDto = GetMessagesDto(messages);
            return Json(taggedMessagesDto);
        }

        private List<Message> GetTaggedMessagesFromDb(TagsRequest request)
        {
            return dbcontext.MessageTags
                .Where(mt => request.SelectedTags.Contains(mt.Tag.Name))
                .Include(mt => mt.Message)
                .ThenInclude(m => m.MessagesAndTags)
                .ThenInclude(mt => mt.Tag)
                .Select(mt => mt.Message).Distinct().ToList();
        }

        private List<Message> GetNonTaggedMessagesFromDb()
        {
            return dbcontext.Messages
                .Where(m => !m.MessagesAndTags.Any())
                .ToList();
        }

        private List<MessageDto> GetMessagesDto(List<Message> messages)
        {
            return messages.Select(m => new MessageDto
            {
                Id = m.Id,
                Text = m.Text,
                Tags = m.MessagesAndTags?.Select(mt => mt.Tag.Name).ToList() ?? new List<string>()
            }).ToList();
        }

        public IActionResult GetAllMessages()
        {
            var messagesDto = dbcontext.Messages.ToList().Select(m => new MessageDto
            {
                Id = m.Id,
                Text = m.Text,
                Tags = m.MessagesAndTags.Select(mt => mt.Tag.Name).ToList()
            }).ToList();
            return Json(messagesDto);
        }

        [HttpGet]
        public IActionResult GetAllTags()
        {
            var allTags = GetTagList();
            return Json(allTags);
        }

        private List<string> GetTagList()
        {
            var allTagNames = new List<string>();
            var tags = dbcontext.Tags.ToList();
            foreach (var tag in tags)
                allTagNames.Add(tag.Name);
            return allTagNames;
        }
    }
}
