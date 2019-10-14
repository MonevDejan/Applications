using System;
using System.Collections.Generic;
using System.Text;

namespace SafeSportChat.Models
{
    public class Conversation
    {
        public int Id { get; set; }
        public string WithName { get; set; }
        public string LastMessage { get; set; }
        public int NumberOfParticipants { get; set; }
        public DateTime LastActivity { get; set; }

        public string LastActivityRelative { get; set; }

        public bool IsHighlightedForREview { get; set; }
        public bool HasLike { get; set; }
        public bool ShowLastMessage { get { return !HasLike; } }

        public bool HasAlert{ get; set; }
        public bool HasAttachment { get; set; }
        public bool HasTimeoutAlert { get; set; }
        public bool IsFlagged { get; set; }

    }
}
